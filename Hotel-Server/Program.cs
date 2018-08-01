/* ARCHITECTURE OF THE HOTEL-SERVER PROJECT
 *
 * This project encapsulates the Data Access Layer and the Controllers of the Hotel Reservation
 * System Project. It is an ASP.NET Core WebAPI project intended to be a standalone program that 
 * runs on a server. This project's intended client is the Hotel-Client project, but it is designed
 * to be language and program agnostic. In other words, Hotel-Server can talk to clients besides 
 * Hotel-Client, even if they are written in other languages.
 *
 * TODO: INCORPORATE AND REWRITE THIS BLOCK TO EXPLAIN HOTEL-SERVER'S ROLL AS AN APP SERVER.
 *
 *       Application Server: is the server program portion of an application that is divided
 *       into a multi-tier architecture, as is the case for this project. The application 
 *       server for this project is called 'Hotel-Server'. An application server is an
 *       environment that provides all the runtime services your application needs. It hosts
 *       and exposes the business logic of your application to the client through APIs.
 *
 *       The FRONT-END of a multi-tier application is also called the Presentation layer. It
 *       presents a UI to the user, by which the user issues commands to the application. The
 *       front-end can be a client application that runs on the users computer, but its equally
 *       likely to be Web-browser based. The front-end for this project is called 'Hotel-Client'.
 *
 *       The middle tier of the application, called the APPLICATION SERVER, contains the
 *       business logic layer of the application. It sits between the front-end and the backend,
 *       acting as the intermediary between the database and the users of the application.
 *       The front-end and back-end talk to the application server via protocols and APIs.
 *       Application servers usually extend the capabilities of a web server by either 
 *       encompassing or being paired with a web server. Application servers usually run on
 *       the user's computer and but they can also run on servers. 
 *
 *       The last tier, the BACK-END, is the database and transaction layer. It handles
 *       communications with the database. Backends usually run on server machines. The
 *       backend for this project is called 'Hotel-Server'.
 * 
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
 * What happens when an ASP.NET Core application runs?
 *
 * When an ASP.NET Core application starts, the first thing it will do is to execute the
 * Main() method in Program.cs. In any ASP.NET Core project, the first few instructions
 * in Main() configure and start a Host for the program. The Host is container in which a
 * program executes. After initializing, the Host will configure and boot up these two
 * things at a bare minimum:
 *
 * 
 *     1. A Web Server: to handle incoming HTTP requests,                     (Program.cs)
 *     2. A Middleware Pipeline: to process and respond to these requests.    (Startup.cs)
 *
 *
 * The host in turn will have to configure and turn on the web server and middleware
 * services, for which it will need to have configuration information such as:
 *
 * 
 *     * Environment variables of the web app,
 *     * User secrets configuration values,
 *     * the IIS web server config defaults,
 *     * logging config defaults,
 *
 * 
 * plus config information for all the other parts of the application that your web app
 * requires. Therefore, there is a lot of configuration information that the Host needs.
 *
 * Default configuration data for the things listed above is specified in this static class
 * and method: Webhost.CreateDefaultBuilder(). I believe you can change these defaults by
 * making changes in Program.cs. In Startup.cs, you can specify the the Middleware services
 * you want to add to your application and also configure the Request Processing Pipeline.
 *
 * 
 * WHAT IS A WEB HOST?
 *
 * I can't find a clear definition for a Host. It's an ambiguous term whose meaning varies
 * with context.
 *
 * In the most often used sense, a web host appears to refer to the bundle of hardware and
 * software required to serve up websites. The word appears to be synonymous with Web Server.
 * In this sense, a host seems be a reference to a physical (or virtual) web server that
 * stores files for one or more websites along with other related applications. In the
 * typical case, when a user requests a website or a resource via their browser, the request
 * travels over the internet to the host server, which responds by delivering the requested
 * content, provided that the user is authorized to receive it. A Web Hosting Service leases
 * this bundle to clients who want to host websites. 
 *
 * This is related to but different from what the word 'Host' means in .NET/ASP.NET Core
 * terminology. An ASP.NET Core Web Application must run inside, for lack of a better word,
 * a container. This container, called a HOST or Webhost, appears to refer to the program or
 * process that executes the web application. A Host also connects the web app to a web server.
 * This section is concerned with this type of host.
 *
 * As the Host is the container for the Web app, creating and instantiating a Host is the
 * first thing an ASP.NET Core application does. This code is the BuildWebHost() method in
 * Program.cs of this application; it loads configuration information into a Host and turns
 * it on. Some variation of it must run at the start of every ASP.NET Core program:
 *
 * 
 *         public static IWebHost BuildWebHost(string[] args) =>
 *             WebHost.CreateDefaultBuilder(args)
 *                 .UseStartup<Startup>()
 *                 .Build();
 *
 *
 * As you can see above, the first line of Main() in this program calls BuildWebHost(),
 * which creates a Host (the IWebHost object) and then boots it up. ASP.NET Core hosts must
 * implement the IWebHost interface. An IWebHost object represents a configured web host.
 * They are created by the static WebHost class, which "provides convenience methods for
 * creating instances of IWebHost and IWebHostBuilder with pre-configured defaults."[2]
 *
 * The WebHost class uses an instance of IWebHostBuilder to construct IWebHost objects.
 * Webhost.CreateDefaultBuilder() will load default configuration information into an
 * IWebHostBuilder object. IWebHostBuilder will return a IWebHost object.
 *
 * In ASP.NET Core programs, the host is responsible for application startup and lifetime
 * management. Another one of its responsibilities is to correctly configure and turn on
 * an application's services and a web server. Why does a web application have to be
 * connected to a web server?
 *
 * Every web application is network-facing, i.e. it communicates to other programs and
 * processes over a network. This entails handling incoming HTTP requests and responding
 * to them. Managing HTTP requests is the duty of a WEB SERVER program. Responding to
 * requests is the job of ASP.NET Core's Middleware Services (Such services make up the
 * Request Processing Pipeline). When the host boots up, at a minimum, it will configure
 * a web server and a request processing pipeline.
 *
 * See the section below titled, 'What is a Server?' for more about servers. See the
 * Configure() method of the Startup class to learn how to create the Request Pipeline.
 *
 *
 * THE HOST: CONFIGURATION OPTIONS 
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
 *
 *     * CONFIGURES THE DEFAULT WEBSERVER (KESTREL) AND STARTS IT: Configures Kestrel as
 *       the web server and configures the server using the app's hosting configuration
 *       providers. For the Kestrel default options, see Kestrel web server implementation
 *       in ASP.NET Core.
 *
 *     * CONTENT DIRECTORY: Sets the content root to the path returned by
 *       Directory.GetCurrentDirectory. ["The content root is the base path to any content
 *       used by the app, such as views, Razor Pages, and static assets. By default, the
 *       content root is the same as application base path for the executable hosting the app."
 *
 *       ContentRoot is similar to Web Root (wwwroot), which contains public static resources,
 *       such as HTML, CSS and JavaScript files. When the Host object is being constructed,
 *       by default, the program directory that contains appsettings.json is made the Content
 *       Root (aka Content Directory) for that web application.]
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
 *     * ENABLES LOGGING: Configures logging for console and debug output. Logging includes
 *       log filtering rules specified in a Logging configuration section of an
 *       appsettings.json or appsettings.{Environment}.json file.
 *
 *     * SETS UP KESTREL INTEGRATION WITH IIS: When running behind IIS, enables IIS
 *       integration. Configures the base path and port the server listens on when using the
 *       ASP.NET Core Module. The module creates a reverse proxy between IIS and Kestrel.
 *       Also configures the app to capture startup errors. For the IIS default options, see
 *       Host ASP.NET Core on Windows with IIS. Sets ServiceProviderOptions.ValidateScopes to
 *       true if the app's environment is Development. For more information, see Scope
 *       validation.
 * 
 *
 * If you want a configuration different from CreateDefaultBuilder()'s default configuration,
 * you can make changes by calling other methods and passing in different arguments. See
 * the Microsoft Docs for more information. 
 * 
 * Below, see some additional notes on Web Hosts and Web Servers[4]. Please note that this
 * source appears to be out of date, not having been updated since the release of .NET Core
 * 1.0:
 *
 * 
 *     What is a Host?
 * 
 *     ASP.NET Core apps require a host in which to execute. A host must implement the
 *     IWebHost interface, which exposes collections of features and services, and a Start
 *     method. The host is typically created using an instance of a WebHostBuilder, which
 *     builds and returns a WebHost instance. The WebHost references the server that will
 *     handle requests.
 *
 *     What is the difference between a host and a server?
 *
 *     The host is responsible for application startup and lifetime management. The server
 *     is responsible for accepting HTTP requests. Part of the host’s responsibility includes
 *     ensuring the application’s services and the server are available and properly
 *     configured. You can think of the host as being a wrapper around the server. The host
 *     is configured to use a particular server; the server is unaware of its host.
 *
 * 
 * WHAT IS A SERVER?
 *
 * In computing, the term server is overloaded and can be used for lots of things. In the
 * context of ASP.NET Core, when I talk about 'servers', the term can refer to the physical
 * computer that hosts applications or the actual program that runs on these computers and
 * delivers content to users or to the combination of hardware and software working together. 
 *
 * In the hardware sense, what is a server? Here is an explanation from an answer to a
 * question on Quora[5]:
 * 
 *
 *     In a hardware context, a "server" is computer hardware that has been engineered for
 *     the purpose of hosting large, long-running applications with a high degree of
 *     reliability and durability. Typically server-class hardware will support more
 *     processors and more memory than desktop or workstation class hardware, will have
 *     redundant power supplies and redundant cooling systems, and may have the capability
 *     to support more hard drives, network interfaces, and other IO options than typically
 *     found on desktop or workstation class hardware. Hardware components (power supplies,
 *     fans, hard drives, memory, sometimes even processors) can be removed and replaced
 *     without shutting down the machine or requiring the operating system to restart.
 *     Servers typically have very limited video capabilities (although some have
 *     "headless GPUs" to support GPU-based computation) and usually have no audio capability
 *     at all, just a little thing that can go beep. Server-class hardware has been engineered
 *     to run continuously for years at a time in temperature-controlled environments.
 *     Architecturally, server-grade hardware is not that dissimilar to workstation-grade
 *     hardware; it's just engineered for a different level of performance.
 * 
 *
 * Both physical servers and programs that serve content are called servers, but this section
 * is mostly devoted to studying programs that run on server machines, not the machines
 * themselves.
 *
 * What is a server in the software sense? What does it do?
 *
 * A SERVER is an application that runs passively on a device, listening on a network port
 * for incoming requests. It stands ready to perform operations or answer inquiries in
 * response to queries from other programs. Programs that makes queries or requests of the
 * server are called CLIENTS. The Server and Client form a two-sided distributed,
 * network-based architecture called the 'Client-Server Model'[6]. Clients and Servers in
 * this model communicate across a network (via network protocols) even if they are running
 * on the same machine.
 *
 * Servers usually don't act on their own; they act in response to client requests. Because
 * servers are designed to respond to queries, a server program spends most of its time
 * waiting for requests. A server program is usually not turned off, instead often running
 * for months, years or even decades without interruption. This is why they run on special
 * server computers that are designed for reliablity and durability. You rarely see server
 * programs intended for long-term, uninterrupted uptime run on phones or desktop computers.  
 *
 *
 * TYPES OF SERVERS
 * 
 * There are many kinds of servers. A distinction should be made here between dedicated
 * and virtual servers. When the bundle of hardware and software in one server machine
 * performs a single task, it is called a DEDICATED SERVER. For instance, email servers are
 * usually dedicated to sending, receiving and storing emails.
 * 
 * Servers can also be VIRTUAL, i.e you can use virtualization software to run several virtual
 * operating systems at once. You can install server programs on each of these virtual OSs to
 * maximize the use of the physical server's capabilities. In this way, you can run dozens of
 * server programs across many virtual OSs on a single server machine. Each server program
 * can now act like a dedicated server. However if too many server programs reside on one
 * machine or if a single ravenous server starts hogging resources, it can negatively impact
 * performance for all virtual servers on that machine.
 *
 * Servers can also be categorized according to purpose. Here is a partial list of server types:
 *
 * 
 *     * Email Servers: Mail servers move and store email across the internet and corporate
 *       intranets.
 * 
 *     * Proxy Servers: "A proxy server is a server that sits between a client application,
 *       such as a Web browser, and a real server. Proxy servers have two main purposes: to
 *       improve performance and to filter requests."[7]
 * 
 *     * Web Servers: "Web servers are computers that deliver (or serve up) Web pages. Every
 *       Web server has an IP address and possibly a domain name. There are many Web server
 *       software applications, including public domain software and commercial packages."[7]
 *       Web servers usually involve a browser (the client) and the server communicating over
 *       the HTTP Protocol.
 * 
 *     * Real-Time Communication Server: are sometimes called Chat servers or IRC servers.
 *       They are used to send and receive instant messages in real time.
 * 
 *     * FTP Server: is one of the oldest types of servers. An FTP server uses the File
 *       Transfer Protocol (FTP) to send and receive files securely.
 * 
 *     * Collaboration Server: are used to facilitate collaboration for groupware software.
 *
 *     * Telnet Server: "A Telnet server enables users to log on to a host computer and
 *       perform tasks as if they're working on the remote computer itself."[8]
 *
 *     * List Server: "List servers offer a way to better manage mailing lists, whether
 *       they be interactive discussions open to the public or one-way lists that deliver
 *       announcements, newsletters or advertising."
 * 
 *     * Application Server: is the server program portion of an application that is divided
 *       into a multi-tier architecture, as is the case for this project. Application servers
 *       host the business logic of your application and expose access to it through APIs to
 *       clients. An application server is an environment that provides all the runtime
 *       services your application needs. The application server for this project is called
 *       'Hotel-Server'.  
 *
 *
 * WHAT IS A WEB SERVER?
 *
 * A Web Server can refer to a physical machine (or a virtual server running on it) or server
 * software running on a server machine. An Mozilla Developer Network article describes a
 * hardware web server as follows[9]:
 *
 * 
 *     On the hardware side, a web server is a computer that stores web server software 
 *     and a website's component files (e.g. HTML documents, images, CSS stylesheets, and
 *     JavaScript files). It is connected to the Internet and supports physical data
 *     interchange with other devices connected to the web.
 *
 * 
 * The article describes Web server software like this:
 *
 * 
 *     On the software side, a web server includes several parts that control how web users
 *     access hosted files, at minimum an HTTP server. An HTTP server is a piece of software
 *     that understands URLs (web addresses) and HTTP (the protocol your browser uses to view
 *     webpages). It can be accessed through the domain names (like mozilla.org) of websites
 *     it stores, and delivers their content to the end-user's device.
 *
 * 
 * HOW DOES A WEB SERVER COMMUNICATE OVER THE INTERNET?
 *
 * 
 *         +----------------------+                           +---------------------+
 *         |      WEB SERVER      |                           |                     |
 *         | +-------+  +-------+ |        HTTP REQUEST       |                     |
 *         | |       |  |       | | <-----------------------+ |                     |
 *         | |       |  |       | |                           |                     |
 *         | |       |  | HTTP  | |                           |                     |
 *         | | FILES |  | SERVER| |                           |     WEB BROWSER     |
 *         | |       |  |       | |       HTTP RESPONSE       |                     |
 *         | |       |  |       | | +-----------------------> |                     |
 *         | +-------+  +-------+ |                           |                     |
 *         |                      |                           |                     |
 *         +----------------------+                           +---------------------+
 *
 * Take a look at this diagram. The MDN article explains how the browser gets a file from a
 * web server:
 * 
 *
 *     At the most basic level, whenever a browser needs a file which is hosted on a web server,
 *     the browser requests the file via HTTP. When the request reaches the correct web server
 *     (hardware), the HTTP server (software) accepts request, finds the requested document
 *     (if it doesn't then a 404 response is returned), and sends it back to the browser, also
 *     through HTTP.
 *
 * 
 * As the article explains, there are some rules to communication over the HTTP protocol:
 *
 * 
 *     * Only clients can make HTTP requests, and then only to servers. Servers can only
 *       respond to a client's HTTP request.
 *
 *     * When requesting a file via HTTP, clients must provide the file's URL.
 *
 *     * The web server must answer every HTTP request, at least with an error message.
 * 
 *
 * The article explains the responsibilities of a Web Server. Note that the term 'HTTP Server'
 * is used instead 'Web Server'. There is a distinction between the terms, but here you can
 * treat them as the same:
 *
 * 
 *     On a web server, the HTTP server is responsible for processing and answering incoming
 *     requests.
 *         1. On receiving a request, an HTTP server first checks whether the requested URL
 *            matches an existing file.
 * 
 *         2. If so, the web server sends the file content back to the browser. If not, an
 *            application server builds the necessary file.
 * 
 *         3. If neither process is possible, the web server returns an error message to the
 *            browser, most commonly "404 Not Found". (That error is so common that many web
 *            designers spend quite some time designing 404 error pages.)
 *
 *
 * CHOICE OF WEB SERVERS
 *
 * According to w3techs[10], these are the most popular web servers as of 26 July, 2018:
 *
 *
 *     1. Apache: 45.9%
 *     2. Nginx: 39.0%
 *     3. IIS: 9.5%
 *     4. Litespeed: 3.4%
 *
 * Microsoft-oriented shops tend to use IIS (Internet Information Server), but it's no longer
 * the default web server for .NET Core/ASP.NET Core so its market share is declining. Linux
 * shops tend to favour Apache, Nginx and Litespeed. Apache has been the big dog since 1995, 
 * but it consumes much more memory while being much less performant, so unsurprisingly,
 * it will probably soon lose its crown in a few years to the new rising star, Nginx. 
 * 
 * The default web server for ASP.NET Core programs is Kestrel.
 *
 *
 * KESTREL
 *
 * Stackify has an article that introduces Kestrel[11]:
 *
 *     Kestrel is open-source (source code available on GitHub), event-driven, asynchronous
 *     I/O based server used to host ASP.NET applications on any platform. It’s a listening
 *     server and a command-line interface. You install the listening server on a Windows or
 *     Linux server and the command-line interface on your computer.
 *
 *     It was launched by Microsoft along with ASP.NET Core. All ASP.NET Core apps utilize a
 *     new MVC framework and the Kestrel web server. These new apps can run on full .NET
 *     Framework or .NET Core...
 *
 *     Kestrel is considered a preferred web server for newer ASP.NET applications... It is
 *     based on the libuv library, the same one used by node.js. Libuv supports an
 *     event-driven style of programming. Some of its core utilities include:
 *
 *         * Non-blocking network support
 *         * Asynchronous file system access
 *         * Timers
 *         * Child processes
 *
 *     It allows ASP.NET Core applications to be run easily on other cross-platform webservers
 *     such as Nginx and Apache, without the need to address varying startup configurations. By
 *     using Kestrel as an in-process server, applications will have a consistent process
 *     (Startup (Main(), Startup.ConfigireServices(), Startup.Configure()) ) even with
 *     cross-platform support.
 *
 * However, Kestrel is not a fully-featured web server; it does as little as possible. Kestrel
 * developers seem to optimized for speed by leaving out most of IIS's features. So, it
 * sounds as though Kestrel is intended to work as an in-process server, with another fully
 * featured web server like Nginx or IIS in front of it for public web applications, as Kestrel
 * cannot handle windows authentication, security and other such services. When you use another
 * web server in front of Kestrel, that web server is said to be a Reverse Proxy Server.
 *
 * You can configure some of these options in Configure() method of Startup.cs. See Microsoft
 * Docs for more details.
 * 
 * 
 **************************************************************************************************
 **************************************************************************************************
 * 
 * 
 * SOURCES
 * 
 * 01: https://docs.microsoft.com/en-us/aspnet/core/
 * 02: https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.webhost?view=aspnetcore-2.1
 * 03: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/web-host?view=aspnetcore-2.1
 * 04: https://aspnetcore.readthedocs.io/en/stable/fundamentals/hosting.html
 * 05: https://www.quora.com/What-is-a-software-server-and-what-are-the-main-differences-between-a-software-server-and-a-hardware-server
 * 06: https://en.wikipedia.org/wiki/Client%E2%80%93server_model
 * 07: https://www.webopedia.com/TERM/S/server.html
 * 08: https://www.webopedia.com/quick_ref/servers.asp
 * 09: https://developer.mozilla.org/en-US/docs/Learn/Common_questions/What_is_a_web_server
 * 10: https://w3techs.com/technologies/overview/web_server/all
 * 11: https://stackify.com/what-is-kestrel-web-server/
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
            // On the line below, the Host is declared and turned on. 
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
