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

    }

    public class Resource
    {
        public readonly string Name;
        public readonly int Minimum;
        public readonly int Maximum;
        public int Current { get; set; }

        public Resource(string name, int min, int max, int value)
        {
            Name = name;
            Minimum = min;
            Maximum = max;
            Current = value;
        }

        public void Add(int quantity)
        {
            if (quantity + Current > Maximum) Current = Maximum;
            else if(quantity + Current < Minimum) Current = Minimum;
            else  Current += quantity;
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

        public static Resource operator-(Resource a, Resource b)
        {
            a.Add(-b.Current);
            return a;
        }

        #endregion

    }
}
