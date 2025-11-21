using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage1._0.Vehicles
{
    internal class Motorcycle : Vehicle
    {
        public int CylinderVolume { get; set; }
        public Motorcycle(int regnr, int color, int weels, int model) : base(regnr, color, weels, model)
        {
        }

        public Motorcycle(int regnr, int color, int weels, int model, int cylinderVolume) : base(regnr, color, weels, model)
        {
        }
    }
}
