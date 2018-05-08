/* CONTEXT CLASS (aka DBCONTEXT CLASS)
 *
 * When the Hotel-Server program is run, EF Core will try to talk to the database. Recall that EF
 * Core will attempt to do two-way translations between the program and the database:
 *
 * 
 *                    PROGRAM <----> ENTITY FRAMEWORK CORE <----> DATABASE
 *
 * 
 * Before it can do this, it needs a blueprint of the database to which it is trying to talk. Enter
 * Context class, which is a repository of one of the things EF Core needs to act as ORM for this
 * project, namely, the Schema. As a reminder, the Schema contains a specific definition of the
 * topology of a database, the entities in it and their relationships at a point in time, usually
 * reflecting the current status.
 *
 * Context class is an integral part of Entity Framework Core. It is a custom class written by the
 * programmer. It must inherit from an EF Core class called Microsoft.EntityFrameworkCore.DbContext,
 * which is it is often known as DbContext class.
 * 
 * From the docs[1]: "A DbContext instance represents a session with the database and can be used
 * to query and save instances of your entities." Put succinctly, the DbContext class helps EF Core
 * talk to the database. For this reason, one of the first things EF Core will do is create a
 * Context object.
 *
 * In Context class, the programmer must specify specific parameters for Tables and other entities
 * in the database. As such, it serves as a bridge between the data model (classes that model
 * database entities in the Models/Tables folder of the Common project) and database entities
 * (records stored in the database).
 *
 *
 * 
 * STEP 1: WRITING AND CONFIGURING THIS CLASS
 *
 * When you write a Context class, first set it to inherit from DbContext. Call the class whatever
 * you want, plus the "Context" suffix; e.g. ACMEContext, SchoolContext, HotelContext etc. or just
 * Context as I have for this project. It's a good idea to name it after the database, as each
 * database requires its own Context class.
 *
 * 
 * STEP 2: DEFINE DATA STRUCTURES FOR TABLES IN THE DATABASE
 * 
 * The first thing you have to do is define tables in the database as properties to Context class.
 * From Microsoft's DbContext docs[1]:
 *
 * 
 *     Typically you create a class that derives from DbContext and contains DbSet<TEntity>
 *     properties for each entity in the model. If the DbSet<TEntity> properties have a public
 *     setter, they are automatically initialized when the instance of the derived context is
 *     created.
 * 
 *
 * In short, you have to add DbSet<T> data structures as properties to the Context class, one to
 * represent each table from the database. Name them after the table they are representing. When
 * EF Core translates database records into C# objects, it will store them in these data structures.
 * Here's an example from this file:
 *
 *         public virtual DbSet<Hotel> Hotels { get; set; }
 *
 *
 * STEP 3: CONFIGURING THE INSTANTIATION OF THE CONTEXT OBJECT
 *
 * This step is an somewhat of deviation, as it's about configuring the instantiation of the
 * Context object, not about the schema.
 * 
 * At runtime, EF Core will create a Context instance and pass it to the Controller classes. So,
 * the next thing to do is configure the creation of a Context instance.  There are three options
 * for doing this, of which two are listed below[1]:
 *
 * 
 *     Override the OnConfiguring(DbContextOptionsBuilder) method to configure the database (and
 *     other options) to be used for the context. Alternatively, if you would rather perform
 *     configuration externally instead of inline in your context, you can use
 *     DbContextOptionsBuilder<TContext> (or DbContextOptionsBuilder) to externally create an
 *     instance of DbContextOptions<TContext> (or DbContextOptions) and pass it to a base
 *     constructor of DbContext.
 *
 *
 * If you want to do the configuration in one class, Context, go with overridding the
 * OnConfiguring(DbContextOptionsBuilder) method. If you want to do configuration externally,
 * go the Context Factory route.
 * 
 * In this project, I've chosen to do the configuration externally. Look in the DbContextFactory
 * folder and open the MyDbContextFactory.cs. EF Core will execute this class to create the Context
 * object for this project. The connection string and other details are defined in this file and
 * then passed to Context's constructor, which creates the object.
 *
 * The third option is to do configuration via dependency injection in Startup.cs; see Section 3.3
 * in Database Setup.txt.
 *
 *
 * STEP 4: DECLARING THE SCHEMA: DEFINING ENTITY RELATIONSHIPS
 *
 * Let's return to schema; in this section, I'll lightly delve into how to define the schema for
 * this project's database. Open up the the Model classes (in project Common) and use information
 * from them as a guide. The docs tell us how EF Core detects the schema:
 *
 * 
 *     The model is discovered by running a set of conventions over the entity classes found in
 *     the DbSet<TEntity> properties on the derived context. To further configure the model that is
 *     discovered by convention, you can override the OnModelCreating(ModelBuilder) method.
 *
 * 
 * Declare the schema by overriding the OnModelCreating() method. In it, you have to declare a new
 * table with the modelBuilder.Entity<ModelClass> LINQ expression. You have to provide the C# model
 * class that will be a template for this table.
 *
 * Once the table has been declared, you have to declare their properties as columns in the table.
 * Go through all the properties of the model class, and declare them as columns in the table. 
 * While you're doing this, you can provide each property in this LINQ command with a list of
 * limitations and annotations, like column limitations, such as character counts and value
 * creation times:
 *
 * 
 *     modelBuilder.Entity<Hotel>(entity =>
 *     {
 *         entity.HasKey(e => e.Id);
 *         entity.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
 *         entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
 *         entity.Property(e => e.Address).IsRequired().HasMaxLength(200);
 *         entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(100);
 *     });
 *
 *
 * After declaring tables and their columns, you can start defining inter-entity relationships,
 * as shown here for the HotelRoom entity: 
 *
 * 
 *     entity.HasOne(e => e.Hotel)              // 1. HotelRoom is declared to be owned by Hotel objects.
 *         .WithMany(e => e.HotelRooms)         // 2. A Hotel object will have many HotelRooms.
 *         .HasForeignKey(e => e.HotelId)       // 3. The Hotels table has the HotelId foreign key.
 *         .OnDelete(DeleteBehavior.Cascade);   // 4. Deletes any dependent entities, so any row that
 *                                              //    has a foreign key to the deleted entity. Delete
 *                                              //    a hotel and all its hotels rooms will be deleted
 *                                              //    as well.
 *
 * 
 * See the Context class below and read the comments to see all the details of data model
 * declaration. 
 *
 * SOURCES
 *
 * 1: https://docs.microsoft.com/en-us/ef/core/api/microsoft.entityframeworkcore.dbcontext
 */



