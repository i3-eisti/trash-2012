using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trash2012.Engine;

namespace Trash2012.Model
{

    public class Game
    {
        public static readonly DateTime TRASH2012_BEGIN = new DateTime(2012, 1, 1);

        public static readonly int PAYDAY = 1000;

        public DateTime CurrentDate 
        {
            get { return _currentDate; }
            private set
            {
                _currentDate = value;
                OnDateChange(_currentDate);
            }
        }

        public City City { get; private set; }
        public Company Player { get; private set; }

        public Game(
            IMapTile[][] cityMap
        ) {
            City = new City(cityMap);
            _currentDate = TRASH2012_BEGIN;
            Player = new Company();

            dateChangeHandlers.Add( PayDay_Handler );
        }

        #region Date changed handler (e.g payday)

        private List<Action<DateTime>> dateChangeHandlers = new List<Action<DateTime>>();

        private void OnDateChange(DateTime newDate)
        {
            foreach (Action<DateTime> handler in dateChangeHandlers)
                handler(newDate);
        }

        private void PayDay_Handler(DateTime newDate)
        {
            //every month, receive payday
            if (newDate.Day == 1)
            {
                //Player.Gold += PAYDAY;
            }
        }
        
        #endregion

        #region Hidden members

        private DateTime _currentDate;

        #endregion
    }
}
