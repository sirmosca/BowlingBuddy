using System.Collections.ObjectModel;
using BowlingStats.Model;
using Caliburn.Micro;

namespace BowlingStats.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class ScheduleItemViewModel : PropertyChangedBase
    {
        private Week _selectedWeek;
        private Team _teamOne;
        private Team _teamTwo;
        private ObservableCollection<Team> _teams;
        private ObservableCollection<Week> _weeks;

        /// <summary>
        /// Gets or sets the selected team one.
        /// </summary>
        /// <value>
        /// The selected team one.
        /// </value>
        public Team SelectedTeamOne
        {
            get { return _teamOne; }
            set
            {
                _teamOne = value;
                NotifyOfPropertyChange(() => SelectedTeamOne);
            }
        }

        /// <summary>
        /// Gets or sets the selected team two.
        /// </summary>
        /// <value>
        /// The selected team two.
        /// </value>
        public Team SelectedTeamTwo
        {
            get { return _teamTwo; }
            set
            {
                _teamTwo = value;
                NotifyOfPropertyChange(() => SelectedTeamTwo);
            }
        }
        /// <summary>
        /// Gets or sets the selected week.
        /// </summary>
        /// <value>
        /// The selected week.
        /// </value>
        public Week SelectedWeek
        {
            get { return _selectedWeek; }
            set
            {
                _selectedWeek = value;
                NotifyOfPropertyChange(() => SelectedWeek);
            }
        }

        /// <summary>
        /// Gets or sets the name of the team one.
        /// </summary>
        /// <value>
        /// The name of the team one.
        /// </value>
        public string TeamOneName { get; set; }

        /// <summary>
        /// Gets or sets the name of the team two.
        /// </summary>
        /// <value>
        /// The name of the team two.
        /// </value>
        public string TeamTwoName { get; set; }

        /// <summary>
        /// Gets or sets the teams.
        /// </summary>
        /// <value>
        /// The teams.
        /// </value>
        public ObservableCollection<Team> Teams
        {
            get { return _teams; }
            set
            {
                _teams = value;
                NotifyOfPropertyChange(() => Teams);
            }
        }
        /// <summary>
        /// Gets or sets the name of the week.
        /// </summary>
        /// <value>
        /// The name of the week.
        /// </value>
        public string WeekName { get; set; }

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
    }
}