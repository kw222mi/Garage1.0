using Garage1._0.Garage;
using Garage1._0.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Generic;

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

            if (_garage == null) throw new ArgumentNullException(nameof(_garage));
            bool isAdded = _garage.TryAdd(vehicle);
            return isAdded;
        }

        public bool CreateAndAddCar(string regnr, string color, int weels, string model, string fueltype)
        {
            if (_garage is null) return false;

            var car = new Car(regnr, color, weels, model, fueltype);
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

        internal IEnumerable<Vehicle> FindByRegNr(string regNr)
        {
            var results = new List<Vehicle>();
            if (_garage is null)
                return Enumerable.Empty<Vehicle>();

            foreach (var item in _garage)
            {
                // skipp null 
                if (item is null)
                    continue;

                if (string.Equals(item.RegistrationNumber, regNr, StringComparison.OrdinalIgnoreCase))

                    results.Add(item);
            }

            return results;
        }

        internal IEnumerable<Vehicle> AdvancedSearch(string? vehicleType, string? regNr, string? color, int? wheels, string? model)
        {
            // If null return empty list
            if (_garage is null)
                return Enumerable.Empty<Vehicle>();

            else
            {
                var results = new List<Vehicle>();
                foreach (var item in _garage)
                {
                    if (item is null)
                        continue;

                    bool match = true;

                    if (!string.IsNullOrWhiteSpace(vehicleType))
                    {
                        var typeName = item.GetType().Name;
                        if (!string.Equals(typeName, vehicleType, StringComparison.OrdinalIgnoreCase))
                        {
                            match = false;
                        }
                    }
                    if (match && !string.IsNullOrWhiteSpace(regNr))
                    {
                        if (!string.Equals(item.RegistrationNumber, regNr, StringComparison.OrdinalIgnoreCase))
                        {
                            match = false;
                        }
                    }

                    if (match && !string.IsNullOrWhiteSpace(color))
                    {
                        if (!string.Equals(item.Color, color, StringComparison.OrdinalIgnoreCase))
                        {
                            match = false;
                        }
                    }


                    if (match && wheels.HasValue)
                    {
                        if (item.Wheels != wheels.Value)
                        {
                            match = false;
                        }
                    }


                    if (match && !string.IsNullOrWhiteSpace(model))
                    {
                        if (!string.Equals(item.Model, model, StringComparison.OrdinalIgnoreCase))
                        {
                            match = false;
                        }
                    }

                    if (match)
                    {
                        results.Add(item);
                    }
                }

                return results;
            }

        }


internal void SeedGarage()
    {
        if (_garage is null)
            return;

        var sampleVehicles = new List<Vehicle>
    {
        // Bil
        new Car("ABC123", "Röd",    4, "Volvo V70", "bensin" ),
        new Car("DEF456", "Blå",    4, "Saab 9-5",    "disel"),

        // Båt
        new Boat("BOAT01", "Vit",   0, "Yamarin 50", 30),

        // Motorcykel
        new Motorcycle("MC001", "Svart", 2, "Yamaha MT-07", 900),

        // Buss
        new Bus("BUS001", "Gul",   6, "Volvo 7900", 20),

        // Flygplan
        new Airplane("PLN001", "Vit", 3, "Boeing 737", 4)
    };

        foreach (var v in sampleVehicles)
        {
            // Try to add, break if full
            if (!_garage.TryAdd(v))
                break;
        }
    }




}
}


