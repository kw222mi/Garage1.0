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

        internal bool Add<VehicleType>(
            string regNr,
            string color,
            int wheels,
            string model,
            Func<string, string, int, string, VehicleType> factoryMethod
        )
            where VehicleType : Vehicle
        {
            if (_garage is null)
                return false;

            // 1.Unique regnr?
            if (RegistrationExists(regNr))
                return false;

            // 2. Create vehicle with factory method (delegat)
            var vehicle = factoryMethod(regNr, color, wheels, model);

            // 3. Add to garage
            return _garage.TryAdd(vehicle);
        }

        public bool AddCar(string regNr, string color, int wheels, string model, string fueltype)
        {
            return Add<Car>(
        regNr,
        color,
        wheels,
        model,
        (r, c, w, m) => new Car(r, c, w, m, fueltype)
    );
        }

        internal bool AddBoat(string regNr, string color, int wheels, string model, int length)
        {
            return Add<Boat>(
                regNr,
                color,
                wheels,
                model,
                (r, c, w, m) => new Boat(r, c, w, m, length)
            );
        }

        internal bool AddMotorcycle(string regNr, string color, int wheels, string model, int cylinderVolume)
        {
            return Add<Motorcycle>(
                regNr,
                color,
                wheels,
                model,
                (r, c, w, m) => new Motorcycle(r, c, w, m, cylinderVolume)
            );
        }

        internal bool AddAirplane(string regNr, string color, int wheels, string model, int numberOfEngines)
        {
            return Add<Airplane>(
                regNr,
                color,
                wheels,
                model,
                (r, c, w, m) => new Airplane(r, c, w, m, numberOfEngines)
            );
        }

        internal bool AddBus(string regNr, string color, int wheels, string model, int numberOfSeats)
        {
            return Add<Bus>(
                regNr,
                color,
                wheels,
                model,
                (r, c, w, m) => new Bus(r, c, w, m, numberOfSeats)
            );
        }

        internal bool RemoveVehicle(string regNr)
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

        internal Dictionary<string, int> GetVehicleTypeCounts()
        {
            Dictionary<string, int> count = new Dictionary<string, int>();
            if (_garage is null)
            {
                return count;
            }

            foreach (var item in _garage)
            {
                if (item is null)
                    continue;
                else
                {
                    var type = item.GetType().Name;
                    if (count.ContainsKey(type))
                    {
                        count[type]++;
                    }

                    else
                        count.Add(type, 1);
                }
            }
            return count;

        }

        private bool RegistrationExists(string regNr)
        {
            if (_garage is null)
                return false;

            foreach (var vehicle in _garage)
            {
                if (vehicle is null)
                    continue;

                if (string.Equals(vehicle.RegistrationNumber,
                                  regNr,
                                  StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        internal Vehicle? FindByRegNr(string regNr)
        {
            if (_garage is null)
                return null;

            foreach (var item in _garage)
            {
                if (item is null)
                    continue;

                if (string.Equals(item.RegistrationNumber, regNr, StringComparison.OrdinalIgnoreCase))
                    return item;
            }

            return null; // No match
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

        new Car("ABC123", "Röd",    4, "Volvo V70", "bensin" ),
        new Car("DEF456", "Blå",    4, "Saab 9-5",    "disel"),

        new Boat("BOAT01", "Vit",   0, "Yamarin 50", 30),

        new Motorcycle("MC001", "Svart", 2, "Yamaha MT-07", 900),

        new Bus("BUS001", "Gul",   6, "Volvo 7900", 20),

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
