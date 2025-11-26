using Garage1._0.Vehicles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;



namespace Garage1._0.Garage
{
    public class Garage<T> : IEnumerable<T> where T : Vehicle

    {
        public int Capacity { get;  }
        public int Count { get; private set; }

        private readonly T[] _vehicles;


        public Garage(int capacity) {
        Capacity = capacity;
            _vehicles= new T[capacity];
        }

        private int FindFreeParking () {
            throw new NotImplementedException();

        }

        public bool TryAdd(T item) {

            if (item == null) throw new ArgumentNullException(nameof(item));

            if (Count == Capacity) return false;

            for (int i = 0; i < _vehicles.Length; i++) {
                if (_vehicles[i] == null) {
                    _vehicles[i] = item;
                    Count++;
                    return true;
                        }
            }
            return false;
        }

        internal bool TryRemove(string regnr)
        {
            for (int i = 0; i < _vehicles.Length; i++)
            {
                var vehicle = _vehicles[i];

                // skipp null 
                if (vehicle is null)
                    continue;

                if (vehicle.RegistrationNumber == regnr)
                {
                    // Remove vehicle
                    _vehicles[i] = null;
                    Count--;
                    return true;
                }
            }
            // can find a vehicle with regnr
            return false;
        }


        //Not the best way to do it, problem with null and search twice
        /*
        internal bool TryRemove(string regnr)
        {
            var res = _vehicles.FirstOrDefault(item => item.RegistrationNumber == regnr);

            if (res is null) return false;
           
            
                for (int i = 0; i < _vehicles.Length; i++)
                {
                    if (_vehicles[i] == res)
                    {
                        _vehicles[i] = null;
                        Count--;
                        return true;
                    }
                }
                return false;
            }
        */


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


        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

       
        
    }
    }

