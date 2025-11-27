using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage1._0.Vehicles
{
    internal class Airplane : Vehicle
    {
        public int NumberOfEngines { get; set; }
        public Airplane(string regnr, string color, int weels, string model) : base(regnr, color, weels, model)
        {
        }

        public Airplane(string regnr, string color, int weels, string model, int numberrOfEngines) : base(regnr, color, weels, model)
        {
            NumberOfEngines = numberrOfEngines;
        }

        protected override string GetVehicleInfo()
        {
            return base.GetVehicleInfo() + $", Antal motorer: {NumberOfEngines}";
        }

    }
}