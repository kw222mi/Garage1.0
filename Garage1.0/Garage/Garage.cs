using Garage1._0.Vehicles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Garage1._0.Garage
{
    /// <summary>
    /// Represents a garage that can store a fixed number of vehicles.
    /// The class is generic and can store any type derived from Vehicle.
    /// </summary>
    /// <typeparam name="T">
    /// The type of vehicle this garage handles. Must inherit from Vehicle.
    /// </typeparam>
    public class Garage<T> : IEnumerable<T> where T : Vehicle
    {
        /// <summary>
        /// Gets the maximum number of vehicles the garage can hold.
        /// </summary>
        public int Capacity { get; }

        /// <summary>
        /// Gets the current number of parked vehicles.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Internal array representing the parking slots.
        /// A null entry means that the slot is available.
        /// </summary>
        private readonly T?[] _vehicles;

        /// <summary>
        /// Creates a new garage instance with the specified capacity.
        /// </summary>
        /// <param name="capacity">The maximum number of vehicles allowed in the garage.</param>
        public Garage(int capacity)
        {
            Capacity = capacity;
            _vehicles = new T?[capacity]; // Using an array to model fixed parking slots
        }

        /// <summary>
        /// Attempts to add a new vehicle to the first available slot.
        /// Returns true if the vehicle was successfully added, otherwise false.
        /// </summary>
        /// <param name="item">The vehicle to add.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the supplied vehicle is null.
        /// </exception>
        public bool TryAdd(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            // If Count == Capacity, no more vehicles can be added.
            if (Count == Capacity) return false;

            // Find the first free (null) slot in the array.
            for (int i = 0; i < _vehicles.Length; i++)
            {
                if (_vehicles[i] == null)
                {
                    _vehicles[i] = item;
                    Count++;
                    return true;
                }
            }

            // Should not occur if Count is correctly maintained.
            return false;
        }

        /// <summary>
        /// Attempts to remove a vehicle based on its registration number.
        /// Returns true if the vehicle was found and removed, otherwise false.
        /// </summary>
        /// <param name="regnr">The registration number of the vehicle to remove.</param>
        public bool TryRemove(string regnr)
        {
            for (int i = 0; i < _vehicles.Length; i++)
            {
                var vehicle = _vehicles[i];

                // Skip empty slots
                if (vehicle is null)
                    continue;

                // Compare registration numbers (case-insensitive)
                if (string.Equals(vehicle.RegistrationNumber, regnr, StringComparison.OrdinalIgnoreCase))
                {
                    // Remove vehicle by clearing the slot
                    _vehicles[i] = null;
                    Count--;
                    return true;
                }
            }

            // No vehicle matched the provided registration number
            return false;
        }


        /// <summary>
        /// Returns an enumerator that iterates over all non-null vehicles in the garage.
        /// Enables the use of foreach directly on the Garage&lt;T&gt; instance.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in _vehicles)
            {
                if (item is not null)
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Non-generic enumerator implementation required by IEnumerable.
        /// Delegates to the generic GetEnumerator().
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
