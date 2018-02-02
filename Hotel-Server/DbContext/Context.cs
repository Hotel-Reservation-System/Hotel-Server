/* CONTEXT CLASS
 *
 * Recall that EF Core will attempt to do two-way translations between this program and the
 * database.
 * 
 *                    PROGRAM <----> ENTITY FRAMEWORK CORE <----> DATABASE
 * 
 * This file contains one of the three things EF Core needs to act as ORM, namely, the Data Model.
 * 
 * In Context class, the programmer must specify the general parameters for Tables and other
 * entities in the database. As such, it serves as a bridge between the model classes (classes that
 * model database tables in the Models/Tables folder of the Common project) and database entities
 * (records stored in the database).
 *
 * This class, Context, inherits from an EF Core class called Microsoft.EntityFrameworkCore.DbContext.
 * From the docs: "A DbContext instance represents a session with the database and can be used to
 * query and save instances of your entities." Put succintly, the DbContext class helps you talk to
 * the database.
 *
 * The programmer must create Context.cs and set it to inherit from DbContext. Then, she has to
 * outline certain information about the database in this class. Use information from the Model
 * classes (from project Common) to define the Data Model of the database. See below for more
 * details on how to do this.
 */


using System;
using System.Collections.Generic;
using System.Text;
using Common.Models;
using Microsoft.EntityFrameworkCore;

namespace PostgresEFCore.Providers
{
    // DbContext is an EF Core class.
    public class Context : DbContext
    {
        // This is list of Models that will be turned into Tables in the Database.
        public virtual DbSet<Hotel> Hotels { get; set; }
        public virtual DbSet<HotelRoom> HotelRooms { get; set; }
        public virtual DbSet<RoomReservation> RoomReservations { get; set; }
        public virtual DbSet<RoomType> RoomTypes { get; set; }
        public virtual DbSet<BedType> BedTypes { get; set; }
        
        
        // This constructor calls the base constructor. "Initializes a new instance of the 
        // DbContext class using the specified options."
        public Context(DbContextOptions options) : base(options)
        {
            
        }


        // DECLARING DATA MODELS
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Declare the Hotel Class. Also see the HotelRoom declaration.
            modelBuilder.Entity<Hotel>(entity =>
            {
                // Declaring columns in the Hotel Table. Primary Keys are declared with HasKey(). 
                // Other keys are declared with the Property keyword. 
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Address).IsRequired().HasMaxLength(200);
                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(100);
            });

            // Declare the HotelRoom Class. 
            modelBuilder.Entity<HotelRoom>(entity =>
            {
                // This is where the 'O' in ORM in defined.
                // Declare columns in the HotelRoom Table. Composite key is declared on the first
                // line.
                entity.HasKey(x => new { x.RoomNumber, x.HotelId });
                entity.Property(x => x.RoomNumber).IsRequired();
                entity.Property(x => x.HotelId).IsRequired();
                entity.Property(x => x.NightlyRate).IsRequired();
                entity.Property(x => x.NumberOfBeds).IsRequired();
                entity.Property(x => x.RoomTypeId).IsRequired();
                entity.Property(x => x.BedTypeId).IsRequired();

                // This is where the 'R' in ORM in defined. Here, the relationship between Hotel
                // and HotelRoom is declared. 
                entity.HasOne(e => e.Hotel)             // HotelRoom is declared to be owned by Hotel objects.
                    .WithMany(e => e.HotelRooms)        // Hotels will have many HotelRooms.
                    .HasForeignKey(e => e.HotelId)      // Hotels have the HotelId foreign key.
                    .OnDelete(DeleteBehavior.Cascade);  // Deletes any dependent entities, so any row that
                                                        // has a foreign key to the deleted entity. Delete
                                                        // a hotel and all its hotels rooms will be deleted
                                                        // as well.
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
