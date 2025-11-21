using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage1._0.Vehicles
{
    internal class Car : Vehicle
    {
        public string Fueltype { get; set; }
        public Car(int regnr, int color, int weels, int model) : base(regnr, color, weels, model)
        {
        }

        public Car(int regnr, int color, int weels, int model, string fueltype) : base(regnr, color, weels, model)
        {
        }
    }
}
