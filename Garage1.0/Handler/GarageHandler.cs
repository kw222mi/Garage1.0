using Garage1._0.Garage;
using Garage1._0.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
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

        public bool AddVehicle(Vehicle vehicle)
        {

            if(_garage  == null) throw new ArgumentNullException(nameof(_garage));
            bool isAdded = _garage.TryAdd(vehicle);
            return isAdded;
        }

        public bool CreateAndAddCar(string regnr, string color, int weels, string model, string fueltype)
        {
            if (_garage is null) return false;

            var car = new Car( regnr, color, weels, model, fueltype);
            return _garage.TryAdd(car);
        }

        internal bool RemoveCar(string regNr)
        {
            if (_garage is null) return false;

            else
                return _garage.TryRemove(regNr);

        }

        public IEnumerable<Vehicle> GetAllVehicles()
        {
            // If null return empty list
            if (_garage is null)
            {
                return Enumerable.Empty<Vehicle>();
            }

            return _garage;
        }

        
    }
}

