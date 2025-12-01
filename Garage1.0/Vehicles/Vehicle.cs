using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage1._0.Vehicles
{
    /// <summary>
    /// Abstract base class for all vehicle types in the garage system.
    /// Contains shared properties and basic behavior common to every vehicle.
    /// </summary>
    public abstract class Vehicle
    {
        /// <summary>
        /// Unique identifier for the vehicle. Used for searching and removal.
        /// </summary>
        public string RegistrationNumber { get; set; }

        /// <summary>
        /// The color of the vehicle.
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// The number of wheels the vehicle has. Can be zero (e.g., a boat).
        /// </summary>
        public int Wheels { get; set; }

        /// <summary>
        /// The model name or designation of the vehicle.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Constructor initializing the shared vehicle properties.
        /// </summary>
        public Vehicle(string regnr, string color, int weels, string model)
        {
            RegistrationNumber = regnr;
            Color = color;
            Wheels = weels;
            Model = model;
        }

        /// <summary>
        /// Overrides ToString to provide a readable summary of the vehicle.
        /// Delegates formatting to GetVehicleInfo, which derived classes may extend.
        /// </summary>
        public override string ToString()
        {
            return GetVehicleInfo();
        }

        /// <summary>
        /// Returns a formatted string describing the vehicle.
        /// Marked as virtual so subclasses can append additional information.
        /// </summary>
        protected virtual string GetVehicleInfo()
        {
            return $" Regnummer: {RegistrationNumber} Färg: {Color} Antal hjul: {Wheels} Modell: {Model} ";
        }
    }
}
