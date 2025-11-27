using Microsoft.VisualBasic.FileIO;
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
        public Car(string regnr, string color, int weels, string model) : base(regnr, color, weels, model)
        {
        }

        public Car(string regnr, string color, int weels, string model, string fueltype) : base(regnr, color, weels, model)
        {
            Fueltype = fueltype;
        }

        protected override string GetVehicleInfo()
        {
            return base.GetVehicleInfo() + $", Bränsletyp : {Fueltype}";
        }
    }
}
