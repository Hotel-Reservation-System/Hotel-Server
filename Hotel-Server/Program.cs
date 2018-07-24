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
 * Kestrel etc.), Environment Variable settings (Development, Production etc.), Project settings
 * as well as your Application's URL are declared here. At launch time, the program will consult
 * this file and load the program as defined here.
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
 **************************************************************************************************
 **************************************************************************************************
 *
 * THE PROCESS OF BOOTING UP ASP.NET CORE PROGRAMS
 *
 * When an ASP.NET Core application is run, what happens? An ASP.NET Core application needs a
 * Webhost, so a host is created, which configures and boots up a Web Server. 
 *
 * 
 * WHAT IS A WEB HOST?
 *
 * I can't find a clear definition for a Host. It's an ambiguous term whose meaning varies
 * with context.
 *
 * In the most often-used sense, a web host appears to refer to the bundle of hardware and
 * software required to serve up websites. A host can be a reference to a physical
 * (or virtual) web server that stores files for one or more websites along with other
 * related applications. A host serves websites to those with authorization to access them.
 * A Web Hosting Service leases this bundle to clients who want to host websites. 
 *
 * This is related to but different from what the word 'Host' means in .NET/ASP.NET Core
 * terminology. An ASP.NET Core Web Application must run inside, for lack of a better word,
 * a container. This container, called a HOST or Webhost, appears to refer to the program or
 * process that executes the web application. This kind of host is the subject of this section.
 *
 * The first thing an ASP.NET Core application does is create and instantiate a Host. In ASP.NET
 * Core programs, the host is responsible for application startup and lifetime management.
 * This code is from Program.cs; it configures a Host and turns it on. Some variation of it
 * must run at the start of every ASP.NET Core program:
 *
 * 
 *         public static IWebHost BuildWebHost(string[] args) =>
 *             WebHost.CreateDefaultBuilder(args)
 *                 .UseStartup<Startup>()
 *                 .Build();
 *
 *
 * As you can see above, the first line of Main() in this program calls BuildWebHost(), which
 * creates a Host (the IWebHost object) and then boots it up. ASP.NET Core hosts must implement
 * the IWebHost interface. An IWebHost object represents a configured web host. They are created
 * by the static WebHost class, which "provides convenience methods for creating instances of
 * IWebHost and IWebHostBuilder with pre-configured defaults."[2] The WebHost class uses an
 * instance of IWebHostBuilder to construct IWebHost objects. 
 *
 * A Host only starts an web app and manages it. There is one other important thing that
 * every web application needs: handling incoming HTTP requests and responding to them.
 * Managing HTTP requests is the duty of a WEB SERVER program. Responding to requests is
 * the job of ASP.NET Core's Middleware Services (Such services make up the Request
 * Processing Pipeline). When the host boots up, at a minimum, it will configure a web
 * server and a request processing pipeline.
 *
 * See the section below titled, 'What is a Web Server?' for more about servers. See the
 * Configure() method of the Startup class to learn how to create the Request Pipeline.
 *
 *
 * THE HOST: THE DEFAULT CONFIGURATION
 *
 * By default, when an IWebHost instance is created, it needs to be given configuration
 * parameters. Configuring a web app is complicated because a lot of parameters need to
 * configured. Fortunately, the configuration required is quite standard because it does
 * not change between applications or as the application changes over time.
 *
 * The WebHost class comes with a CreateDefaultBuilder() method, which if called, will
 * automatically configure and return an IWebHostBuilder object with standard settings
 * for the Host. The IWebHostBuilder object will in turn create a Host object. This is
 * what CreateDefaultBuilder() looks like in ASP.NET Core 2.0:
 *
 * 
 *     public static IWebHostBuilder CreateDefaultBuilder(string[] args)
 *     {
 *         return new WebHostBuilder()
 *             .UseKestrel()
 *             .UseContentRoot(Directory.GetCurrentDirectory())
 *             .ConfigureAppConfiguration((Action<WebHostBuilderContext, IConfigurationBuilder>) ((hostingContext, config) =>
 *             {
 *                 // SETUP CONFIGURATION    
 *             }))
 *             .ConfigureLogging((Action<WebHostBuilderContext, ILoggingBuilder>) ((hostingContext, logging) =>
 *             {
 *                 // CONFIGURE LOGGING 
 *             }))
 *             .UseIISIntegration()
 *             .UseDefaultServiceProvider((Action<WebHostBuilderContext, ServiceProviderOptions>) ((context, options) =>
 *             {
 *                 // SETUP THE DEPENDENCY INJECTION CONTAINER FOR USE
 *             }
 *     }
 *
 *
 * These are the things CreateDefaultBuilder() is doing[3]:
 *
 *     * CONFIGURES THE DEFAULT WEBSERVER (KESTREL) AND STARTS IT: Configures Kestrel as the web
 *       server and configures the server using the app's hosting configuration providers. For
 *       the Kestrel default options, see Kestrel web server implementation in ASP.NET Core.
 *
 *     * CONTENT DIRECTORY: Sets the content root to the path returned by
 *       Directory.GetCurrentDirectory. ["The content root is the base path to any content used
 *       by the app, such as views, Razor Pages, and static assets. By default, the content root
 *       is the same as application base path for the executable hosting the app." ContentRoot
 *       is similar to Web Root (wwwroot), which contains public static resources, such as HTML,
 *       CSS and JavaScript files.
 *       When the Host object is being constructed, by default, the program directory that
 *       contains appsettings.json is set as the Content Root (aka Content Directory) for
 *       that web application.]
 * 
 *     * OPTIONAL CONFIGURATIONS: Loads host configuration from:
 *         * Environment variables prefixed with ASPNETCORE_ (for example, ASPNETCORE_ENVIRONMENT).
 *         * Command-line arguments.
 * 
 *     * OPTIONAL CONFIGURATIONS: Loads app configuration from:appsettings.json.
 *         * appsettings.{Environment}.json.
 *         * User secrets when the app runs in the Development environment using the entry assembly.
 *         * Environment variables.
 *         * Environment variables prefixed with ASPNETCORE_ (for example, ASPNETCORE_ENVIRONMENT).
 *         * Command-line arguments.
 *
 *     * ENABLES LOGGING: Configures logging for console and debug output. Logging includes log
 *       filtering rules specified in a Logging configuration section of an appsettings.json or
 *       appsettings.{Environment}.json file.
 *
 *     * SETS UP KESTREL INTEGRATION WITH IIS: When running behind IIS, enables IIS integration. 
 *       Configures the base path and port the server listens on when using the ASP.NET Core 
 *       Module. The module creates a reverse proxy between IIS and Kestrel. Also configures the
 *       app to capture startup errors. For the IIS default options, see Host ASP.NET Core on
 *       Windows with IIS. Sets ServiceProviderOptions.ValidateScopes to true if the app's
 *       environment is Development. For more information, see Scope validation.
 *
 * Some additional notes on Web Hosts and Web Servers are given below[4]. Please note that this
 * source appears to be out of date, not having been updated since the release of .NET Core 1.0.
 *
 *     What is a Host?
 * 
 *     ASP.NET Core apps require a host in which to execute. A host must implement the IWebHost
 *     interface, which exposes collections of features and services, and a Start method. The
 *     host is typically created using an instance of a WebHostBuilder, which builds and returns
 *     a WebHost instance. The WebHost references the server that will handle requests.
 *
 *     What is the difference between a host and a server?
 *
 *     The host is responsible for application startup and lifetime management. The server is
 *     responsible for accepting HTTP requests. Part of the host’s responsibility includes
 *     ensuring the application’s services and the server are available and properly configured.
 *     You can think of the host as being a wrapper around the server. The host is configured
 *     to use a particular server; the server is unaware of its host.
 *
 * 
 * WHAT IS A WEB SERVER?
 *
 * The server will listen for 
 *
 * 
 **************************************************************************************************
 **************************************************************************************************
 * 
 * 
 * SOURCES
 * 
 * 1: https://docs.microsoft.com/en-us/aspnet/core/
 * 2: https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.webhost?view=aspnetcore-2.1
 * 3: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/web-host?view=aspnetcore-2.1
 * 4: https://aspnetcore.readthedocs.io/en/stable/fundamentals/hosting.html
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
        // Main() boots up a web server and the host for this project.  
        public static void Main(string[] args)
        {
            // On the line below, the Host is declared.
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

            // The Host is started.
            host.Run();
        }


        // This method is where the WebHost is created, supplied with configuration information
        // and then instantiated. According to the documentation, WebHost "provides convenience
        // methods for creating instances of IWebHost and IWebHostBuilder with pre-configured
        // defaults." It is a static class that also contains a variety of Start() methods for
        // booting up instances of IWebHost. Check the WebHost class and the IWebHost,
        // IWebHostBuilder interfaces for more details. 
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
