/* STARTUP CLASS
 *
 * When an ASP.NET Core application starts, the first thing it will do in Main() is to boot
 * the Webhost. The host will try to configure the application and to do that, it will look to
 * the Startup Class for host and server configuration information, Middleware Services to 
 * load and the Request Handling Pipeline.
 *
 *
 * 
 * 
 * STARTUP.CS
 * 
 * Do note that in the process of setting up the Host, the WebHost class's CreateDefaultBuilder()
 * method calls the UserStartup() method and passes in the Startup class. It is mandatory that this
 * class be included in every ASP.NET Core application, but it does not have to be called 'Startup'.
 * What does it do? This class contains crucial configuration information that the Host needs to
 * know. 
 *
 * Recall that the Host will configure and boot up these two things at a bare minimum:
 *
 *     1. A Web Server: to handle incoming HTTP requests,
 *     2. A Middleware Pipeline: to process and respond to these requests.
 *
 * The Startup class has two methods:
 *
 *     1. ConfigureServices(): This method is called before Configure() by the Web host. In
 *        this method, you can add and configure Services that you want to add to your
 *        application. 
 *     2. Configure(): This method configures the middleware pipeline, determining how the
 *        application will respond to requests. Any Services that were added in
 *        ConfigureServices() are availabe for use in Configure().
 * 
 *
 * MIDDLEWARE
 *
 * See this link for more information: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/index?tabs=aspnetcore2x
 * 
 * A browser sends a request to a server. This application will be running on the server, so it
 * needs to interpret incoming requests. Requests come in Context objects. Context objects and the
 * requests contained therein, are managed by ASP.NET Core's Middleware. The Middleware can be 
 * thought of as a series of pipes that decide what to do with incoming requests.
 *
 * When a request reaches the server, ASP.NET Core's Middleware handles it. The first "pipe-juncture"
 * checks if the request requires an immediate response message. If no response is necessary, the
 * request message (as a Context object) passes to the next "pipe-juncture". It's possible for a
 * request to go through the whole middleware stack without a response message being sent back,
 * which the client would interpret as a 404 error message (Resource Not Found). Sometimes, a
 * "pipe-juncture" returns a response (which is attached to the Context object), but typically, the
 * request message goes all the way to last "pipe-juncture", which will respond. The response
 * message passes through the pipeline and the server sends it back to the client.
 *
 * MVC is a piece of middlware that handles requests and responses. You need to add MVC to the
 * middleware pipeline. There are three steps to adding MVC to our project:
 *
 * 1. Add MVC to the Project's References: All ASP.NET Core projects should include the
 *    Microsoft.AspNetCore.All package. If it's missing, add it to the project.
 * 
 * 2. Add MVC to the Services: A service is a pre-requisite component that supports MVC and other 
 *    bits of the middleware during the functioning of the program. Go to Startup.cs and in the
 *    ConfigureServices() method, include this line to add MVC services to the services collection:
 *
 *        services.AddMvc();
 * 
 * 3. Add MVC to the Middleware Pipeline: In this step, I will add MVC to the pipeline, but this
 *    has to done carefully. Here is a sample Startup.cs. Take a look at the Configure() method:
 *
 *        if (env.IsDevelopment())
 *        {
 *            app.UseDeveloperExceptionPage();
 *        }
 *
 *        app.UseMvc();
 *        app.Run(async (context) => { await context.Response.WriteAsync("Hello World!"); });
 *
 *    The if statement is running an environment status check, which is one of the first things
 *    an ASP.NET Core project does when it starts. I don't want to add MVC before this check. The
 *    last line is  Run() method, which sends a response to incoming requests. If MVC is added to
 *    the pipeline at this point, it would never get turned on until after the responses message
 *    had been sent out. That's why adding MVC to the middleware pipeline is the second instruction.
 * 
 * 
 **************************************************************************************************
 **************************************************************************************************
 * 
 * 
 * If you right-click the Hotel-Server project name in Solution Explorer and go to Properties, you
 * can find the App URL in the Debug tab. 
 * 
 * 

 */


