using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage1._0.Vehicles
{
    abstract class Vehicle
    {
        public int RegistrationNumber { get; set; }
        public int Color { get; set; }
        public int Wheels { get; set; }

        public int Model { get; set; }

       public Vehicle(int regnr, int color, int weels, int model) {
            RegistrationNumber = regnr;
            Color = color;
            Wheels = weels;
            Model = model;
        
        }

        public override string ToString()
        {
            return GetVehicleInfo();
        }

        protected virtual string GetVehicleInfo()
        {
            return $" RegistrationNumber: {RegistrationNumber} Color: {Color} Wheels: {Wheels} Model: {Model} "; ;
        }
    }
}
