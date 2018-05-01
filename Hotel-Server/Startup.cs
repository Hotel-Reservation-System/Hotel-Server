using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostgresEFCore.Providers;

using Common.Models;


using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Server
{
    /// <summary>
    /// "ASP.NET Core apps use a Startup class, which is named Startup by convention. The Startup
    /// class:
    ///     * Can optionally include a ConfigureServices method to configure the app's services.
    ///     * Must include a Configure method to create the app's request processing pipeline.
    /// ConfigureServices and Configure are called by the runtime when the app starts..."[1]
    /// 
    /// Sources
    /// 1: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        
        /* CONFIGURESERVICES() METHOD
         *
         * This method gets called by the runtime. Use this method to add services to the container.
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
         * available in the Hosting topic." [1]
         * 
         */
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            
            // It appears that you can add a connection string and instantiate a Context object
            // in the Startup file. The documented way is do all this in Context.cs and 
            // MyDbContextFactory.cs. Investigate this further.
//            var connectionString = "Host=localhost;" +
//                                   "Username=postgres;" +
//                                   "Password=password;" +
//                                   "Database=HotelManagement";
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
         * what to do with incoming requests. Consider this line:
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
