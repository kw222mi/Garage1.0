using System;

namespace Garage1._0.Vehicles
{
    /// <summary>
    /// Represents an airplane, a type of vehicle equipped with one or more engines.
    /// Inherits shared vehicle properties from the Vehicle base class.
    /// </summary>
    internal class Airplane : Vehicle
    {
        /// <summary>
        /// Gets the number of engines on the airplane.
        /// Must be a positive integer.
        /// </summary>
        public int NumberOfEngines { get; private set; }

        /// <summary>
        /// Creates a new Airplane instance.
        /// Initializes common vehicle data through the base constructor
        /// and validates and sets the airplane-specific engine count.
        /// </summary>
        /// <param name="regnr">Unique registration number of the aircraft.</param>
        /// <param name="color">Color of the airplane body.</param>
        /// <param name="wheels">Number of wheels. Varies depending on model and landing gear.</param>
        /// <param name="model">Model or manufacturer name (e.g., Boeing 737).</param>
        /// <param name="numberOfEngines">Total number of engines. Must be greater than zero.</param>
        public Airplane(string regnr, string color, int wheels, string model, int numberOfEngines)
            : base(regnr, color, wheels, model)
        {
            if (numberOfEngines <= 0)
                throw new ArgumentOutOfRangeException(nameof(numberOfEngines),
                    "An airplane must have at least one engine.");

            NumberOfEngines = numberOfEngines;
        }

        /// <summary>
        /// Returns a string describing the airplane.
        /// Includes base vehicle information along with the number of engines.
        /// </summary>
        protected override string GetVehicleInfo()
        {
            return base.GetVehicleInfo() + $", Engines: {NumberOfEngines}";
        }
    }
}
