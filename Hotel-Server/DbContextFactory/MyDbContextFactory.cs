/* THE DATABASE CONTEXT FACTORY
 *
 * To talk to the database, Entity Framework Core needs to start a database session. The session 
 * comes in the form of a Context object. This file, MyDbContextFactory.cs, is a factory that 
 * creates such a database session object. At runtime, EFC will create a Context object which is 
 * fed the Data Model (from DbContext/Context.cs) and a database connection string (from this file). 
 * This object will be passed to Controllers in the AllController.cs file.
 * 
 * Copy and/or edit this file to direct Entity Framework to recognize other databases.
 * 
 * SOURCES
 * 1: https://docs.microsoft.com/en-us/ef/core/api/microsoft.entityframeworkcore.dbcontext
 */


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PostgresEFCore.Providers;


namespace PostgresEFCore.Factories
{
    /// <summary>
    /// "A DbContext instance represents a session with the database and can be used to query and 
    /// save instances of your entities... Typically you create a class that derives from DbContext 
    /// and contains DbSet<TEntity> properties for each entity in the model. If the DbSet<TEntity> 
    /// properties have a public setter, they are automatically initialized when the instance of 
    /// the derived context is created." [1]
    /// </summary>
    public class MyDbContextFactory : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext(string[] args)
        {
            /* CONFIGURING THE DATABASE CONNECTION STRING
             * 
             * "Override the OnConfiguring(DbContextOptionsBuilder) method to configure the 
             * database (and other options) to be used for the context. Alternatively, if you would 
             * rather perform configuration externally instead of inline in your context, you can 
             * use DbContextOptionsBuilder<TContext> (or DbContextOptionsBuilder) to externally 
             * create an instance of DbContextOptions<TContext> (or DbContextOptions) and pass it 
             * to a base constructor of DbContext." [1]
             */
            var builder = new DbContextOptionsBuilder<Context>();


            // For the sake of simplicity, we pass a hard-coded connection string to the Npgsql() 
            // method to configure the database. You also could use dependency injection.
            builder.UseNpgsql("Host=localhost;" +
                              "Username=postgres;" +
                              "Password=password;" +
                              "Database=HotelManagement");
            return new Context(builder.Options);
        }
    }
}
