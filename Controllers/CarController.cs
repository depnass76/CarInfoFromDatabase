using CarInfoFromDatabase.Data;
using CarInfoFromDatabase.Services;
using CarInfoFromDatabase.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarInfoFromDatabase.Controllers
{
    [Route("api/[Controller]")]
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class CarController : Controller
    {
        public  readonly ICarRepository _repo;
        public  readonly ILogger _logger;
     
        public CarController(ICarRepository repo, ILogger<CarController> logger)
        {
            this._repo = repo;
            this._logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
              return Ok(_repo.GetAllCars());
            }
            catch(Exception ex)
            {
                _logger.LogError($"Failed to get Car: {ex}");
                return BadRequest("Failed to get Car");
            }

        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var car = _repo.GetCarById(id);
                if (car != null) return Ok(car);
                else return NotFound();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get Car: {ex}");
                return BadRequest("Failed to get Car");
            }
        }



        [HttpPost]
        public IActionResult Post([FromBody]Root model)
        {
            try
            {

                _repo.AddEntity(model);
                if( _repo.SaveAll())
                {
                  return Created($"/api/car/{model.carId}", model);
                }



            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get Car: {ex}");

            }
            return BadRequest("Failed to get Car");
        }

        



    }
}