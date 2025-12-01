using System;

namespace Garage1._0.Vehicles
{
    /// <summary>
    /// Represents a motorcycle in the garage.
    /// Extends Vehicle with motorcycle-specific data such as cylinder volume.
    /// </summary>
    internal class Motorcycle : Vehicle
    {
        /// <summary>
        /// Gets the engine's cylinder volume (in cubic centimeters).
        /// Higher values typically indicate higher performance.
        /// </summary>
        public int CylinderVolume { get; private set; }

        /// <summary>
        /// Creates a new Motorcycle instance.
        /// Initializes shared vehicle properties via the base constructor,
        /// and validates and sets the motorcycle-specific engine displacement.
        /// </summary>
        /// <param name="regnr">Unique registration number of the motorcycle.</param>
        /// <param name="color">Motorcycle color.</param>
        /// <param name="wheels">Number of wheels (usually 2).</param>
        /// <param name="model">Model or manufacturer name.</param>
        /// <param name="cylinderVolume">Engine displacement in cubic centimeters (cc). Must be greater than zero.</param>
        public Motorcycle(string regnr, string color, int wheels, string model, int cylinderVolume)
            : base(regnr, color, wheels, model)
        {
            if (cylinderVolume <= 0)
                throw new ArgumentOutOfRangeException(nameof(cylinderVolume),
                    "Cylinder volume must be greater than zero.");

            CylinderVolume = cylinderVolume;
        }

        /// <summary>
        /// Returns full information about the motorcycle.
        /// Extends the base vehicle info by appending engine data.
        /// </summary>
        protected override string GetVehicleInfo()
        {
            return base.GetVehicleInfo() + $", Cylinder volume: {CylinderVolume} cc";
        }
    }
}
