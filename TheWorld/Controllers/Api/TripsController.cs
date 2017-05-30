using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.ViewModel;

namespace TheWorld.Controllers.Api
{
    [Route("api/trips")]
    [Authorize]
    public class TripsController : Controller
    {
        private IWorldRepository _repository;
        private ILogger<TripsController> _logger;

        public TripsController(IWorldRepository repository,ILogger<TripsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }


        [HttpGet("")]
        public IActionResult Get()
        {
        try
            {
                //var results = _repository.GetAllTrips();
                var results = _repository.GetTripsByUsername(this.User.Identity.Name);
                //importante colocar o IEnumerable para pegar toda coleção de retorno
                return Ok(Mapper.Map<IEnumerable<TripViewModel>>(results));
            }
        catch (Exception ex) 
            {

                _logger.LogError($"Failed to get All Trips: {ex}");
                return BadRequest(ex);

                }
            
}

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]TripViewModel theTrip)
        {
            //SAVE TO THE DATABASE
            if (ModelState.IsValid)
            {

                var newTrip = Mapper.Map<Trip>(theTrip);

                //definindo o usuario logado como usuario cadastrado na trip
                newTrip.UserName = User.Identity.Name;

                _repository.AddTrip(newTrip);

                //if savechanges async is ok, call Created...
                if(await _repository.SaveChangesAsync())
                {
                    return Created($"api/trips/{theTrip.Name}", Mapper.Map<TripViewModel>(newTrip));
                }
        
            }
            return BadRequest("Failed to save the trip");
        }
    }
}
