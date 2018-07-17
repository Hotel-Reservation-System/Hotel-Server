/* ARCHITECTURE OF THE HOTEL-SERVER PROJECT
 *
 * This project encapsulates the Data Access Layer and the Controllers of the Hotel Reservation
 * System Project. It is an ASP.NET Core WebAPI project intended to be a standalone program that 
 * runs on a server. This project's intended client is the Hotel-Client project, but it is designed
 * to be language and program agnostic. In other words, Hotel-Server can talk to clients besides 
 * Hotel-Client, even if they are written in other languages.
 *
 * Hotel-Server is partially structured in the MVC pattern. Specifically, it gets Models (M) from
 * the Common project and implements Controllers (C). As it is a WebAPI project, it should not and
 * does not have Views (V) implemented. That is the business of the Hotel-Client project. See below
 * for a discussion about this architecture.
 *
 * This project has a lot of notes that are essential to learning how this project is structured
 * and how it operates. Topics include: How the MVC architecture works, Database Setup,
 * Database Providers, Data Models, the Schema, Migrations, Controllers, Endpoints, REST
 * architecture, HTTP Requests, Route Variables and more. Read the notes in this order:
 * 
 *     1. WHAT IS MVC?:                    MVC Architecture.txt (Hotel-Server folder)
 *     2. DATABASE SETUP AND CONCEPTS:     Database Setup.txt (Hotel-Server folder)
 *     3. ALL ABOUT MIGRATIONS:            Migrations.txt (Migrations folder)
 *     4. CONTROLLERS & ENDPOINTS:         HotelController.cs (Controllers folder)
 *
 * 
 **************************************************************************************************
 **************************************************************************************************
 *
 * 
 * INTRODUCTION TO ASP.NET CORE
 * 
 * "ASP.NET Core is a cross-platform, high-performance, open-source framework for building modern,
 * cloud-based, Internet-connected applications. With ASP.NET Core, you can:
 * 
 *      * Build web apps and services, IoT apps, and mobile backends.
 *      * Use your favorite development tools on Windows, macOS, and Linux.
 *      * Deploy to the cloud or on-premises.
 *      * Run on .NET Core or .NET Framework."[1]
 *
 *
 * SOLUTION STRUCTURE
 *
 * The Properties folder comes with a 'launchsettings.json' file. This file contains all settings
 * the project needs to launch the application. Things like the settings for the server (IIS,
 * Kestrel etc.), Development and Production Profile settings are declared here. At launch time,
 * the program will consult this file and load the program as defined here.
 *
 * The wwwroot folder is treated as the root folder of your website. This means that
 * http://yourDomainName.com/ points to this folder. Any files in your web application that are
 * client facing, such as HTML, CSS JavaScript and Image files, should be put into wwwroot or
 * one of its subfolders. Files in wwwroot are whitelisted by the server for serving up to 
 * anyone who requests it. Note that the core code for your web application should not be made
 * available to the web (via wwwroot). C# code, Razor pages and configuration files should NOT 
 * sit in wwwroot.
 *
 * The Dependencies folder contains all the packages being used by this web application. Right now,
 * this consists of Nuget packages for server-side dependencies and Bower packages for client-side
 * dependencies.
 * 
 * In 'Solution Explorer' if you right-click 'Hotel-Server' and unload it, you can then choose to
 * 'Edit Hotel-Server.csproj' file. A .csproj file is where packages and dependencies for a
 * project are declared:
 *
 *     1. TargetFramework: sets the version number of .NET Core and ASP.NET Core. This project sets
 *        the value to 'netcoreapp2.0'. You can also right-click the project, go to 'Properties'
 *        and edit these value in the 'Project Properties' window.
 * 
 *     2. Nuget Packages: In the .csproj file, you can declare the dependencies you'll need in 
 *        your project and their version numbers. Usually, you don't have to hand edit this file;
 *        you add packages with the 'Nuget Package Manager' or 'Package Manager Console' and
 *        it will automatically update the .csproj file. However, if you choose it, you can declare
 *        dependencies by hand and Nuget will automatically download and install them to the
 *        project when you save the .csproj file.
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
 **************************************************************************************************
 **************************************************************************************************
 * 
 * 
 * SOURCES
 * 
 * 1: https://docs.microsoft.com/en-us/aspnet/core/
 * 
 **************************************************************************************************
 **************************************************************************************************
 *
 * 
 * Please note that both Program.cs and Startup.cs were auto-generated when this WebAPI project was
 * created. Program.cs contains the Main() method, which boots up the server. Startup.cs defines a
 * boot object used in Program.cs.
 */


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hotel_Server.Database;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Hotel_Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            // Creates the scope within which operations are performed.
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    // 1. Get a database context instance from the dependency injection container.
                    var context = services.GetRequiredService<Context>();
                    // 2. Call the seed method, passing to it the context.
                    DatabasePopulator.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
                // 3. Dispose the context when the seed method is done and goes out of scope.
            }

            host.Run();
        }


        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
