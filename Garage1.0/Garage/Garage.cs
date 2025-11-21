using Garage1._0.Vehicles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garage1._0.Garage
{
    public class Garage<T> : IEnumerable<T>
    {
        public int Capacity { get;  }
        public int Count { get; private set; }

        private readonly T[] _vehicles;


        public Garage(int capacity) {
        Capacity = capacity;
            _vehicles= new T[capacity];
        }

        private int FindFreeParking () { 
           throw new NotImplementedException ();
        }

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

