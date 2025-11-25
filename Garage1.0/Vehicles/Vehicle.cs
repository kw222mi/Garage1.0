using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage1._0.Vehicles
{
    public abstract class Vehicle
    {
        public string RegistrationNumber { get; set; }
        public string Color { get; set; }
        public int Wheels { get; set; }

        public string Model { get; set; }

       public Vehicle(string regnr, string color, int weels, string model) {
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
