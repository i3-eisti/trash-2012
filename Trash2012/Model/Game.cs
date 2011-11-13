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
            private set
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

            dateChangeHandlers.Add( PayDay_Handler );
        }

        #region Save/ Load
        
        #endregion

        #region Game Event

        #region Date changed handler (e.g payday)

        private delegate void DateHandler(DateTime dateEvent);

        private List<DateHandler> dateChangeHandlers = new List<DateHandler>();

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
