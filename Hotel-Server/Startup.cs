/* STARTUP CLASS
 *
 * The Startup class contains configuration information for the Request Processing
 * Pipeline and Middleware Services. In Program.cs, when the Host is being configured,
 * the WebHost class's CreateDefaultBuilder() method calls the UserStartup() method and
 * passes in the Startup class and all its config data. It is mandatory that this class
 * be included in very ASP.NET Core application, but it does not have to be called
 * 'Startup', but by convention, that is the class name.
 * 
 * The Startup class has two methods:
 *
 *     1. ConfigureServices(): This method is called before Configure() by the Web host.
 *        In this method, you can add and configure Services that you want to add to your
 *        application. This method is optional.
 *     
 *     2. Configure(): Startup class must include this method. This method configures the
 *        middleware pipeline, determining how the application will respond to requests.
 *        Any Services that were added in ConfigureServices() are available for use in
 *        Configure().
 * 
 *
 * MIDDLEWARE
 *
 * See this link for more information: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/index?tabs=aspnetcore2x
 *
 * When a browser sends a request to a server, it comes in the form of a Context object.
 * Your application's Request Processing Pipeline will have to interpret these objects.
 * Context objects and the requests contained therein, are managed by ASP.NET Core's
 * Middleware. The Middleware can be thought of as a series of pipes that decide what to
 * do with incoming requests.
 * 
 * When a request reaches the server, ASP.NET Core's Middleware handles it. The first
 * "pipe-juncture" checks if the request requires an immediate response message. If no
 * response is necessary, the request message (as a Context object) passes to the next
 * "pipe-juncture". It's possible for a request to go through the whole middleware stack
 * without a response message being sent back, which the client would interpret as a 404
 * error message (Resource Not Found). Sometimes, a "pipe-juncture" returns a response
 * (which is attached to the Context object), but typically, the request message goes all
 * the way to last "pipe-juncture", which will respond. The response message passes
 * through the pipeline and the server sends it back to the client.
 *
 * MVC is a piece of middlware that handles requests and responses. You need to add MVC
 * to the middleware pipeline. There are three steps to adding MVC to our project:
 *
 * 1. Add MVC to the Project's References: All ASP.NET Core projects should include the
 *    Microsoft.AspNetCore.All package. If it's missing, add it to the project.
 * 
 * 2. Add MVC to the Services: A service is a pre-requisite component that supports MVC
 *    and other bits of the middleware during the functioning of the program. Go to
 *    Startup.cs and in the ConfigureServices() method, include this line to add MVC
 *    services to the services collection:
 *
 *        services.AddMvc();
 * 
 * 3. Add MVC to the Middleware Pipeline: In this step, I will add MVC to the pipeline,
 *    but this has to done carefully. Here is a sample Startup.cs. Take a look at the
 *    Configure() method:
 *
 *        if (env.IsDevelopment())
 *        {
 *            app.UseDeveloperExceptionPage();
 *        }
 *
 *        app.UseMvc();
 *        app.Run(async (context) => { await context.Response.WriteAsync("Hello World!"); });
 *
 *    The if statement is running an environment status check, which is one of the first
 *    things an ASP.NET Core project does when it starts. I don't want to add MVC before
 *    this check. The last line is  Run() method, which sends a response to incoming
 *    requests. If MVC is added to the pipeline after this point, it would never get
 *    turned on until after the response message had been sent out. That's why adding MVC
 *    to the middleware pipeline is the second instruction.
 *
 *
 * SOURCES
 *
 * 1: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup
 * 
 **************************************************************************************************
 **************************************************************************************************
 * 
 * 
 * If you right-click the Hotel-Server project name in Solution Explorer and go to
 * Properties, you can find the App URL in the Debug tab. 
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
    /// "ASP.NET Core apps use a Startup class, which is named Startup by convention. The
    /// Startup class:
    /// 
    ///     * Can optionally include a ConfigureServices method to configure the app's
    ///       services.
    ///     * Must include a Configure method to create the app's request processing
    ///       pipeline.
    /// 
    /// ConfigureServices and Configure are called by the runtime when the app starts..."[1]
    /// </summary>
    public class Startup
    {
        // ICONFIGURATION: A BUNDLE OF CONFIG DATA 
        //
        // This property, Configuration, "represents a set of key/value application
        // configuration properties". In short, this property contains the config
        // information that the Host needs. The Webhost object loads default information
        // into this object when it creates a Startup object.
        //
        // The other methods in Startup can read this property.
        public IConfiguration Configuration { get; }
        
        
        
        /* STARTUP CLASS CONSTRUCTOR
         *
         * You can use the constructor to inject some internal services and configure them.
         * Examples:
         *
         *     * IConfiguration is an interface that represents a set of key/value
         *       application configuration properties. The IConfiguration object will
         *       include config values loaded by WebHostBuilder. This bundle of config
         *       values will made available to your application via the Dependency
         *       Injection. See the IConfiguration property above.
         *
         *     * IApplicationBuilder is an interface that provides the mechanisms to
         *       configure an application's request pipeline.
         *
         *     * IHostingEnvironment is an interface that contains information related to
         *       the web hosting environment on which application is running. Using this
         *       interface method, we can change behaviour of the application based on
         *       the environment its running in.
         *
         *     * ILoggerFactory is an interface that provides configuration for the
         *       logging system in ASP.NET Core. It also creates an instance of a logging
         *       system.
         */
        public Startup(IConfiguration configuration, ILoggerFactory log)
        {
            Configuration = configuration;
        }
        

        /* CONFIGURESERVICES() METHOD
         *
         * The Webhost consults this method to find the list of services that your app
         * needs. 
         *
         * 
         *     The ConfigureServices method is:
         * 
         *         * Optional.
         *         * Called by the web host before the Configure method to configure the
         *           app's services.
         *         * Where configuration options are set by convention.
         *
         *     Adding services to the service container makes them available within the
         *     app and in the Configure method. The services are resolved via dependency
         *     injection or from IApplicationBuilder.ApplicationServices.
         * 
         *     The web host may configure some services before Startup methods are called.
         *     Details are available in the Hosting topic.[1]
         *
         *
         * The IServiceCollection allows us to register any services our application code
         * depends on with the Dependency Injection Framework. When your application is
         * started, a service provider will be built using this collection, which can
         * then resolve and inject required dependencies into application's classes. For
         * example, dependencies can be injected into Controller classes. 
         * 
         */
        // This method gets called by the runtime. Use this method to add services to the
        // (IServiceCollection) container.
        public void ConfigureServices(IServiceCollection services)
        {
            // This adds a number of services related to ASP.NET Core's MVC collection to
            // IServiceCollection. Normally, services are grouped together like this.
            services.AddMvc();
            
            // DECLARING DBCONTEXT AND MAKING IT AVAILABLE AS A SERVICE VIA 
            // DEPENDENCY INJECTION
            //
            // See 'Step 4: Configuring the Instantiation of the Context Object' in
            // Context.cs for more information about this section. The short version: 
            // the Context object needs to have a connection string and other
            // configuration data loaded into it. There are several way to do this. This
            // project uses Approach 2. The segment below illustrates an alternate 
            // implementation, Approach 3. 
            //
            // If you want to configure configure the Context class, DbContextOptions and
            // the Connection string via Dependency Injection, this is the place to do
            // it. Here is an example: 

//            var connectionString = "Host=localhost;" +
//                                   "Username=postgres;" +
//                                   "Password=password;" +
//                                   "Database=HotelReservationSystemDB";
//            services.AddDbContext<Context>(options => options.UseNpgsql(connectionString));
        }
        

        /* CONFIGURE METHOD()
         *
         * This method will let you configure the ASP.NET Core Middleware Pipeline. This
         * will let you control how your application will process and respond to HTTP
         * requests. This method can also initialize dependencies which have been
         * injected by the Dependency Injection framework in the ConfigureServices()
         * method.
         *
         * The Configure() method has two parameters: IApplicationBuilder and
         * IHostingEnvironment. The first will be used to build the middleware pipeline
         * and the second will provide runtime information about how the application is
         * being hosted. The first thing Configure() does is check what environment it is
         * running in.
         *
         * This line responds to all incoming requests with a 'Hello World!' message:
         *
         *    app.Run(async (context) => { await context.Response.WriteAsync("Hello World!"); });
         *
         * If that line were added to the Configure method right after the environment
         * check (and if this using statement, "using Microsoft.AspNetCore.Http;" were
         * added to the file), this server application would respond to every request
         * with a response that printed "Hello World" to the screen.
         *
         * From the first article[1]: 
         * 
         *     The Configure method is used to specify how the app responds to HTTP
         *     requests. The request pipeline is configured by adding middleware
         *     components to an IApplicationBuilder instance. IApplicationBuilder is
         *     available to the Configure method, but it isn't registered in the service
         *     container. Hosting creates an IApplicationBuilder and passes it directly
         *     to Configure (reference source).
         *
         *     The ASP.NET Core templates configure the pipeline with support for a
         *     developer exception page, BrowserLink, error pages, static files, and
         *     ASP.NET MVC. 
         */ 
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // If running in a development environment, a special piece of middleware is 
            // added to the pipeline: UseDeveloperExceptionPage(). If an exception is 
            // thrown in your application, this middleware component will display a
            // detailed error page to you. This will show you sensitive information like
            // stacktraces to help debugging easier. However, as this information is
            // sensitive, it will not displayed outside of a development environment. 
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // The MVC middleware is registered and added to the request processing
            // pipeline. ASP.NET Core's MVC library provides this via an extension method.
            // This will let the MVC framework to process the incoming requests and 
            // generate the appropriate outgoing responses.
            app.UseMvc();
        }
    }
}
 