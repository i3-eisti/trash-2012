using System;
using System.Collections.Generic;

namespace Trash2012.Model
{

    public class Game
    {
        public static readonly DateTime TRASH2012_BEGIN = new DateTime(2012, 1, 1);

        public static readonly int PAYDAY = 1000;

        public DateTime CurrentDate 
        {
            get { return _currentDate; }
            set
            {
                _currentDate = value;
                OnDateChange(_currentDate);
            }
        }

        public City City { get; private set; }
        public Company Company { get; private set; }

        public Game(
            IMapTile[][] cityMap
        ) {
            City = new City(cityMap);
            _currentDate = TRASH2012_BEGIN;
            Company = new Company();

            var newTruck = new Truck(TrashType.Paper, 25, 1f);
            Company.Trucks.Add(newTruck);

            dateChangeHandlers.Add( PayDay_Handler );
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

        /// <summary>
        /// Citizens throw new garbage everyday, that is why this method simulates
        /// </summary>
        public void ApplyDailyGarbage()
        {
            throw new NotImplementedException();
        }

        #region Date changed handler (e.g payday)

        public delegate void DateHandler(DateTime dateEvent);

        public List<DateHandler> dateChangeHandlers = new List<DateHandler>();

        private void OnDateChange(DateTime newDate)
        {
            foreach (DateHandler handler in dateChangeHandlers)
                handler(newDate);
        }

        private void PayDay_Handler(DateTime newDate)
        {
            //every month, receive payday
            if (newDate.Day == 1)
            {
                Company.Gold += PAYDAY;
            }
        }
        
        #endregion

        #endregion

        #region Hidden members

        private DateTime _currentDate;

        #endregion
    }
}