using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace PostgresEFCore.Providers
{
    // DbContext is an EF Core class. Your Context class must inherit from it.
    public class Context : DbContext
    {
        // This is list of DbSet<Model> Data Structures; create one per table. Model classes are
        // the homologue of tables in the database, so we need these data structures to store
        // records, once they get translated into C# objects.
        public virtual DbSet<Hotel> Hotels { get; set; }
        public virtual DbSet<HotelRoom> HotelRooms { get; set; }
        public virtual DbSet<RoomReservation> RoomReservations { get; set; }
        public virtual DbSet<RoomType> RoomTypes { get; set; }
        public virtual DbSet<BedType> BedTypes { get; set; }
        
        
        // If you choose to, you can all Context object instantiation set up in the constructor,
        // by specifying the sconnection string, DbContextOptions and other DB configuration
        // information. See Section 3.3 of Database Setup.txt.
        // 
        // This is a Context class constructor that "initializes a new instance of the 
        // DbContext class using the specified options." In MyDbContextFactory.cs, an object of 
        // type DbContextOptionsBuilder<Context> is created and configured with a connection string.
        // A Context object is created and this DbContextOptionsBuilder<Context> object is passed
        // as an argument to its constructor. This object and its details get passed to the base
        // constructor, which creates the final Context object.
        public Context(DbContextOptions options) : base(options)
        {
            
        }


        // DECLARING THE SCHEMA
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // This is where the 'O' in ORM (Objects) are defined.
            // Declare the Hotel Class. Also see the HotelRoom declaration.
            modelBuilder.Entity<Hotel>(entity =>
            {
                // Declaring columns in the Hotel Table. Recall that columns map on to properties
                // in a model class. Primary Keys are declared with HasKey(). Other keys are
                // declared with the Property keyword. 
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Address).IsRequired().HasMaxLength(200);
                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(100);
            });

            
            // Declare the HotelRoom Class. 
            modelBuilder.Entity<HotelRoom>(entity =>
            {
                // Declare columns in the HotelRoom Table. Composite key is declared on the first
                // line.
                entity.HasKey(x => new { x.RoomNumber, x.HotelId });
                entity.Property(x => x.RoomNumber).IsRequired();
                entity.Property(x => x.HotelId).IsRequired();
                entity.Property(x => x.NightlyRate).IsRequired();
                entity.Property(x => x.NumberOfBeds).IsRequired();
                entity.Property(x => x.RoomTypeId).IsRequired();
                entity.Property(x => x.BedTypeId).IsRequired();

                // This is where the 'R' in ORM (Object Relations) are defined. Here, the 
                // relationship between Hotel and HotelRoom is detailed. 
                entity.HasOne(e => e.Hotel)             // 1. HotelRoom is declared to be owned by Hotel objects.
                    .WithMany(e => e.HotelRooms)        // 2. A Hotel object will have many HotelRooms.
                    .HasForeignKey(e => e.HotelId)      // 3. The Hotels table has the HotelId foreign key.
                    .OnDelete(DeleteBehavior.Cascade);  // 4. Deletes any dependent entities, so any row that
                                                        //    has a foreign key to the deleted entity. Delete
                                                        //    a hotel and all its hotels rooms will be deleted
                                                        //    as well.
            });

            
            modelBuilder.Entity<RoomReservation>(entity => {
                entity.HasKey(e => e.ReservationId);
                entity.Property(e => e.ReservationId).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.HotelId).IsRequired();
                entity.Property(e => e.RoomNumber).IsRequired();
                entity.Property(e => e.StartDate).IsRequired();
                entity.Property(e => e.EndDate).IsRequired();

                entity.HasOne(e => e.HotelRoom)
                    .WithMany(e => e.RoomReservations)
                    .HasForeignKey(e => new { e.RoomNumber, e.HotelId })
                    .OnDelete(DeleteBehavior.Cascade);
            });

            
            modelBuilder.Entity<BedType>(entity => {
                entity.HasKey(e => new { e.Id, e.Name });
                entity.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired();
            });

            
            modelBuilder.Entity<RoomType>(entity => {
                entity.HasKey(e => new { e.Id, e.Name });
                entity.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired();
            });
        }


        public void FixState()
        {
            foreach (var entry in ChangeTracker.Entries<IObjectWithState>())
            {
                IObjectWithState stateInfo = entry.Entity;
                entry.State = ConvertState(stateInfo.State);
            }
        }

        public static EntityState ConvertState(ObjectState state)
        {
            switch (state)
            {
                case ObjectState.Added:
                    return EntityState.Added;
                case ObjectState.Modified:
                    return EntityState.Modified;
                case ObjectState.Deleted:
                    return EntityState.Deleted;
                default:
                    return EntityState.Unchanged;
            }
        }
    }
}
