using System;
using System.Linq;
using Common.Models;
using PostgresEFCore.Providers;


namespace Hotel_Server.Database
{
    /// <summary>
    /// This class checks whether a 'HotelReservationSystemDB' database exists and populates it.
    /// The class checks if there are any hotels in the database, and if not, it assumes the
    /// database is new and needs to be seeded with test data. It loads test data into arrays
    /// rather than List<T> collections to optimize performance.
    /// </summary>
    public static class DatabasePopulator
    {
        public static void Initialize(Context context)
        {
            context.Database.EnsureCreated();
            
            
            // Look for any hotels in the database.
            if (context.Hotels.Any())
            {
                return;   // DB has been seeded; Exit this program.
            }
            
            
            // If no hotel objects are found, populate the database as follows:
                        
            
            // ROOMTYPES
            // The RoomType and BedTypes arrays will populate tables intended to be rather static
            // over time.
            
            // In an array, create and populate a RoomType array:
            RoomType[] typesOfSuites = new RoomType[11]
            {
                // REGULAR SUITES
                new RoomType{Name = "Single Suite"},
                new RoomType{Name = "Double Suite"},
                new RoomType{Name = "Triple Suite"},
                new RoomType{Name = "Quad Suite"},
                
                // SPECIALTY SUITES
                new RoomType{Name = "Apartment Suite"},
                new RoomType{Name = "Accessibility Suite"},
                new RoomType{Name = "Smoking Suite"},
                
                // PREMIUM SUITES
                new RoomType{Name = "Executive Suite"},
                new RoomType{Name = "Honeymoon Suite"},
                new RoomType{Name = "Cabana Suite"},
                new RoomType{Name = "Presidential Suite"}
            };

            // Load RoomType objects into the database.
            foreach(var suiteType in typesOfSuites)
            {
                context.RoomTypes.Add(suiteType);
            }
            context.SaveChanges();
            
            
            // BEDTYPES
            // In an array, create and populate a BedType array:
            BedType[] bedTypes = new BedType[10]
            {
                // SINGLE BEDS:
                // Small Beds
                new BedType{Name = "Small, Single Bed"}, 
                new BedType{Name = "Small, Single Long Bed"}, 
                // Standard Beds
                new BedType{Name = "Standard, Single Bed"}, 
                new BedType{Name = "Standard, Single Long Bed"}, 
                // Premium Beds
                new BedType{Name = "Single, Full-size Bed"},
                
                
                // DOUBLE BEDS
                // Small Beds
                new BedType{Name = "Small, Double Bed"},
                // Standard Beds
                new BedType{Name = "Standard, Double Bed"},
                // Premium Beds
                new BedType{Name = "Queen-size, Double Bed"}, 
                new BedType{Name = "King-size, Double Bed"}, 
                new BedType{Name = "Emperor-size, Double Bed"}
            };
            
            // Load BedType objects into the database.
            foreach(var bedType in bedTypes)
            {
                context.BedTypes.Add(bedType);
            }
            context.SaveChanges();
            
            
            // Once the static tables have been populated, you can populate the main tables of the
            // database. This includes Hotel, HotelRoom and RoomReservation models.
            
            // HOTELS
            // // In an array, create and populate a Hotel array:
            Hotel[] hotels = new Hotel[]
            {
                new Hotel{Name = "Forward Operating Base Comfort", Address = "Hellscape", PhoneNumber = "1-800-289-8234"},
                new Hotel{Name = "Sewage Treatment Plant 85", Address = "New York City", PhoneNumber = "234-603-9387"},
                new Hotel{Name = "Major Trauma Centre, Whiskey", Address = "Timbuktu", PhoneNumber = "329-238-2859"},
                new Hotel{Name = "Political Theatre Centre", Address = "Monmouth", PhoneNumber = "859-289-8839"},
                new Hotel{Name = "Chipzilla Fab 19", Address = "Miami", PhoneNumber = "1-800-901-8924"},    
            };
            
            // Load Hotel objects into the database.
            foreach(var hotel in hotels)
            {
                context.Hotels.Add(hotel);
            }
            context.SaveChanges();
            
            
            // HOTELROOM AND ROOMRESERVATION OBJECTS
            // These objects require the HotelId property to be known, which are generated when
            // Hotel objects are added to the database. Due to this fact, I will not be able to 
            // provide HotelId values for HotelRoom and RoomReservation objects here. Therefore,
            // these tables will have to be written by hand after DatabasePopulator.cs fills the
            // database with RoomType, BedType and Hotel objects.
        }

    }
}