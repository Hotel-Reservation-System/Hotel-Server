/* PROGRAM.CS
 * 
 * Please note that both Program.cs and Startup.cs were auto-generated when this WebAPI
 * project was created. Program.cs contains the Main() method, which boots up the server.
 * Startup.cs defines a boot object used in Program.cs.
 *
 * Program.cs is the entry point of an ASP.NET Core applications. For more information on
 * Program.cs and what happens when the application starts, see '2.2 Application Startup
 * - Program.cs' in the Docs folder.
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

            // Creates the scope within which this program's services and operations are
            // executed.
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    // POPULATE THE DATABASE
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