using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Hotel_Server.Database;

using Common.Models;


using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Hotel_Server
{
    /// <summary>
    /// "ASP.NET Core apps use a Startup class, which is named Startup by convention. The Startup
    /// class:
    /// 
    ///     * Can optionally include a ConfigureServices method to configure the app's services.
    ///     * Must include a Configure method to create the app's request processing pipeline.
    /// 
    /// ConfigureServices and Configure are called by the runtime when the app starts..."[1]
    /// 
    /// Sources
    /// 1: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup
    /// </summary>
    public class Startup
    {
        // 
        // This property, Configuration, "represents a set of key/value application configuration
        // properties". In short, this property contains the config information that the Host needs.
        // It appears that, the Webhost object loads default information into this object when it 
        // creates a Startup object.
        public IConfiguration Configuration { get; }
        
        
        
        /* STARTUP CLASS CONSTRUCTOR
         *
         * You can use the constructor to inject some internal services and configure them.
         * Examples:
         *
         *     * IApplicationBuilder is an interface that provides the mechanisms to configure
         *       an application's request pipeline.
         *
         *     * IHostingEnvironment is an interface that contains information related to the
         *       web hosting environment on which application is running. Using this interface
         *       method, we can change behaviour of the application.
         *
         *     * ILoggerFactory is an interface that provides configuration for the logging
         *       system in ASP.NET Core. It also creates an instance of a logging system.
         * 
         */
        public Startup(IConfiguration configuration, ILoggerFactory log)
        {
            Configuration = configuration;
        }
        

        /* CONFIGURESERVICES() METHOD
         *
         * The Webhost consults this method to find the list of services to add to the container.
         * "The ConfigureServices method is:
         *     * Optional.
         *     * Called by the web host before the Configure method to configure the app's services.
         *     * Where configuration options are set by convention.
         * 
         * Adding services to the service container makes them available within the app and in the
         * Configure method. The services are resolved via dependency injection or from
         * IApplicationBuilder.ApplicationServices.
         * 
         * The web host may configure some services before Startup methods are called. Details are
         * available in the Hosting topic."[1]
         */
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            
            // See Database Setup.txt, Section 3.3.
            // If you want to configure configure the Context class, DbContextOptions and the 
            // Connection string via Dependency Injection, this is the place to do it. Here
            // is an example: 

//            var connectionString = "Host=localhost;" +
//                                   "Username=postgres;" +
//                                   "Password=password;" +
//                                   "Database=HotelReservationSystemDB";
//            services.AddDbContext<Context>(options => options.UseNpgsql(connectionString));
        }
        

        /* CONFIGURE METHOD()
         *
         * "The Configure method is used to specify how the app responds to HTTP requests. The
         * request pipeline is configured by adding middleware components to an IApplicationBuilder
         * instance. IApplicationBuilder is available to the Configure method, but it isn't
         * registered in the service container. Hosting creates an IApplicationBuilder and passes
         * it directly to Configure (reference source).
         *
         * The ASP.NET Core templates configure the pipeline with support for a developer exception
         * page, BrowserLink, error pages, static files, and ASP.NET MVC:" [1]
         * 
         * This method is a part of ASP.NET Core Middleware. It gets called by the runtime. Use it
         * to configure the HTTP request pipeline. It uses an IApplicationBuilder object to decide
         * what to do with incoming requests. This line responds to all incoming requests with
         * a 'Hello World!' message:
         *
         *    app.Run(async (context) => { await context.Response.WriteAsync("Hello World!"); });
         *
         * If that line were added to the Configure method right after the if-statement (and this
         * using statement, "using Microsoft.AspNetCore.Http;"), this server application would
         * respond to every request with a response that printed "Hello World" to the screen. 
         */ 
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
