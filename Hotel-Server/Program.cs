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
 * Data Providers, Data Models, the Schema, Migrations, Controllers, Endpoints, REST architecture, 
 * HTTP Requests, Route Variables and more. Read the notes in this order:
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
 * The wwwroot folder is the root folder of your website. Any files that are part of your web
 * application, such as HTML and JavaScript files, should be put into subfolders of wwwroot.
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
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Hotel_Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }


        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
