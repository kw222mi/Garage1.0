using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage1._0.Vehicles
{
    internal class Boat : Vehicle
    {
        public int Lenght { get; set; }
        public Boat(int regnr, int color, int weels, int model) : base(regnr, color, weels, model)
        {
        }

        public Boat(int regnr, int color, int weels, int model, int Lenght) : base(regnr, color, weels, model)
        {
        }
    }
}
