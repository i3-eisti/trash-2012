using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trash2012.Model
{
    // Summary:
    //      Different type of trash available.
    //      For independant type of trash, use 1 << 'X' syntax
    //      For mixed type of trash, use 'X' | 'Y' syntax
    [Flags]
    public enum TrashType 
    {
        Paper,
        Plastic,
        Glass,
        Metal,
        Organic = Paper | Plastic
    }

    // Summary:
    //      Representation of a garbage truck which reclaim trash.
    //      This truck can handle only a certain type of 'TrashType';
    //      His move cost is defined by its 'Comsumption'
    public class Truck
    {
        public TrashType HandledResource { get; private set; }
        public int Capacity { get; private set; }

        public float Consumption { get; private set; }

        public Truck(TrashType type, int capacity, float consumption)
        {
            HandledResource = type;
            Capacity = capacity;
            Consumption = consumption;
        }

        public bool CanHandle(TrashType ttype)
        {
            return (HandledResource & ttype) == ttype;
        }
    }
}
