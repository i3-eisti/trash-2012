using System;
using System.Collections.Generic;

namespace Trash2012.Model
{

    public class Game
    {
        public static readonly DateTime Trash2012Begin = new DateTime(2012, 1, 1);

        public static readonly int PAYDAY = 1000;

        private readonly int[] _dailyTrashRange = {0, 5};
        private readonly int[] _technoParadeTrashRange = {10, 50};

        private const double RandomEventProbability = 0.1;

        public Random GameRandomness { get; private set; }

        public DateTime CurrentDate 
        {
            get { return _currentDate; }
            set
            {
                _currentDate = value;
                DateChangeEvents.OnDateChange(_currentDate);
            }
        }

        public readonly DateChangeEvent DateChangeEvents = new DateChangeEvent();

        public City City { get; private set; }
        public Company Company { get; private set; }

        public Game(
            IMapTile[][] cityMap
        ) {
            City = new City(cityMap);
            _currentDate = Trash2012Begin;
            Company = new Company();

            var newTruck1 = new Truck(TrashType.Paper, 25, 1f);
            var newTruck2 = new Truck(TrashType.Paper, 25, 1f);
            Company.Trucks.Add(newTruck1);
            Company.Trucks.Add(newTruck2);

            GameRandomness = new Random();
        }

        #region Save/ Load
        
        #endregion

        #region Game Event

        /// <summary>
        /// Apply a travel to a game city
        /// </summary>
        /// <param name="dailyTravel">truck travel</param>
        /// <param name="assignedTruck">truck</param>
        /// <returns>total collected garbage</returns>
        public int ApplyTravel(Travel dailyTravel, Truck assignedTruck)
        {
            int garbageAccumulation = 0;
            foreach (var tile in dailyTravel)
            {
                if (assignedTruck.IsFull)
                    break;
                if (!(tile is IHouseTile)) 
                    continue;

                var tilHouse = (IHouseTile) tile;
                var collectedGarbage = assignedTruck.Swallow(tilHouse.Garbage);
                garbageAccumulation += collectedGarbage;
                tilHouse.Garbage = new Trash(
                    tilHouse.Garbage.Type,
                    tilHouse.Garbage.Amount - collectedGarbage
                );
            }
            return garbageAccumulation;
        }

        public GameEvent? ApplyRandomEvent()
        {
            if(GameRandomness.NextDouble() <= RandomEventProbability)
            {
                return new GameEvent()
                {
                    Message = "Techno parade ! Gros déchêt en perspective !",
                    Effect = delegate ()
                    {
                        int w = City.Width,
                            h = City.Height;
                        var m = City.Map;
                        for (var i = h; i-- > 0; )
                        {
                            for (var j = w; j-- > 0; )
                            {
                                if (m[i][j] is IHouseTile)
                                    ((IHouseTile)m[i][j]).Garbage.Amount += GameRandomness.Next(_technoParadeTrashRange[0], _technoParadeTrashRange[1]);
                            }
                        }             
                    }
                };
            }
            return null;
        }

        public struct GameEvent
        {
            public string Message;
            public Action Effect;
        }

        /// <summary>
        /// Citizens throw new garbage everyday, that is why this method simulates
        /// </summary>
        public void ApplyDailyGarbage()
        {
            int w = City.Width,
                h = City.Height;
            var m = City.Map;
            for (var i = h; i-- > 0; )
            {
                for (var j = w; j-- > 0; )
                {
                    if (m[i][j] is IHouseTile)
                        ((IHouseTile)m[i][j]).Garbage.Amount += GameRandomness.Next(_dailyTrashRange[0], _dailyTrashRange[1]);
                }
            }
        }

        public class DateChangeEvent
        {

            public delegate void DateHandler(DateTime dateEvent);

            public readonly List<DateHandler> _dateChangeHandlers = new List<DateHandler>();

            public void OnDateChange(DateTime newDate)
            {
                foreach (var handler in _dateChangeHandlers)
                    handler(newDate);
            }

            public void Add(DateHandler handler)
            {
                _dateChangeHandlers.Add(handler);
            }

        }
        
        #endregion

        #region Hidden members

        private DateTime _currentDate;

        #endregion
    }
}
