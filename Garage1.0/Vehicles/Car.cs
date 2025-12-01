using System;

namespace Garage1._0.Vehicles
{
    /// <summary>
    /// Represents a car in the garage.
    /// Extends Vehicle with car-specific information such as fuel type.
    /// </summary>
    internal class Car : Vehicle
    {
        /// <summary>
        /// Gets the type of fuel the car uses
        /// (e.g., gasoline, diesel, electric).
        /// </summary>
        public string FuelType { get; private set; }

        /// <summary>
        /// Creates a new Car instance.
        /// Initializes shared vehicle properties via the base constructor
        /// and validates and sets the fuel type specific to the car.
        /// </summary>
        /// <param name="regnr">Unique registration number of the car.</param>
        /// <param name="color">Color of the car.</param>
        /// <param name="wheels">Number of wheels (usually 4).</param>
        /// <param name="model">Model or manufacturer name.</param>
        /// <param name="fuelType">Fuel type of the car. Must not be null or empty.</param>
        public Car(string regnr, string color, int wheels, string model, string fuelType)
            : base(regnr, color, wheels, model)
        {
            if (string.IsNullOrWhiteSpace(fuelType))
                throw new ArgumentNullException(nameof(fuelType),
                    "Fuel type must be provided.");

            FuelType = fuelType;
        }

        /// <summary>
        /// Returns detailed information about the car.
        /// Extends the base vehicle info by appending its fuel type.
        /// </summary>
        protected override string GetVehicleInfo()
        {
            return base.GetVehicleInfo() + $", Fuel type: {FuelType}";
        }
    }
}
