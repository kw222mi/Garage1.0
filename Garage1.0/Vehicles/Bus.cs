using System;

namespace Garage1._0.Vehicles
{
    /// <summary>
    /// Represents a bus, a specific type of vehicle with a number of seats.
    /// Inherits common vehicle properties from the Vehicle base class.
    /// </summary>
    internal class Bus : Vehicle
    {
        /// <summary>
        /// Gets or sets the number of passenger seats in the bus.
        /// Must be a positive integer.
        /// </summary>
        public int NumberOfSeats { get; private set; }

        /// <summary>
        /// Creates a new Bus object.
        /// The base constructor initializes shared vehicle data,
        /// and this constructor validates and sets the bus-specific property.
        /// </summary>
        /// <param name="regnr">Unique registration number.</param>
        /// <param name="color">Color of the bus.</param>
        /// <param name="wheels">Number of wheels on the bus.</param>
        /// <param name="model">Manufacturer/model name.</param>
        /// <param name="numberOfSeats">Number of passenger seats. Must be greater than zero.</param>
        public Bus(string regnr, string color, int wheels, string model, int numberOfSeats)
            : base(regnr, color, wheels, model)
        {
            if (numberOfSeats <= 0)
                throw new ArgumentOutOfRangeException(nameof(numberOfSeats),
                    "Number of seats must be greater than zero.");

            NumberOfSeats = numberOfSeats;
        }

        /// <summary>
        /// Returns a string describing the bus.
        /// Includes base vehicle info plus the bus-specific seat count.
        /// </summary>
        protected override string GetVehicleInfo()
        {
            return base.GetVehicleInfo() + $", Seats: {NumberOfSeats}";
        }
    }
}
