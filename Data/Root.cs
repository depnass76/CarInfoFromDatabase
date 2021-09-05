using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CarInfoFromDatabase.Data
{
    public class Root
    {
        [Key]
        public int carId { get; set; }
        public string Name { get; set; }
        public double? Miles_per_Gallon { get; set; }
        public int Cylinders { get; set; }
        public double Displacement { get; set; }
        public int? Horsepower { get; set; }
        public int Weight_in_lbs { get; set; }
        public double Acceleration { get; set; }
        public string Year { get; set; }
        public string Origin { get; set; }
        public StoreUsers User { get; set; }

    }
}
