/* THE DATABASE CONTEXT FACTORY
 *
 * To talk to the database, Entity Framework Core needs to start a database session. The session 
 * comes in the form of a Context object. At runtime, EFC will create a Context object which is 
 * fed the Schema (from DbContext/Context.cs) and other configuration data (from this file).
 *
 * There are three possible ways of doing this; see Section 3.3 of Database Setup.txt. However, I
 * will do the work of specifying connection strings, database providers etc., here in this file.
 * This class, MyDbContextFactory, is a factory that creates such a database session object. This
 * object will be passed to Controllers in the AllController.cs file. Any part of the program that
 * wants to talk to the database will need this Context object.
 *
 * The first thing this class does is create a DbContextOptions object, which contains certain
 * information that Context class needs. Copy and/or edit this file to direct Entity Framework to
 * recognize other databases.
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
            // INSTANTIATING AND CONFIGURING A DbContextOptions OBJECT
            var builder = new DbContextOptionsBuilder<Context>();


            // We use the Npgsql() method to let EF Core know that we're using a PostgreSQL 
            // database. For the sake of simplicity, we pass a hard-coded connection string to 
            // the Npgsql() method to configure the database. You also could use dependency
            // injection.
            builder.UseNpgsql("Host=localhost;" +
                              "Username=postgres;" +
                              "Password=password;" +
                              "Database=HotelManagement");
            return new Context(builder.Options);
        }
    }
}
