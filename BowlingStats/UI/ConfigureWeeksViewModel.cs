using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BowlingStats.Model;
using Caliburn.Micro;

namespace BowlingStats.UI
{
    public class ConfigureWeeksViewModel : PropertyChangedBase
    {
        private ObservableCollection<Week> _weeks;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureWeeksViewModel"/> class.
        /// </summary>
        /// <param name="weeks">The weeks.</param>
        public ConfigureWeeksViewModel(List<Week> weeks)
        {
            _weeks = new ObservableCollection<Week>(weeks);
        }

        /// <summary>
        /// Gets or sets the weeks.
        /// </summary>
        /// <value>
        /// The weeks.
        /// </value>
        public ObservableCollection<Week> Weeks
        {
            get { return _weeks; }
            set
            {
                _weeks = value;
                NotifyOfPropertyChange(() => Weeks);
            }
        }

        /// <summary>
        /// Determines whether this instance [can save weeks].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance [can save weeks]; otherwise, <c>false</c>.
        /// </returns>
        public bool CanSaveWeeks()
        {
            return true;
        }

        /// <summary>
        /// Saves the weeks.
        /// </summary>
        public void SaveWeeks()
        {
            EventAggregationProvider.EventAggregator.Publish(_weeks.ToList());
        }
    }
}