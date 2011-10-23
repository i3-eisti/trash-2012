using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trash2012.Model
{
    public class Company
    {
        public Resource<int> Gold { get; set; }
        public List<Truck> Trucks { get; set; }

        public Company()
        {
            Gold = new Resource<int>("Or", 0, 10000, 1000);
            Trucks = new List<Truck>(10);
        }

        /*
         * TODO
         * Implements a actions' history.
         * In order to compute performance dashboard, 
         * we need to iterate over all actions done.
         */
    }

    public class Resource<T>
    {
        public string Name { get; private set; }
        public T Minimum { get; private set; }
        public T Maximum { get; private set; }
        public T Current { get; set; }

        public Resource(string name, T min, T max, T value)
        {
            // Make 'T' implements ICompare of smth like to enable comparing
            //if (value < min || value > max)
            //    throw new ArgumentException("Value is out of range");

            Name = name;
            Minimum = min;
            Maximum = max;
            Current = value;
        }

    }
}
