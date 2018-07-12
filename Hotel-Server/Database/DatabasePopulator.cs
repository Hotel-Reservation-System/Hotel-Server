using System;
using Common.Models;


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
        
    }
}