/* INTRODUCTION TO CONTROLLER CLASSES AND ENDPOINTS
 * 
 * One of the main purposes of the Hotel-Server project is to define controllers (The
 * other is to set up the Data Access Layer.). The Controller refers to the C in the MVC
 * pattern. What is a controller? From an MSDN article[1]:
 *
 * 
 *     Controllers process incoming requests, handle user input and interactions, and
 *     execute appropriate application logic. A controller class typically calls a
 *     separate view component to generate the HTML markup for the request.
 *
 * 
 * Simply put, controllers are classes that contain business logic methods which respond
 * to user input. When a request comes in, the controller will process it. To do this,
 * one or more of the methods in the controller may query the database to read and alter
 * models. Then, it may issue updates to the View. The updated View may be returned as a
 * response to the user request.
 *
 * Controller classes contain ACTION METHODS, which perform ACTIONS and return results
 * called ACTION RESULTS.
 *
 *
 * CONTROLLER CLASSES
 * 
 * Here's a little more on Controllers[2]:
 * 
 *
 *     A controller is used to define and group a set of actions. An action (or action
 *     method) is a method in a controller which handles requests. Controllers logically
 *     group similar actions together. This aggregation of actions allows common sets of
 *     rules, such as routing, caching, and authorization, to be applied collectively.
 *     Requests are mapped to actions through routing.
 * 
 *     By convention, controller classes:
 *
 * 
 * 
 *         * Reside in the project's root-level Controllers folder
 *         * Inherit from Microsoft.AspNetCore.Mvc.Controller
 *
 * 
 *     A controller is an instantiable class in which at least one of the following
 *     conditions is true:
 * 
 *
 *     * The class name is suffixed with "Controller".
 *     * The class inherits from a class whose name is suffixed with "Controller".
 *     * The class is decorated with the [Controller] attribute.
 * 
 *
 *     A controller class must not have an associated [NonController] attribute...
 *
 *     The controller is a UI-level abstraction. Its responsibilities are to ensure
 *     request data is valid and to choose which view (or result for an API) should be
 *     returned. In well-factored apps, it doesn't directly include data access or
 *     business logic. Instead, the controller delegates to services handle these
 *     responsibilities.
 *
 * 
 * From the first source[1], here is an excerpt that explains the class hierarchy:
 *
 * 
 *     The base class for all controllers is the ControllerBase class, which provides
 *     general MVC handling. The Controller class inherits from ControllerBase and is
 *     the default implement[ation] of a controller. The Controller class is responsible
 *     for the following processing stages:
 *
 *         * Locating the appropriate action method to call and validating that it can be
 *           called.
 *         * Getting the values to use as the action method's arguments.
 *         * Handling all errors that might occur during the execution of the action
 *           method.
 *
 * 
 * The ControllerBase supports MVC Controllers WITHOUT View support. The Controller class
 * is the base class for an MVC controller with View support. The Controller class has
 * useful properties related to Request and Response objects built into it. It appears
 * that either .NET Core or Entity Framework Core magically instantiates this class
 * behind the scenes and uses it direct control flow. 
 * 
 * The Controller for this project is the class AllControllers. (Note that its name
 * appears to violate the requirement that controllers need to have the suffix
 * -Controller.)
 *
 *
 * ACTIONS/ACTION METHODS
 *
 * ACTIONS/ACTION METHODS appear to ASP.NET Core jargon for ENDPOINTS. What are
 * endpoints?
 * 
 * In a non-networked program, a class would contain regular methods that control
 * application flow by reacting to user actions. But for networked projects such as this
 * one, where a multi-tier architecture separates the project into a user-facing client
 * program, a server-based database backend and a Common class library, the client must
 * communicate to the database backend over a network. In this context, 'Network' can
 * refer to the internet or LANs such as your home network. If the client and server are
 * running on the same machine, the communication happens only on the local network.
 *
 * To facilitate inter-program communication over a network, Hotel-Server must have its
 * Controller class (AllControllers) expose an API to said network. Through this API, any
 * network-based program can communicate with the database. In the case of networked
 * programs, APIs are exposed through public methods called ENDPOINTS. In the context of
 * a Web API project, Endpoints are just public methods inside Controller classes that
 * can be called over a network. All endpoints are controller methods, but not all
 * controller methods are endpoints, because Controllers can have private, helper
 * methods.
 * 
 * Here's the ASP.NET team's explanation for Actions[2]:
 *
 * 
 *     Public methods on a controller, except those decorated with the [NonAction]
 *     attribute, are actions. Parameters on actions are bound to request data and are
 *     validated using model binding. Model validation occurs for everything that's
 *     model-bound. The 'ModelState.IsValid' property value indicates whether model
 *     binding and validation succeeded.
 *
 *     Action methods should contain logic for mapping a request to a business
 *     concern. Business concerns should typically be represented as services that the
 *     controller accesses through dependency injection. Actions then map the result of
 *     the business action to an application state.
 *
 *     Actions can return anything, but frequently return an instance of IActionResult
 *     (or Task<IActionResult> for async methods) that produces a response. The action
 *     method is responsible for choosing what kind of response. The action result does
 *     the responding.
 *
 *
 * ACTION RESULTS
 * 
 * The Controller class has predefined methods with predefined return values. As noted in
 * the previous paragraph, most method return values indirectly implement the same
 * interface: IActionResult.
 *
 * The ActionResult abstract class is an implementation of IActionResult. Most Controller
 * class return data types derive from ActionResult. Look for methods that return objects
 * ending in the word 'Result'. These data types usually implement IActionResult.
 * Examples: ContentResult, EmptyResult, FileResult, HttpStatusCodeResult,
 * JavaScriptResult, JsonResult, RedirectResult and RedirectToRouteResult. 
 *
 * 
 * SOURCES:
 *
 * 1: https://msdn.microsoft.com/en-us/Library/dd410269(v=vs.98).aspx
 * 2: https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/actions
 *
 * 
 **************************************************************************************************
 **************************************************************************************************
 * 
 * 
 * REST ARCHITECTURE
 * 
 * AllControllers implements the REST (Representational State Transfer) architecture. It's
 * an architecture for managing state information in designing distributed systems. "It
 * is not a standard but a set of constraints, such as being stateless, having a
 * client/server relationship, and a uniform interface. REST is not strictly related to
 * HTTP, but it is most commonly associated with it." [1] All network-facing APIs i.e.
 * Endpoints, that are intended for communicating over the internet use the HTTP protocol
 * to send and receive requests. 
 *
 * Therefore, in AllControllers, there are many endpoints that feature HTTP requests such
 * as GET, PUT, POST  etc. These endpoints implement Create, Read, Update and Delete
 * (CRUD) operations for Hotel, HotelRoom and RoomReservation objects. The other tables
 * in the database are lookup tables, on which CRUD operations will not be performed; for
 * this reason, only these three tables have endpoints in this project.
 *
 * 
 **************************************************************************************************
 **************************************************************************************************
 * 
 * ROUTING IN ASP.NET CORE
 *
 * In .../Controllers/, look for 'Routing in ASP.NET Core.txt'.
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
 * Add | Controller | (API Controller with actions, using Entity Framework). In the
 * message box that pops up, select the Model class (Hotel, HotelRoom or RoomReservation),
 * the Data Context Class (Context.cs), and the name of the Controller. Do this three times
 * to generate controllers for Hotel, HotelRoom and RoomReservation objects.
 * HotelController.cs is an amalgamation of these three files. 
 * 
 * INTELLIJ RIDER
 * As of September 11, 2017, this feature does not exist in IntelliJ Rider.[2] You may be
 * able to generate it through the terminal.[] 
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
using Hotel_Server.Database;


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
         * WHY DO WE NEED IT?
         * 
         * The Controller class will issue orders that require reading and writing from the
         * database. Talking to the database is EF Core's job. To facilitate contact with the
         * database through EF Core, the Controller will need a copy of the database's Schema.
         * 
         * The _context property is an instance of class Context. Class Context defines the schema 
         * of the database. All endpoints in this class, which are defined in the methods below,  
         * talk to the database through _context. As this property is readonly, it prevents  
         * modification of the Context's schema when the program is running, but through it, the
         * AllControllers class can perform Create, Read, Update & Delete (CRUD) operations on the 
         * database.
         *
         * EF Core will translate these orders into SQL when sending them to the database, and
         * converts returned values into C#.
         *
         * Note: Instantiating a context object within a Controller class like this VIOLATES best
         * practices like SOLID. A better idea would be to use a using statement or even better
         * yet, use EF Core's Dependency Injection, which offers a very nice solution for
         * passing contexts to Controllers.
         *
         * CALLING SAVECHANGES()
         *
         * After doing any kind of operation that changes database values (Create, Update and
         * Delete operations from CRUD), you must call the SaveChanges() method on the Context
         * object. None of the changes you made will be propagated until you call SaveChanges().
         *
         * You can wait until after you've completed all operations to call this method, but it
         * would be a better idea to execute SaveChanges() immediately after Creating, Updating
         * or Deleting data, provided that you don't impact performance negatively. 
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