/* CONTROLLER CLASSES AND ENDPOINT METHODS
 * 
 * One of the main purposes of the Hotel-Server project is to define controllers (The other is to 
 * set up the Data Access Layer.). The Controller refers to the C in the MVC pattern. What is a
 * controller? When a user provides input to an application, controllers process and respond to it.
 * Controllers contain logic to query and alter models and to create and update new views.
 *
 * The Controller for this project is the class AllControllers. It appears that either .NET Core or
 * Entity Framework Core magically instantiates this class behind the scenes and uses it direct
 * control flow. In a non-networked program, a Controller class would contain regular methods that
 * control application flow by reacting to user actions. But for networked projects such as this
 * one, where a multi-tier architecture separates the project into a user-facing client program,
 * a server-based database backend and a Common classes library, the client must communicate to the
 * database over a network. In this context, 'Network' can refer to the internet or LANs such as
 * your home network. If the client and server are running on the same machine, the communication
 * happens only on the local network.
 *
 * To facilitate inter-program communication over a network, Hotel-Server must have its Controller 
 * class (AllControllers) expose an API to said network. Through this API, any network-based 
 * program can communicate with the database. In the case of networked programs, APIs are exposed 
 * through public methods called ENDPOINTS. In the context of an API, Endpoints are just public 
 * methods inside Controller classes that can be called over a network. All endpoints are 
 * controller methods, but not all controller methods are endpoints. 
 *
 * 
 **************************************************************************************************
 **************************************************************************************************
 * 
 * 
 * ROUTING IN ASP.NET CORE
 * 
 * If you run Hotel-Server, it should open a URL in a browser tab that looks like this:
 * 
 *      http://localhost:23598/Home/Index
 * 
 * These are the parts of a URL:
 * 
 * http: is the network PROTOCOL.
 * localhost: is a universal name for the local computer (HOST) on which the program is run.
 * 23598: is the PORT where IIS (Internet Information Services, a Microsoft web server that comes
 *        standard on most recent versions of Windows) is hosting this ASP.NET Core application.
 *        IIS will find an use a port that is not being used to prevent conflicts. 
 * Home: is a reference to the CONTROLLER of the application. 
 * Index: In the Home Controller on the local host, the Index ACTION is being sought.
 * 
 * The Protocol and the Host are not part of the ASP.NET Core MVC stack. They are managed by your
 * Network Firewall and Server. Neither of these will be discussed here. When you want to deploy
 * this program in a production environment, you will have to talk to your company's IT services
 * to determine the specific URL. The Controller and Action portions of the URL are managed by the
 * MVC framework.
 * 
 * ASP.NET Core has something called ROUTES. Here's an explanation of what is does: 
 * 
 *      
 * 
 * MVC maps Home to a Controller and Index to an Action through a ROUTING TEMPLATE. A Routing Template is a 
 *
 * 
 **************************************************************************************************
 **************************************************************************************************
 * 
 * 
 * REST ARCHITECTURE
 * 
 * AllControllers implements the REST (Representational State Transfer) architecture. It's an
 * architecture for managing state information in designing distributed systems. "It is not a
 * standard but a set of constraints, such as being stateless, having a client/server relationship,
 * and a uniform interface. REST is not strictly related to HTTP, but it is most commonly 
 * associated with it." [1] All network-facing APIs i.e. Endpoints, that are intended for 
 * communicating over the internet use the HTTP protocol to send and receive requests.
 *
 * Therefore, in AllControllers, there are many endpoints that feature HTTP requests such as GET, 
 * PUT, POST  etc. These endpoints implement Create, Read, Update and Delete (CRUD) operations for 
 * Hotel, HotelRoom and RoomReservation objects. The other tables in the database are lookup tables, 
 * on which CRUD operations will not be performed; for this reason, only these three tables have 
 * endpoints in this project.
 *
 * 
 **************************************************************************************************
 **************************************************************************************************
 * 
 * 
 * GENERATING CONTROLLER CLASSES
 * 
 * The methods in this class were all auto-generated. 
 * 
 * VISUAL STUDIO
 * 
 * In Visual Studio, right-click the Controller folder, select 
 * Add | Controller | (API Controller with actions, using Entity Framework). In the message box 
 * that pops up, select the Model class (Hotel, HotelRoom or RoomReservation), the Data Context 
 * Class (Context.cs), and the name of the Controller. Do this three times to generate controllers 
 * for Hotel, HotelRoom and RoomReservation objects. HotelController.cs is an amalgamation of 
 * these three files. 
 * 
 * INTELLIJ RIDER
 * As of September 11, 2017, this feature does not exist in IntelliJ Rider.[2] You may be able to 
 * generate it through the terminal. 
 * 
 * Go the the main program folder (Hotel-Server) and find the project folder (also named 
 * Hotel-Server). Open a command prompt window. If you want to see general options, run 
 * 'dotnet ef -help'.
 *
 * 
 **************************************************************************************************
 **************************************************************************************************
 * 
 * 
 * SOURCES
 * 1: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/routing
 * 2: https://spring.io/understanding/REST
 * 3: https://stackoverflow.com/questions/41011700/how-to-generate-controller-using-dotnetcore-command-line
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Common.Models;
using PostgresEFCore.Providers;


namespace Hotel_Server.Controllers
{
    /* CLASS ATTRIBUTES
     * 
     * Attributes are often used on methods and properties, but they can be added
     * to the class. What the attributes do depend on which ones were included. Attributes can be
     * thought of as helper tags. Sample use cases: You can use attributes to require that a user 
     * has to be authenticated to be able to use that class, or specify a middleware, an exception
     * handler, or just extra information the class can use.
     *
     * For the AllControllers class, the attributes add metadata to the assemblies. The first
     * attribute probably warns consumers of this class that its output is in JSON. The second
     * attribute specifies that routes to the endpoints in this class must begin with 
     * `api/AllControllers`. See below for much more on Route variables, which are hugely 
     * important.
     */
    [Produces("application/json")]
    [Route("api/AllControllers")]
    public class AllControllers : Controller
    {
        /* INSTANCE OF CLASS CONTEXT
         * 
         * This property is an instance of class Context. Class Context defines the Data Model of 
         * the database. All endpoints in this class, which are defined in the methods below, talk 
         * to the database through _context. As this property is readonly, it prevents modification 
         * of the model relationships in the class when the program is running, but through it, the
         * AllControllers class can perform Create, Read, Update & Delete (CRUD) operations on the 
         * database.
         */
        private readonly Context _context;


        /* ALLCONTROLLERS CONSTRUCTOR
         * 
         * When this program runs, AllControllers will be instantiated, and as noted before,
         * a private object of the Context class will be passed to it. _context will use the 
         * information that was defined in the MyDbContextFactory class (hostname, username, 
         * password, connection string and database name) and the information in Context.cs,
         * which contains all the Model and Relational information, to create the database. 
         * This gives the controller methods below direct access to the database.
         */
        public AllControllers(Context context)
        {
            _context = context;
        }


        // HOTEL CONTROLLERS (METHODS AND ENDPOINTS)

        // This private method (not an endpoint) checks the database to determine if a Hotel 
        // object exists.
        private bool HotelExists(long id)
        {
            return _context.Hotels.Any(e => e.Id == id);
        }


        // This endpoint returns a Collection of all Hotels in the database. (R in CRUD)
        // GET: api/AllControllers
        [HttpGet]
        public IEnumerable<Hotel> GetHotels()
        {
            return _context.Hotels;
        }


        /* ROUTE VARIABLE FOR ENDPOINT METHODS
         * 
         * When a client program talks to an a server over a network, it cannot directly invoke 
         * endpoints in server-side programs. Instead, the client has to send an HTTP request to
         * the server when it wants to invoke an endpoint in a Web API. In the request body,
         * the client will include all the data the invoked endpoint will need as a string. This 
         * data string is called the ROUTE. This is why Routes are needed despite the fact that
         * endpoints have names.
         * 
         * Before making a call to an endpoint, consult the API documentation to identify the route
         * of the endpoint you want to invoke. In MVC, the route has a standardized format:
         * {domain}/{controller}/{action}. The {domain} name is the address that hosts the server.
         * The {controller} is the name of the controller. The {action} is an identifier for a
         * specific endpoint in the controller class.
         * 
         * Just above this endpoint, note this attribute: [HttpGet("{id}")]. The "HttpGet" part
         * lets the method know that it will be invoked through incoming HTTP GET requests. The
         * "{id}" is the action part of this endpoint's route. Actions are defined in an attribute 
         * above an endpoint method. They can be a number or strings; check the type of the 
         * endpoint's paramter.
         * 
         * Let's look at some examples:
         *
         *     Route: "aws.amazon.com/hostName/Hotel/5" (domain/controller/action)
         *     Action: "5" OR "users/98" OR "users/3453/parent/mother/children" 
         *
         * 
         * THE GetHotel() ENDPOINT
         * 
         * When the client invokes this endpoint, GetHotel(), it will retrieve a Hotel object from
         * the database and return it to the caller. GetHotel() specifies its input requirements
         * in its parameters: it requires the caller to provide the ID number of the desired hotel
         * object. Note that the id parameter is actually annotated with the [FromRoute] attribute
         * which corresponds to the [HttpGet("{id}")] attribute above GetHotel(). This means that
         * id argument will come from the route of the current request. It must be a long as
         * demanded by the parameter.
         * 
         * The auto-generated comment above this endpoint notes the general route format:
         * api/AllControllers/5. This appears to be a generic, example route, not a real one. 
         * 
         * What does this endpoint's action value look like? Is the id value a Hotel object ID
         * number? If so, how can it be unique to this endpoint? How does action value help to 
         * distinguish between two endpoints that accept long values when callers cannot invoke 
         * by method name? The answers are unknown at this point.
         * 
         * Potentially Useful Source on Attribute Routing:
         * https://docs.microsoft.com/en-us/aspnet/web-api/overview/web-api-routing-and-actions/attribute-routing-in-web-api-2
         */

        // This endpoint method returns a Hotel object from the Database to the client. (R in CRUD)
        // GET: api/AllControllers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHotel([FromRoute] long id)
        {
            // Task<IActionResult> is the standard response for an asynchronous method.
            // IActionResult is the standard response type for a synchronous method.

            // This if-statement's purpose is unknown; it may be checking if all the properties of 
            // the model have values.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Note the id argument passed into this method. This line searches the database for an
            // object matching the id argument. This doesn't have to be done asynchronously, but 
            // since this will take an unknown amount of time, you don't want this request to block
            // the running of the rest of the application.
            var hotel = await _context.Hotels.SingleOrDefaultAsync(m => m.Id == id);

            if (hotel == null)
            {
                return NotFound();
            }

            // This line returns an HTTP status code for OK and a hotel object from the database.
            // The HTTP Status code for OK = 200.
            return Ok(hotel);
        }

        // This method updates a Hotel object in the Database. (U in CRUD). The "HttpPut" 
        // statement below requires an Hotel id value. This endpoint will update the matching
        // Hotel object. 
        // PUT: api/AllControllers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel([FromRoute] long id, [FromBody] Hotel hotel)
        {
            // This if-statement's purpose is unknown.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // The client makes a request and it comes in over the network. Before proceeding, this 
            // if statement checks if the Id number is found in the database. The if statement is 
            // checking if the Id from the route string (see the attribute above this method) 
            // matches a Hotel Id in the database. (??)
            if (id != hotel.Id)
            {
                return BadRequest();
            }

            // This statement sets some state property to indicate that the object was modified so 
            // EF Core will do something with it.
            _context.Entry(hotel).State = EntityState.Modified;

            // This try-catch statement is attempting to save the state change above asynchronously.
            // It is prepared to catch and handle a concurrency exception that might be thrown if 
            // the Async-await attempt fails.
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // This returns an HTTP status code. NoContent's status code = 204. This is returned
            // as a IActionResult, which has to be wrapped in Task<> in async methods.
            return NoContent();
        }


        // This endpoint method receives a Hotel object from the client and creates a new Hotel in
        // the Database. (C in CRUD)
        // POST: api/AllControllers
        [HttpPost]
        public async Task<IActionResult> PostHotel([FromBody] Hotel hotel)
        {
            // This if-statement's purpose is unknown.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // A Hotel object is added to the Database asynchronously. 
            _context.Hotels.Add(hotel);
            // This statement adds the object to the database asynchronously so the program doesn't 
            // hang waiting for it to complete.
            await _context.SaveChangesAsync();

            // Poor Method Name. This instruction is unclear.
            return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);
        }


        // This endpoint method receives the ID of a Hotel object from the client and deletes this
        // object from the Database. (D in CRUD)
        // DELETE: api/AllControllers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel([FromRoute] long id)
        {
            // This if-statement's purpose is unknown.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // The next section of statements searches the database asynchronously for a hotel 
            // object that matches the id argument...
            var hotel = await _context.Hotels.SingleOrDefaultAsync(m => m.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            // ... which is then deleted from the database.
            _context.Hotels.Remove(hotel);
            // This statement does the delete to the database asynchronously so the program doesn't 
            // hang waiting for it to complete.
            await _context.SaveChangesAsync();

            return Ok(hotel);
        }




        // HOTELROOM CONTROLLERS (METHODS AND ENDPOINTS)

        private bool HotelRoomExists(long id)
        {
            return _context.HotelRooms.Any(e => e.RoomNumber == id);
        }


        // GET: api/HotelRooms
        [HttpGet]
        public IEnumerable<HotelRoom> GetHotelRooms()
        {
            return _context.HotelRooms;
        }


        // GET: api/HotelRooms/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetHotelRoom([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hotelRoom = await _context.HotelRooms.SingleOrDefaultAsync(m => m.RoomNumber == id);

            if (hotelRoom == null)
            {
                return NotFound();
            }

            return Ok(hotelRoom);
        }


        // PUT: api/HotelRooms/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotelRoom([FromRoute] long id, [FromBody] HotelRoom hotelRoom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != hotelRoom.RoomNumber)
            {
                return BadRequest();
            }

            _context.Entry(hotelRoom).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelRoomExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // POST: api/HotelRooms
        [HttpPost]
        public async Task<IActionResult> PostHotelRoom([FromBody] HotelRoom hotelRoom)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.HotelRooms.Add(hotelRoom);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (HotelRoomExists(hotelRoom.RoomNumber))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetHotelRoom", new { id = hotelRoom.RoomNumber }, hotelRoom);
        }


        // DELETE: api/HotelRooms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotelRoom([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hotelRoom = await _context.HotelRooms.SingleOrDefaultAsync(m => m.RoomNumber == id);
            if (hotelRoom == null)
            {
                return NotFound();
            }

            _context.HotelRooms.Remove(hotelRoom);
            await _context.SaveChangesAsync();

            return Ok(hotelRoom);
        }




        // ROOMRESERVATION CONTROLLERS (METHODS AND ENDPOINTS)

        private bool RoomReservationExists(long id)
        {
            return _context.RoomReservations.Any(e => e.ReservationId == id);
        }


        // GET: api/RoomReservations
        [HttpGet]
        public IEnumerable<RoomReservation> GetRoomReservations()
        {
            return _context.RoomReservations;
        }


        // GET: api/RoomReservations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomReservation([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var roomReservation = await _context.RoomReservations.SingleOrDefaultAsync(m => m.ReservationId == id);

            if (roomReservation == null)
            {
                return NotFound();
            }

            return Ok(roomReservation);
        }


        // PUT: api/RoomReservations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoomReservation([FromRoute] long id, [FromBody] RoomReservation roomReservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != roomReservation.ReservationId)
            {
                return BadRequest();
            }

            _context.Entry(roomReservation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomReservationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // POST: api/RoomReservations
        [HttpPost]
        public async Task<IActionResult> PostRoomReservation([FromBody] RoomReservation roomReservation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.RoomReservations.Add(roomReservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoomReservation", new { id = roomReservation.ReservationId }, roomReservation);
        }


        // DELETE: api/RoomReservations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoomReservation([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var roomReservation = await _context.RoomReservations.SingleOrDefaultAsync(m => m.ReservationId == id);
            if (roomReservation == null)
            {
                return NotFound();
            }

            _context.RoomReservations.Remove(roomReservation);
            await _context.SaveChangesAsync();

            return Ok(roomReservation);
        }
    }
}