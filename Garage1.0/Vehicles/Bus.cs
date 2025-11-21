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
        public Bus(int regnr, int color, int weels, int model) : base(regnr, color, weels, model)
        {
        }

        public Bus(int regnr, int color, int weels, int model, int NumberOfSeats) : base(regnr, color, weels, model)
        {
        }
    }
}
