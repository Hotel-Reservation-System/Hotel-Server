/* CONTEXT CLASS (aka DBCONTEXT CLASS)
 *
 * When the Hotel-Server program is run, EF Core will try to talk to the database. Recall
 * that EF Core will attempt to do two-way translations between the program and the
 * database:
 *
 * 
 *                PROGRAM <----> ENTITY FRAMEWORK CORE <----> DATABASE
 *
 * 
 * Before it can do this, it needs a map of the database to which it is trying to talk.
 * Enter Context class, which is a repository of one of the things EF Core needs to act
 * as ORM for this project, namely, the Schema. As a reminder, the Schema contains a
 * specific definition of the topology of a database, the entities in it and their
 * relationships at a point in time, usually capturing the current state.
 *
 * Context class is an integral part of Entity Framework Core. It must inherit from an
 * EF Core class called Microsoft.EntityFrameworkCore.DbContext, which is often known as
 * DbContext class. From the docs[1]: "A DbContext instance represents a session with the
 * database and can be used to query and save instances of your entities." Put
 * succinctly, the DbContext class helps EF Core talk to the database.
 *
 * In this custom class, you, the programmer, must define the schema to your project's
 * database. The programmer must provide very specific parameters for Tables and other
 * entities in the database. As such, it serves as a bridge between the data model
 * (classes that model database entities) and database entities (records stored in the
 * database).
 *
 * When the project starts, one of the first things EF Core will do is instantiate an
 * object from the Context class that you have provided. This context object will be
 * provided to Controller classes, where it will serve as a map to the database, its
 * entities and their relationships. This will help Controllers to carry out CRUD
 * operations on the database.
 *
 * 
 ***************************************************************************************************
 ***************************************************************************************************
 *
 *
 * WRITING AND CONFIGURING A CONTEXT CLASS
 *
 * Before starting work on the Context class, you should have already finished writing
 * model classes.
 *
 * 
 * STEP 1: INHERIT FROM DBCONTEXT CLASS
 *
 * When you write a Context class, first set it to inherit from DbContext. Call the class
 * whatever you want, plus the "Context" suffix; e.g. ACMEContext, SchoolContext,
 * HotelContext etc. or just Context as I have for this project. It's a good idea to name
 * it after the database, as each database requires its own Context class.
 *
 ***************************************************************************************************
 *
 * STEP 2: DEFINE DATA STRUCTURES THAT CORRESPOND TO TABLES IN THE DATABASE
 * 
 * The next thing you have to do is define tables from the database as properties in
 * this class. When EF Core translates database records into C# objects, it will store
 * them in these data structures. From Microsoft's DbContext docs[1]:
 *
 * 
 *     Typically you create a class that derives from DbContext and contains
 *     DbSet<TEntity> properties for each entity in the model. If the DbSet<TEntity>
 *     properties have a public setter, they are automatically initialized when the
 *     instance of the derived context is created.
 * 
 *
 * Consult the data model classes. Those will be the tables in your database. In step 3,
 * you have to add DbSet<T> data structures as properties to the Context class, one to
 * represent each table from the database. You will have one property for every model
 * class. Give each data structure a plural name, after the table/data model they are
 * representing. Here's an example data structure from this file:
 *
 * 
 *         public virtual DbSet<Hotel> Hotels { get; set; }
 *
 ***************************************************************************************************
 *
 * STEP 3: THE SCHEMA: DECLARING ENTITIES AND DEFINING INTER-ENTITY RELATIONSHIPS
 *
 * In this section, I'll lightly delve into how to define the schema for this project's
 * database. Open up the the Model classes (in project Common) and use information from
 * them as a guide. The docs tell us how EF Core detects the schema:
 *
 * 
 *     The model is discovered by running a set of conventions over the entity classes
 *     found in the DbSet<TEntity> properties on the derived context. To further configure
 *     the model that is discovered by convention, you can override the
 *     OnModelCreating(ModelBuilder) method.
 *
 *
 * Recall that data model classes are C# representations of tables that exist in your
 * database. While the data model is good as a starting point, they are not detailed
 * enough for a database's needs. That's where the schema comes in. In the schema, you
 * declare a model class as the base for a new schema entity and then extend it further.
 *
 * Let's walk through the process of extending a model class into a fully fleshed out
 * schema entity. In order to declare new schema entities, you have to override the 
 * OnModelCreating() method. Within this block, you can declare new tables and their
 * properties with the 'modelBuilder.Entity<ModelClass>' LINQ expression.
 *
 * For reference, here is the 'Hotel' Entity declaration block from this file:
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
 * Let's go through all the pieces of this block. The first order of business is to
 * declare the new Entity. You do this in the declaration heading's generic brackets
 * ('<' and '>'). Between the brackets, enter the name of the C# model class that will
 * serve as a base for the fully defined entity. 
 *
 * Once the table name has been declared, you have to declare its properties as columns
 * in the table. To do this, first consult the model class and study its properties;
 * switch back to the entity declaration block here in the Context and declare all the 
 * properties in it. 
 * 
 * While you're doing declaring properties, you should provide each property in this
 * entity block with appropriate limitations like character limits for a column, whether
 * a value should be generated by the database and whether a field is required or not.
 * You should consult the project's database to make sure the limits you impose in the
 * schema match the ones in the database. Defining these constraints promotes data
 * integrity by preventing poor quality information from entering the database.
 *
 * After declaring tables and their columns, you can start defining inter-entity
 * relationships, as shown here for the HotelRoom entity: 
 *
 * 
 *     entity.HasOne(e => e.Hotel)              // 1. HotelRoom can be owned by only ONE Hotel object.
 *         .WithMany(e => e.HotelRooms)         // 2. A Hotel object can have many HotelRooms. (One-to-Many Relationship)
 *         .HasForeignKey(e => e.HotelId)       // 3. The HotelRoom table has 'HotelId' as foreign key.
 *         .OnDelete(DeleteBehavior.Cascade);   // 4. Delete a hotel and all its hotels rooms will
 *                                              //    be deleted as well.
 *
 * 
 * See the Context class below and read the comments to see all the details of data model
 * declaration. 
 *
 ***************************************************************************************************
 *
 * STEP 4: CONFIGURING THE INSTANTIATION OF THE CONTEXT OBJECT
 *
 * This step is an somewhat of deviation, as it's about configuring the instantiation of
 * the Context object, not about the schema. However, it is a necessary process, if not
 * easy to understand. Do note that section may contain inaccuracies and errors.
 *
 * A Context object needs to have configuration information regarding the database loaded
 * into it before it can instantiated. This bundle of config data is often called a
 * connection string. This includes pieces of data like: Host's name, the database's name
 * and authentication credentials for the database.
 *
 * That makes sense: a Context object contains a map to the layout of a particular
 * database, so configuration and access information for that database should be folded
 * into the Context object. There are three ways of configuring the connection string and
 * adding it to Context objects:
 * 
 *
 *     1. WITHIN THE CONTEXT CLASS
 *        "Override the OnConfiguring(DbContextOptionsBuilder) method to configure the
 *        database (and other options) to be used for the context."[1]
 *
 * 
 *     2. OUTSIDE THE CONTEXT CLASS: IN A DbContextFactory CLASS, USE A
 *        DbContextOptionsBuilder OBJECT TO BUILD A DbContextOptions<TContext> OBJECT
 *        THAT BUNDLES THE CONNECTION STRING. THEN PASS IT TO THE CONTEXT CONSTRUCTOR
 * 
 *        "Alternatively, if you would rather perform configuration externally instead of
 *        inline in your context, you can use DbContextOptionsBuilder<TContext> (or
 *        DbContextOptionsBuilder) to externally create an instance of DbContextOptions<TContext>
 *        (or DbContextOptions) and pass it to a base constructor of DbContext."[1]
 *
 * 
 *     3. DECLARE A DBCONTEXT OBJECT IN STARTUP.CS AS A SERVICE. BUNDLE THE CONNECTION
 *        STRING INTO IT AND PROVIDE IT AS A SERVICE TO THE APPLICATION VIA DEPENDENCY
 *        INJECTION
 * 
 *        A Context class is a Service. You can register services with the Dependency
 *        Injection Framework in your project during application startup. This is usually
 *        done in Startup.cs's ConfigureServices() method.
 *
 *        If you want to see an commented out implementation of this third approach, go
 *        to Startup.cs and check the ConfigureServices() method. 
 *
 *        To register the Context object as a service, first you have to define the
 *        configuration data in a connection string. You can do this either inline in
 *        ConfigureServices(), or following best practices, in the appsettings.json file. 
 *
 *        This line of code adds the connection string to the Context class, then
 *        declares the database Context Service:
 * 
 *
 *            services.AddDbContext<Context>(options => options.UseNpgsql(connectionString));
 *
 * 
 *        As with other services, a Context will be registered with an IServiceCollection.
 *        When the application starts, a service provider will be built using the list of
 *        services defined in IServiceCollection. The Service Provider will resolve and
 *        inject dependencies into application classes as required. For instance, the
 *        Service Provider will provide the Context service to Controller classes, so
 *        that they can interface with the database as part of their duties.
 *
 * 
 * In this project, I've chosen to do the configuration externally (Approach 2). Look in
 * the 'Data' folder and open MyDbContextFactory.cs. A DbContextOptionsBuilder<TContext>
 * object is created and provided with configuration data which includes the Context
 * class, host name, database name and database login credentials. The DbContextOptionsBuilder
 * object will load the connection string and other config data into a DbContextOptions
 * object.
 *
 * The DbContextOptions object is then passed to the Context class's constructor, where it
 * becomes a part of the newly instantiated Context object. 
 *
 ***************************************************************************************************
 *
 * SOURCES
 *
 * 1: https://docs.microsoft.com/en-us/ef/core/api/microsoft.entityframeworkcore.dbcontext
 */



