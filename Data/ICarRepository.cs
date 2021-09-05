using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarInfoFromDatabase.Data
{
    public interface ICarRepository
    {
        IEnumerable<Root> GetAllCars();

        Root GetCarById(int id);

        void AddEntity(object model);
        bool SaveAll();
    }
}
