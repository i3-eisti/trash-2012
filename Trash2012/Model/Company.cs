using System;
using System.Collections.Generic;


namespace Trash2012.Model
{
    public class Company
    {
        public int Gold { get; set; }
        public List<Truck> Trucks { get; set; }

        public Company(int initialAmount)
        {
            Gold = initialAmount;
            Trucks = new List<Truck>(10);
        }

    }
}
