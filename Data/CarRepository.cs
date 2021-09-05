using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarInfoFromDatabase.Data
{
    public class CarRepository:ICarRepository
    {
        private readonly CarContext _ctx;
        private readonly ILogger<CarRepository> _logger;

        public CarRepository(CarContext ctx,ILogger<CarRepository> logger)
        {
            this._ctx = ctx;
            this._logger = logger;
        }

   

        IEnumerable<Root> ICarRepository.GetAllCars()
        {
            try
            {
                _logger.LogInformation("GetAllCars was called");
                            return _ctx.root
                                       .OrderBy(p => p.carId)
                                       .ToList();
            }
            catch(Exception e)
            {
                _logger.LogError($"Failed to get all Cars: {e}");
                return null;
            }

        }


        public bool SaveAll()
        {
            return _ctx.SaveChanges()>0;
        }

        public Root GetCarById(int id)
        {
            return _ctx.root
                .Where(c => c.carId == id)
                .FirstOrDefault();
        }

        public void AddEntity(object model)
        {
            _ctx.Add(model);
        }
    }
}
