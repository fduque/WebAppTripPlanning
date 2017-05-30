using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public class WorldContextSeedData
    {
        private WorldContext _context;
        private UserManager<WorldUser> _userManager;

        public WorldContextSeedData(WorldContext context, UserManager<WorldUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public DateTime DateCreated { get; private set; }

        public async Task EnsureSeedData()
        {
            //se o usuario nao existir... ele vai criarcriando um novo usuario
            if (await _userManager.FindByEmailAsync("sam.hastings@theworld.com") == null)
            {
                var user = new WorldUser()
                {
                    UserName = "samhastings",
                    Email = "sam.hastings@theworld.com"

                };
                await _userManager.CreateAsync(user, "P@ssw0rd!");
            }




            if (!_context.Trips.Any())
            {
                var usTrip = new Trip()
                {
                    DateCreated = DateTime.UtcNow,
                    Name = "US Trip",
                    UserName = "samhastings", 
                    Stops = new List<Stop>()
                    {
                        new Stop() {Name = "Atlanta,GA", Arrival= new DateTime(2014,6,4), Latitude = 33.748995, Longitude = -84.387982, Order = 0 } 
                    }
                };
                _context.Trips.Add(usTrip);
                _context.Stops.AddRange(usTrip.Stops);

                var worldTrip = new Trip()
                {
                    DateCreated = DateTime.UtcNow,
                    Name = "World Trip",
                    UserName = "samhastings", //TODO Add Username
                    Stops = new List<Stop>()
                    {
                          
                        new Stop() {Name = "Atlanta,GA", Arrival= new DateTime(2014,6,4), Latitude = 33.748995, Longitude = -84.387982, Order = 0 }
                    
                    }
                };
                _context.Trips.Add(worldTrip);
                _context.Stops.AddRange(worldTrip.Stops);


                await _context.SaveChangesAsync();
            }




        }
    }
}
