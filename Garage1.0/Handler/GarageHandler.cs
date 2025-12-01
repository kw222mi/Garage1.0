using Garage1._0.Garage;
using Garage1._0.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Garage1._0.Handler
{
    /// <summary>
    /// Handles all business logic related to the garage:
    /// creating the garage, adding/removing vehicles, searches and statistics.
    /// The UI layer communicates with this class instead of talking to Garage&lt;Vehicle&gt; directly.
    /// </summary>
    public class GarageHandler
    {
        /// <summary>
        /// Indicates whether a garage has been created or not.
        /// </summary>
        public bool HasGarage { get; private set; } = false;

        /// <summary>
        /// Reference to the current garage instance.
        /// Null means no garage has been created.
        /// </summary>
        private Garage<Vehicle>? _garage;

        /// <summary>
        /// Creates a new garage with the specified capacity.
        /// </summary>
        /// <param name="capacity">Number of parking slots in the garage.</param>
        /// <returns>True if the garage was created, false if capacity was invalid.</returns>
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

        /// <summary>
        /// Generic helper method for creating and adding a specific vehicle type.
        /// Uses a factory method delegate to construct the concrete vehicle instance.
        /// </summary>
        /// <typeparam name="VehicleType">Concrete vehicle type (e.g. Car, Boat, Bus).</typeparam>
        /// <param name="regNr">Registration number.</param>
        /// <param name="color">Vehicle color.</param>
        /// <param name="wheels">Number of wheels.</param>
        /// <param name="model">Model name.</param>
        /// <param name="factoryMethod">
        /// Delegate that constructs the correct vehicle instance based on the shared properties.
        /// </param>
        /// <returns>True if the vehicle was added, false otherwise.</returns>
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

            // 1. Validate unique registration number
            if (RegistrationExists(regNr))
                return false;

            // 2. Create vehicle using the supplied factory method (delegate)
            var vehicle = factoryMethod(regNr, color, wheels, model);

            // 3. Add vehicle to the garage
            return _garage.TryAdd(vehicle);
        }

        /// <summary>
        /// Creates and adds a car with a specific fuel type.
        /// </summary>
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

        /// <summary>
        /// Creates and adds a boat with a specific length.
        /// </summary>
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

        /// <summary>
        /// Creates and adds a motorcycle with a specific cylinder volume.
        /// </summary>
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

        /// <summary>
        /// Creates and adds an airplane with a specific number of engines.
        /// </summary>
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

        /// <summary>
        /// Creates and adds a bus with a specific number of seats.
        /// </summary>
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

        /// <summary>
        /// Attempts to remove a vehicle from the garage, based on registration number.
        /// Delegates to the garage's TryRemove method.
        /// </summary>
        internal bool RemoveVehicle(string regNr)
        {
            if (_garage is null) return false;

            return _garage.TryRemove(regNr);
        }

        /// <summary>
        /// Returns all vehicles in the current garage.
        /// If no garage exists, an empty sequence is returned.
        /// </summary>
        public IEnumerable<Vehicle> GetAllVehicles()
        {
            if (_garage is null)
            {
                return Enumerable.Empty<Vehicle>();
            }

            // Garage&lt;Vehicle&gt; implements IEnumerable&lt;Vehicle&gt;, so it can be returned directly
            return _garage;
        }

        /// <summary>
        /// Computes how many vehicles exist per vehicle type (Car, Boat, etc.).
        /// The dictionary key is the type name, the value is the count.
        /// </summary>
        internal Dictionary<string, int> GetVehicleTypeCounts()
        {
            var count = new Dictionary<string, int>();

            if (_garage is null)
            {
                return count;
            }

            foreach (var item in _garage)
            {
                if (item is null)
                    continue;

                var type = item.GetType().Name;
                if (count.ContainsKey(type))
                {
                    count[type]++;
                }
                else
                {
                    count.Add(type, 1);
                }
            }

            return count;
        }

        /// <summary>
        /// Checks whether a registration number is already used by any vehicle in the garage.
        /// Comparison is case-insensitive.
        /// </summary>
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

        /// <summary>
        /// Finds a single vehicle by registration number.
        /// Returns the vehicle if found, otherwise null.
        /// </summary>
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

        /// <summary>
        /// Performs an advanced search across multiple optional filters:
        /// vehicle type, registration number, color, number of wheels and model.
        /// Null or empty parameters are treated as "no filter" for that field.
        /// </summary>
        internal IEnumerable<Vehicle> AdvancedSearch(
            string? vehicleType,
            string? regNr,
            string? color,
            int? wheels,
            string? model)
        {
            if (_garage is null)
                return Enumerable.Empty<Vehicle>();

            var results = new List<Vehicle>();

            foreach (var item in _garage)
            {
                if (item is null)
                    continue;

                bool match = true;

                // Filter on runtime type (Car, Boat, etc.)
                if (!string.IsNullOrWhiteSpace(vehicleType))
                {
                    var typeName = item.GetType().Name;
                    if (!string.Equals(typeName, vehicleType, StringComparison.OrdinalIgnoreCase))
                    {
                        match = false;
                    }
                }

                // Filter on registration number
                if (match && !string.IsNullOrWhiteSpace(regNr))
                {
                    if (!string.Equals(item.RegistrationNumber, regNr, StringComparison.OrdinalIgnoreCase))
                    {
                        match = false;
                    }
                }

                // Filter on color
                if (match && !string.IsNullOrWhiteSpace(color))
                {
                    if (!string.Equals(item.Color, color, StringComparison.OrdinalIgnoreCase))
                    {
                        match = false;
                    }
                }

                // Filter on number of wheels
                if (match && wheels.HasValue)
                {
                    if (item.Wheels != wheels.Value)
                    {
                        match = false;
                    }
                }

                // Filter on model
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

        /// <summary>
        /// Seeds the garage with a predefined set of sample vehicles.
        /// Useful for demo and testing.
        /// </summary>
        internal void SeedGarage()
        {
            if (_garage is null)
                return;

            var sampleVehicles = new List<Vehicle>
            {
                new Car("ABC123",  "Röd",   4, "Volvo V70",  "bensin"),
                new Car("DEF456",  "Blå",   4, "Saab 9-5",   "disel"),
                new Boat("BOAT01", "Vit",   0, "Yamarin 50", 30),
                new Motorcycle("MC001", "Svart", 2, "Yamaha MT-07", 900),
                new Bus("BUS001",  "Gul",   6, "Volvo 7900", 20),
                new Airplane("PLN001", "Vit", 3, "Boeing 737", 4)
            };

            foreach (var v in sampleVehicles)
            {
                // Stop seeding if the garage becomes full
                if (!_garage.TryAdd(v))
                    break;
            }
        }
    }
}
