using System;

namespace Garage1._0.Vehicles
{
    /// <summary>
    /// Represents a boat, a vehicle without wheels but with a length measured in feet.
    /// Inherits shared vehicle properties from the Vehicle base class.
    /// </summary>
    internal class Boat : Vehicle
    {
        /// <summary>
        /// Gets the length of the boat in feet.
        /// Must be a positive value.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Creates a new Boat instance.
        /// Initializes shared vehicle data via the base constructor
        /// and validates the boat-specific length value.
        /// </summary>
        /// <param name="regnr">Unique registration number of the boat.</param>
        /// <param name="color">Boat hull color.</param>
        /// <param name="wheels">
        /// Number of wheels (usually 0 for boats, but kept for consistency with the Vehicle design).
        /// </param>
        /// <param name="model">Model or manufacturer name.</param>
        /// <param name="length">Length of the boat in feet. Must be greater than zero.</param>
        public Boat(string regnr, string color, int wheels, string model, int length)
            : base(regnr, color, wheels, model)
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException(nameof(length),
                    "Boat length must be greater than zero.");

            Length = length;
        }

        /// <summary>
        /// Returns a description of the boat including base vehicle info
        /// plus the boat's length in feet.
        /// </summary>
        protected override string GetVehicleInfo()
        {
            return base.GetVehicleInfo() + $", Length (ft): {Length}";
        }
    }
}