using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Hotel_Server.Database
{
    // DbContext is an EF Core class. Your Context class must inherit from it.
    public class Context : DbContext
    {
        // When Entity Framework Core reads and translates data from the database, it
        // needs a place to store this information at runtime on the program's side. That
        // is done in 'DbSet<>' data structures. The properties below are a list of data
        // structures, each one designed to hold a separate kind of record. In the
        // 'Hotels' data structure, EF Core will store 'Hotel' records and so on. 
        public virtual DbSet<Hotel> Hotels { get; set; }
        public virtual DbSet<HotelRoom> HotelRooms { get; set; }
        public virtual DbSet<RoomReservation> RoomReservations { get; set; }
        public virtual DbSet<RoomType> RoomTypes { get; set; }
        public virtual DbSet<BedType> BedTypes { get; set; }
        
        
        // CONTEXT OBJECT INSTANTIATION APPROACH 1 (See Step 4, Approach 1)
        //
        // This is a Context class constructor that "initializes a new instance of the 
        // DbContext class using the specified options." 
        // 
        // If you choose to, you can do all Context object instantiation and configuration
        // in the constructor. You have to specify the connection string (possibly in a)
        // DbContextOptions object. However, this project implements Approach 2.
        public Context(DbContextOptions options) : base(options)
        {
            
        }


        // DECLARING THE SCHEMA
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // DEFINING THE 'O' in ORM (OBJECTS)
            // Declare the Hotel Table. Also see the HotelRoom declaration.
            //
            // A modelBuilder object was passed in to OnModelCreating. Its Entity method
            // is being invoked and passed a generic object, Hotel.
            //
            // This method acts like a foreach loop. It loops over every Hotel object;
            // each object will be referred to as an entity temporarily. On each Hotel
            // object, EF Core will perform the operations listed in the curly brackets. 
            modelBuilder.Entity<Hotel>(entity =>
            {
                // DECLARING COLUMNS AND THEIR CHARACTERISTICS
                //
                // Note that every entity has a property check made on it, of this form:
                // 
                //     (e => e.Property) 
                // 
                // What is happening here? Recall that a model class's properties map on
                // to a table's columns in the database. On each entity object (in this
                // case, every Hotel object) another loop is executed, searching for the
                // 'e.x' property. Entity Framework Core checks both the database and the
                // corresponding model class for a column and a property respectively,
                // with a matching name.
                //
                // When the existence of the property has been verified on both ends, EF
                // Core applies the method calls that follow each property. For instance,
                // look at the 'Name' property:
                //
                //    entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                //
                // There are two limits/annotations that have to be made to the 'Name' 
                // property: the max character count is set to 100 and the field is made
                // mandatory. It must have a value entered if you want to save the value
                // in the database. It would be wise to consult the database for your 
                // project and ascertain that it imposes the same rules as the ones 
                // defined in the schema.
                //
                // There are several methods of this kind: HasMaxLength(), HasColumnType(),
                // and others to do a similar thing as Data Annotation Attributes did in
                // model classes. 
                //
                // Note that EF Core will assume that for an entity, any property with
                // the term 'Id' in it is the Primary Key. Primary Keys can also be
                // explicitly declared with 'HasKey()'. Other keys are declared with the
                // 'Property' keyword. The 'IsRequired()' method puts the 'Not Null'
                // constraint on the column; therefore, when a record is created in the
                // database, fields marked IsRequired() must be filled out or record
                // creation will fail.
                //
                // THE 'VIRTUAL' KEYWORD
                // Aside: If you use the term 'virtual' for a property in a Model class,
                // it tells EF Core that this property is a Navigation property. A
                // Navigation property is a pointer that leads from one entity to another.
                // You can use dot-notation from the navigation property of one entity to
                // peek at other entities.

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Address).IsRequired().HasMaxLength(200);
                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(100);
            });

            
            // Declare the HotelRoom Class. 
            modelBuilder.Entity<HotelRoom>(entity =>
            {
                // Declare columns in the HotelRoom Table. Composite key is declared on 
                // the first line.
                entity.HasKey(x => new { x.RoomNumber, x.HotelId });
                entity.Property(x => x.RoomNumber).IsRequired();
                entity.Property(x => x.HotelId).IsRequired();
                entity.Property(x => x.NightlyRate).IsRequired();
                entity.Property(x => x.NumberOfBeds).IsRequired();
                entity.Property(x => x.RoomTypeId).IsRequired();
                entity.Property(x => x.BedTypeId).IsRequired();

                // DEFINING THE 'R' in ORM (OBJECT-RELATIONS)
                
                // Here, the relationship between Hotel and HotelRoom objects are
                // detailed. Let's talk about inter-table relationships. In database
                // lingo, an entity can have One-to-Many, Many-to-Many and One-to-One
                // relationships to other entities. In the following code block, we will
                // be defining these inter-relationships.
                //
                // This code block defines the relationship between two entities, Hotel
                // and HotelRoom, from the perspective of the HotelRoom entity. (This
                // entity block is for HotelRoom after all.) 
                //
                // One Hotel can own multiple HotelRoom objects. The first line declares
                // the 'One' side of the relationship. The second line declares the
                // 'Many' side of the relationship.
                //
                // The third line declares a HotelRoom object has a foreign key property
                // called HotelId. The fourth line specifies what happens when a Hotel
                // object is deleted: logically, all its HotelRoom properties get deleted
                // too. In short, if the principal entity is deleted, so are the dependent
                // entities.
                
                entity.HasOne(e => e.Hotel)              // 1. HotelRoom can be owned by only ONE Hotel object.
                    .WithMany(e => e.HotelRooms)         // 2. A Hotel object can have many HotelRooms. (One-to-Many Relationship)
                    .HasForeignKey(e => e.HotelId)       // 3. The HotelRoom table has 'HotelId' as foreign key.
                    .OnDelete(DeleteBehavior.Cascade);   // 4. Delete a hotel and all its hotels rooms will 
                                                         //    be deleted as well.
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
