using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public class WorldRepository : IWorldRepository
    {
        private WorldContext _context;
        private ILogger<WorldRepository> _logger;

        public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        public void AddStop(string tripName, Stop newStop)
        {
            var trip = GetTripByName(tripName);
            if (trip != null)
            {
                trip.Stops.Add(newStop);
                _context.Stops.Add(newStop);
            }
        }

        //database
        public void AddTrip(Trip trip)
        {
            _context.Add(trip);

        }

        //get trips collection from database
        public IEnumerable<Trip> GetAllTrips()
        {
            _logger.LogInformation("Getting all trips from databse");
            return _context.Trips.ToList();
        }

        public Trip GetTripByName(string tripName)
        {
            return _context.Trips
                .Include(t => t.Stops)
                .Where(t => t.Name == tripName)
                .FirstOrDefault();


        }

        public Trip GetUserTripByName(string tripName, string name)
        {
            return _context
                .Trips
                .Include(t => t.Stops)
                .Where(t => t.Name == tripName && t.UserName == name)
                .FirstOrDefault();
        }

        public IEnumerable<Trip> GetTripsByUsername(string name)
        {
            return _context
                .Trips
                .Include(t => t.Stops)
                .Where(t => t.UserName == name)
                .ToList();
                
        }

        //defines async way for database issues
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0 ;
        }

   
    }
}
