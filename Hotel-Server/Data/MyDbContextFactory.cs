/* THE DATABASE CONTEXT FACTORY
 *
 * To talk to the database, Entity Framework Core needs to start a database session.
 * The session comes in the form of a Context object. At runtime, EFC will create a
 * Context object which is fed the Schema (from Data/Context.cs) and other configuration
 * data (from this file).
 *
 * There are three possible ways of doing this; see 'Step 4: Configuring the Loading of
 * Config Data Into the Context Object' in Context.cs for a full explanation. 
 *
 * This class, MyDbContextFactory, is a factory that creates a DbContextOptions object.
 * Configuration and connection string related values, such as host name, database
 * providers, database login credentials etc. are defined here in a
 * DbContextOptionsBuilder<TContext> class. DbContextOptionsBuilder loads all this data
 * into a DbContextOptions object, which in turn gets passed to a newly created Context
 * object.
 * 
 * Once the Context session object has been created, it will made available to other
 * services that need it. An example: Controller classes in the AllController.cs file.
 * Any part of the program that wants to talk to the database will need this Context
 * object.
 *
 * Copy and/or edit this file to direct Entity Framework to recognize other databases.
 *
 * 
 * SOURCES
 * 1: https://docs.microsoft.com/en-us/ef/core/api/microsoft.entityframeworkcore.dbcontext
 * 2: https://docs.microsoft.com/en-us/ef/core/miscellaneous/configuring-dbcontext
 */


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Hotel_Server.Database;


namespace Hotel_Server.D
{
    public class MyDbContextFactory : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext(string[] args)
        {
            // INSTANTIATING AND CONFIGURING A DbContextOptionsBuilder OBJECT
            var builder = new DbContextOptionsBuilder<Context>();


            // LOADING CONFIGURATION DATA INTO THE DbContextOptionsBuilder OBJECT
            // We use the Npgsql() method to let EF Core know that we're using a
            // PostgreSQL database. For the sake of simplicity, we pass a hard-coded
            // connection string to the Npgsql() method to configure the database. 
            builder.UseNpgsql("Host=localhost;" +
                              "Username=postgres;" +
                              "Password=67890;" +
                              "Database=HotelsDb");
            
            // CREATE A NEW CONTEXT OBJECT, INTO WHICH THE DbContextOptionsBuilder
            // OBJECT WILL LOAD A DbContextOptions OBJECT THAT CONTAINS THE CONFIGURATION
            // DATA
            return new Context(builder.Options);
        }
    }
}
