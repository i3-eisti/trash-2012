using System;
using System.Collections.Generic;


namespace Trash2012.Model
{
    public class Company
    {
        public Resource Gold { get; set; }
        public List<Truck> Trucks { get; set; }

        public Company()
        {
            Gold = new Resource("Or", 0, 10000, 1000);
            Trucks = new List<Truck>(10);
        }

        /*
         * TODO
         * Implements a actions' history.
         * In order to compute performance dashboard, 
         * we need to iterate over all actions done.
         */
    }

    public class Resource
    {
        public readonly string Name;
        public readonly int Minimum;
        public readonly int Maximum;
        public int Current { get; set; }

        public Resource(string name, int min, int max, int value)
        {
            // Make 'T' implements ICompare of smth like to enable comparing
            //if (value < min || value > max)
            //    throw new ArgumentException("Value is out of range");

            Name = name;
            Minimum = min;
            Maximum = max;
            Current = value;
        }

        public bool Add(int quantity)
        {
            if (quantity < 0)
                throw new ArgumentException("Quantity must be positive");
            if (quantity + Current > Maximum)
            {
                Current += quantity;
                return true;
            }
            else
            {
                return false;
            }
        }

        #region Syntatic Sugar Helpers

        public static implicit operator Resource(int value)
        {
            return new Resource("anything", Int32.MinValue, Int32.MaxValue, value);
        }

        public static Resource operator+(Resource a, Resource b)
        {
            a.Add(b.Current);
            return a;
        }

        #endregion

    }
}
