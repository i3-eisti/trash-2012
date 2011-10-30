using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Trash2012.Model
{
    // Summary:
    //      Different type of trash available.
    [Flags]
    public enum TrashType 
    {
        Paper,
        Plastic,
        Glass,
        Metal,
        Organic = Paper | Plastic
    }

    public struct Trash 
    {
        public readonly TrashType Type;
        public readonly int Amount;

        public Trash(TrashType t, int a)
        {
            this.Type = t;
            this.Amount = a;
        }

    }

    /// Summary:
    ///      Representation of a garbage truck which reclaim trash.
    ///      This truck can handle only a certain type of 'TrashType';
    ///      His move cost is defined by its 'Comsumption'
    public class Truck
    {
        public TrashType HandledResource { get; private set; }
        public int Capacity { get; private set; }
        public readonly int MaxCapacity;

        public float Consumption { get; private set; }

        public Truck(TrashType type, int capacity, float consumption)
        {
            HandledResource = type;
            MaxCapacity = capacity;
            Capacity = capacity;
            Consumption = consumption;
        }

        /// Summary:
        ///      Check if a garbage's type can be handled by this truck
        public bool CanHandle(TrashType ttype)
        {
            return (HandledResource & ttype) == ttype;
        }

        /// <summary>
        ///     Swallow an amount of trash, 
        ///     throws an exception if the garbage type cannot be handled 
        ///     or if there is no enough place to swallow the gabage.
        /// </summary>
        /// <param name="trash">
        ///     Trash to be swallowed
        /// </param>
        public void Swallow(Trash trash)
        {
            if (!CanHandle(trash.Type))
                throw new ArgumentException("Unhandled garbage type : " + trash.Type);
            if (Capacity < trash.Amount)
                throw new ArgumentException(
                    "Not enough place remaining : garbage amount("+trash.Amount+") - remaining place("+Capacity+")");
            Capacity -= trash.Amount;
        }

        /// <summary>
        ///     Reset the trush capacity 
        /// </summary>
        public void Reset()
        {
            if (MaxCapacity != Capacity)
                Capacity = MaxCapacity;
        }
    }
}
