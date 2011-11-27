using System;

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

    public class Trash 
    {
        public readonly TrashType Type;
        public int Amount;

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
        public bool IsFull
        {
            get { return Capacity == 0; }
        }

        public float Consumption { get; private set; }

        public Travel Travel;

        public Truck(TrashType type, int capacity, float consumption)
        {
            HandledResource = type;
            MaxCapacity = capacity;
            Capacity = capacity;
            Consumption = consumption;
            Travel = new Travel();
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
        /// <param name="garbage">
        ///     Trash to be swallowed
        /// </param>
        /// <returns>collected trash</returns>
        public int Swallow(Trash garbage)
        {
            if (!CanHandle(garbage.Type))
                return 0; //Unhandled garbage

            var maxSwallowable = Math.Min(
                garbage.Amount,
                Capacity
            );
            Capacity -= maxSwallowable;
            return maxSwallowable;
        }

        /// <summary>
        ///     Reset the trush capacity 
        /// </summary>
        public void Reset()
        {
            Capacity = MaxCapacity;
        }
    }
}
