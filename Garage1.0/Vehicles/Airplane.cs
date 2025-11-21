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
        public Airplane(int regnr, int color, int weels, int model) : base(regnr, color, weels, model)
        {
        }

        public Airplane(int regnr, int color, int weels, int model, int numberrOfEngines) : base(regnr, color, weels, model)
        {

        }

        protected override string GetVehicleInfo()
        {
            return base.GetVehicleInfo() + $", NumberOfEngines: {NumberOfEngines}";
        }

    }
}