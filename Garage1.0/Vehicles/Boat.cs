using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage1._0.Vehicles
{
    internal class Boat : Vehicle
    {
        public int Length { get; set; }
        public Boat(string regnr, string color, int weels, string model) : base(regnr, color, weels, model)
        {
        }

        public Boat(string regnr, string color, int weels, string model, int Length) : base(regnr, color, weels, model)
        {
           this.Length = Length;
        }

        protected override string GetVehicleInfo()
        {
            return base.GetVehicleInfo() + $", Längd : {Length}";
        }
    }
}
