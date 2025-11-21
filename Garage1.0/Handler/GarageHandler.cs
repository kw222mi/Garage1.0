using Garage1._0.Garage;
using Garage1._0.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage1._0.Handler
{
    public class GarageHandler
    {
        public bool HasGarage { get; private set; } = false;
        private Garage<Vehicle>? _garage;

        public bool CreateGarage(int capacity)
        {
            if (capacity <= 0)
            {
                return false; 
            }

            _garage = new Garage<Vehicle>(capacity);
            HasGarage = true;
            return true;
        }

    }
}
