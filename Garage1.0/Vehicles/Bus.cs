using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage1._0.Vehicles
{
    internal class Bus : Vehicle
    {
        public int NumberOfSeats { get; set; }
        public Bus(string regnr, string color, int weels, string model) : base(regnr, color, weels, model)
        {
        }

        public Bus(string regnr, string color, int weels, string model, int NumberOfSeats) : base(regnr, color, weels, model)
        {
            this.NumberOfSeats = NumberOfSeats;
        }

        protected override string GetVehicleInfo()
        {
            return base.GetVehicleInfo() + $", Antal säten : {NumberOfSeats}";
        }
    }
}
