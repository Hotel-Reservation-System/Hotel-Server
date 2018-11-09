/* 1. PARTS OF THE SCHEMA
 *
 * When the Hotel-Server program is run, it will need to contact the database. It's
 * EF Core's responsibility to talk to the database. Recall that EF Core will attempt to
 * do two-way translations between the program and the database:
 *
 * 
 *                PROGRAM <----> ENTITY FRAMEWORK CORE <----> DATABASE
 *
 * 
 * To talk to a database, Entity Framework Core will need several things:
 *
 * 
 *     * A Map to the Layout of the Database: THE SCHEMA    (Context class)
 *     * The Database's CONFIGURATION LOADOUT               (DbContextOptions class)
 *
 *
 * Both these things are part of this class, the Context class. Entity Framework Core
 * will need a Context object to start a new database session. A DbContextOptions class
 * is a property of a Context object: a DbContextOptions object must be passed to a
 * Context object when the latter is instantiated.
 *
 * When the project starts, one of the first things EF Core will do is instantiate an
 * object from the Context class that you have written. This context object will be
 * provided to Controller classes, where it will be used to start a new database session.
 * This will help Controllers to carry out CRUD operations on the database.
 *
 * Let's examine the two parts of the schema in the two following sections.
 *
 ***************************************************************************************************
 *
 * 1.1 THE SCHEMA
 * 
 * The Schema is discussed in greater in '2.3 Databases - Concepts and Usage.txt' in the
 * 'Docs' folder. Check section '3.2.2 The Program-side Representation of Database
 * Entities: The Data Model vs. The Schema'.
 * 
 * However, permit me to provide a brief explanation of the schema again: It is a map to
 * the database. It contains information about an application's entity types and how they
 * map to the database. The schema specifically defines the entities in a database and
 * their relationships at a single point in time. Usually, the schema defines the
 * current state of a database's topology. Entity Framework Core needs a schema for every
 * database with which it has to interface.
 *
 * A database's schema is defined in a class that is generically called 'Context'. This
 * class plays an integral role in Entity Framework Core's operations. A Context class
 * must inherit from an EF Core class called 'Microsoft.EntityFrameworkCore.DbContext',
 * which is often known as DbContext class. The docs explains that[1]: "A DbContext
 * instance represents a session with the database and can be used to query and save
 * instances of your entities." Put succinctly, a Context session object helps EF Core
 * talk to the database.
 * 
 * The Context class is where you, the programmer, must define the schema to your
 * project's database. You must provide very specific definitions for Tables and other
 * database entities, as well as their relationships. With this information, a Context
 * object can serve as a bridge between the data model (classes that model database
 * entities) and database entities (records stored in the database).
 *
 ***************************************************************************************************
 *
 * 1.2 THE CONFIGURATION LOADOUT
 * 
 * Configuration information pertaining to a database also needs to be declared. This
 * includes: 
 * 
 *
 *     * Database Provider (A library that lets EF Core to talk a given vendor's
 *       database)
 *     * "Any necessary connection string or identifier of the database instance,
 *       typically passed as an argument to the provider selection method..."[2]. This
 *       includes stuff like:
 *         * Hostname (the name of program/computer on which the database is running)
 *         * The Database's Name
 *         * The Database Port (The on which it listens for requests).
 * 
 *     * "Any provider-level optional behavior selectors, typically also chained inside
 *       the call to the provider selection method"[2]
 *     * "Any general EF Core behavior selectors, typically chained after or before the
 *       provider selector method"[2]
 *     * Database Login Credentials
 * 
 *
 * This information is contained in a DbContextOptions class, which itself is a property
 * of a Context class. 
 *
 * 
 ***************************************************************************************************
 ***************************************************************************************************
 *
 *
 * 2. WRITING AND CONFIGURING A CONTEXT CLASS
 *
 * Before starting work on the Context class, you should have already finished writing
 * model classes. You will have to consult them repeatedly to complete the schema.
 *
 * The comments below are divided into 4 sections. Steps 1, 2 and 3 are about writing the
 * Schema. Step 4 is about declaring the database configuration loadout and configuring
 * how the Context session object will be instantiated. The Context session object needs
 * to be created and made available to program services that need it.
 * 
 *
 *     Step 1: Inherit from DbContext Class
 *     Step 2: Declare a DbSet<Table-Name> Property for Each Table in Your Database
 *     Step 3: Declaring Entities and Defining Inter-Entity Relationships
 *     Step 4: Configuring the Instantiation of the Context Object
 * 
 *
 ***************************************************************************************************
 *
 * STEP 1: INHERIT FROM DBCONTEXT CLASS
 *
 * When you write a Context class, first set it to inherit from DbContext. Call the class
 * whatever you want, plus the "-Context" suffix; e.g. 'ACMEContext', 'SchoolContext',
 * 'HotelContext' etc. or just 'Context' as I have for this project. It's a good idea to
 * name it after the database, as each database requires its own Context class.
 *
 ***************************************************************************************************
 *
 * STEP 2: DECLARE A DBSET<TABLE-NAME> PROPERTY FOR EACH TABLE IN YOUR DATABASE
 * 
 * The next thing you have to do is add one property to the class for each table that
 * exists in the database. Each of these properties have to be DbSet data structures. Why
 * are you doing this? When EF Core translates records from the database into C# objects,
 * it will store them in these data structures. Hotel objects in the Hotels DbSet data
 * structure, HotelRoom objects in the HotelRooms data structure, and so forth.
 *
 * From Microsoft's DbContext docs[1]:
 *
 * 
 *     Typically you create a class that derives from DbContext and contains
 *     DbSet<TEntity> properties for each entity in the model. If the DbSet<TEntity>
 *     properties have a public setter, they are automatically initialized when the
 *     instance of the derived context is created.
 * 
 *
 * First, you need to consult the data model classes (in the Models/Tables folder in the
 * 'Common' project), because they model table entities in your database.
 *
 * Then you have to add DbSet<T> data structures as properties to the Context class. You
 * will have to represent each table from the database. You do this by declaring a DbSet
 * object, and inserting the name of one model class in place of 'TEntity' in
 * 'DbSet<TEntity>'. Give each data structure a plural name, after the table/data model
 * they are representing. Repeat this process for all your data model classes.
 *
 * Here's an example data structure from this file:
 *
 * 
 *     public virtual DbSet<Hotel> Hotels { get; set; }
 *
 *
 * See the properties in this file if you want more examples of how DbSet<TEntity>
 * properties are to be declared.
 *
 ***************************************************************************************************
 *
 * STEP 3: THE SCHEMA: DECLARING ENTITIES AND DEFINING INTER-ENTITY RELATIONSHIPS
 *
 * In this section, I'll lightly delve into how to define the schema for this project's
 * database. Open up the the relevant Model class and consult it frequently when doing
 * this. The docs tell us how EF Core detects the schema:
 *
 * 
 *     The model is discovered by running a set of conventions over the entity classes
 *     found in the DbSet<TEntity> properties on the derived context. To further
 *     configure the model that is discovered by convention, you can override the
 *     'OnModelCreating(ModelBuilder)' method.
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
 * switch back to the entity declaration block here in the Context and declare these 
 * properties. 
 * 
 * While you're doing declaring properties, you should provide each property in this
 * entity block with appropriate limitations like character limits for a column, whether
 * a value should be generated by the database and whether a field is required or not.
 * You should consult the project's database to make sure the limits you impose in the
 * schema match the ones in the database. Defining these constraints promotes data
 * integrity by preventing poor quality information from entering the database.
 *
 * After declaring tables and their columns, you can start defining inter-entity
 * relationships. Invoke the 'entity.HasOne' property on a dependent entity (HotelRooms)
 * and specify the exact nature of this entity to an independent entity (Hotel). Here is
 * an example declaration for the HotelRoom entity: 
 *
 *     // In the first two lines, a One-to-Many relationship is defined.  
 *     entity.HasOne(e => e.Hotel)              // 1. HotelRoom can be owned by only ONE Hotel object. (The 'One' side.)
 *         .WithMany(e => e.HotelRooms)         // 2. A Hotel object can have many HotelRooms. (The 'Many' Relationship)
 *         .HasForeignKey(e => e.HotelId)       // 3. The HotelRoom table has 'HotelId' as foreign key.
 *         .OnDelete(DeleteBehavior.Cascade);   // 4. Delete a hotel and all its hotels rooms will
 *                                              //    be deleted as well.
 *
 * 
 * See the Context class below and read all the comments to get a better understanding of
 * how entity and inter-entity relationships are defined.
 *
 * This concludes the writing of the schema.
 *
 ***************************************************************************************************
 *
 * STEP 4: CONFIGURING THE LOADING OF CONFIG DATA INTO THE CONTEXT OBJECT
 *
 * This step is about configuring the instantiation of the Context object. This is a
 * necessary process, if not one that is easy to understand. Do note that this section
 * may contain inaccuracies and errors.
 *
 * A Context object needs to have configuration information regarding the database loaded
 * into it before it can instantiated. This includes pieces of data like: Host's name,
 * the database's name and authentication credentials for the database. This bundle of
 * config data is often contains the connection string and other other data. This data is
 * usually encapsulated within a DbContextOptions object.
 *
 * Loading configuration data into a Context object makes sense: a Context object
 * contains a map to the layout of a particular database, so configuration and access
 * information for that database should be folded into the Context object too.
 *
 * There are three possible ways of configuring the configuration loadout and adding it
 * to Context objects:
 * 
 *
 *     1. APPROACH 1: DECLARE IT WITHIN THE CONTEXT CLASS IN THE OnConfiguring() METHOD
 * 
 *        "Override the OnConfiguring(DbContextOptionsBuilder) method to configure the
 *        database (and other options) to be used for the context."[1] See an example
 *        below in the Context class.
 *
 * 
 *     2. APPROACH 2: DECLARE IT OUTSIDE THE CONTEXT CLASS: IN A DbContextFactory CLASS,
 *        USE A DbContextOptionsBuilder OBJECT TO BUNDLE CONFIGURATION DATA INTO A
 *        DbContextOptions<TContext> OBJECT, THEN PASS IT TO THE CONTEXT CONSTRUCTOR
 * 
 *        "Alternatively, if you would rather perform configuration externally instead of
 *        inline in your context, you can use DbContextOptionsBuilder<TContext> (or
 *        DbContextOptionsBuilder) to externally create an instance of
 *        DbContextOptions<TContext> (or DbContextOptions) and pass it to a base
 *        constructor of DbContext."[1]
 *
 * 
 *     3. APPROACH 3: DECLARE A DBCONTEXT OBJECT IN STARTUP.CS AS A SERVICE. BUNDLE
 *        CONFIGURATION DATA INTO IT AND PROVIDE IT AS A SERVICE TO THE APPLICATION VIA
 *        DEPENDENCY INJECTION
 * 
 *        A Context class is a Service. You can register services with the Dependency
 *        Injection Framework in your project during application startup. This is usually
 *        done in Startup.cs's ConfigureServices() method. If you want to see an commented
 *        out implementation of this third approach, go to Startup.cs and check the
 *        ConfigureServices() method. 
 *
 *        To register the Context object as a service, first you have to define the
 *        configuration data in a connection string. You can do this either inline in
 *        ConfigureServices(), or following best practices, in the appsettings.json file. 
 *
 *        This line of code adds the connection string/config data to the Context class,
 *        then declares the database Context as a service:
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
 *        that they can talk to the database as part of their duties.
 *
 * 
 * In this project, I've chosen to do the configuration externally (Approach 2). Look in
 * the 'Data' folder and open MyDbContextFactory.cs. A DbContextOptionsBuilder<TContext>
 * object is created and provided with configuration data which includes the Context
 * class, host name, database name and database login credentials. The DbContextOptionsBuilder
 * object will load the connection string and other config data into a DbContextOptions
 * object. The DbContextOptions object is then passed to the Context class's constructor,
 * where it becomes a part of the newly instantiated Context object. 
 *
 ***************************************************************************************************
 *
 * SOURCES
 *
 * 1: https://docs.microsoft.com/en-us/ef/core/miscellaneous/configuring-dbcontext
 * 2: https://docs.microsoft.com/en-us/ef/core/api/microsoft.entityframeworkcore.dbcontext
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
        
        
        // Context Constructor
        public Context(DbContextOptions options) : base(options)
        {
            
        }
        
        
        /* APPROACH 1: SUPPLYING DbContextOptions BY OVERRIDING OnConfiguring()
         * (See Step 4, Approach 1 in this file.)
         *
         * This project implements Approach 2. However, this section illustrates
         * Approach 1.
         * 
         * If you choose to, you can do declare configuration data in the OnConfiguring()
         * method. You have to specify the connection string and any other config data
         * you want. Here is an example:
         *
         *
         *     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
         *     {
         *         optionsBuilder.UseNpgsql("Host=localhost;" +
         *                                  "Username=postgres;" +
         *                                  "Password=password;" +
         *                                  "Database=HotelsDb");
         *     }
         */
    

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
                // DECLARING COLUMNS AND THEIR CONSTRAINTS
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
                // There are two constraint annotations that have to be made to the
                // 'Name' property: the max character count is set to 100 and the field
                // is made mandatory. It must have a value entered if you want to save
                // the value in the database. It would be wise to consult the database
                // for your project and ascertain that it imposes the same integrity
                // constraints as the ones defined here in the schema.
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
                
                // Here, the relationship between Hotel and HotelRoom entities are
                // configured. Let's talk about inter-table relationships. In database
                // lingo, an entity can have One-to-Many, Many-to-Many and One-to-One
                // relationships to other entities. In the following code block, we will
                // be defining such inter-relationships.
                //
                // This code block defines the relationship between two entities, Hotel
                // and HotelRoom, from the perspective of the HotelRoom entity. (After
                // all, this entity block is for HotelRoom.) 
                //
                // One Hotel can own multiple HotelRoom objects. The first line declares
                // the 'One' side of the relationship. The second line declares the
                // 'Many' side of the relationship.
                //
                // The third line declares a HotelRoom object has a foreign key property
                // called 'HotelId'. The fourth line specifies what happens when a Hotel
                // object is deleted: logically, all its HotelRoom properties get deleted
                // too. In short, if the principal entity is deleted, so are the dependent
                // entities.
                
                entity.HasOne(e => e.Hotel)              // 1. A HotelRoom can be owned by only ONE Hotel object. (The 'One' Side)
                    .WithMany(e => e.HotelRooms)         // 2. A Hotel object can have many HotelRooms. (The 'Many' Side)
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
