using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BowlingStats.Model;
using Caliburn.Micro;

namespace BowlingStats.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigureTeamViewModel : PropertyChangedBase
    {
        private ObservableCollection<Team> _teams;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureTeamViewModel"/> class.
        /// </summary>
        /// <param name="teams">The teams.</param>
        public ConfigureTeamViewModel(List<Team> teams)
        {
            if (teams.Count == 0)
            {
                teams.Add(new Team("Sub"));
            }
            _teams = new ObservableCollection<Team>(teams);
        }

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

        public void DeleteTeamPoints()
        {
            foreach (Team team in _teams)
            {
                team.WeeklyPoints = new List<WeeklyPoint>();
            }

            EventAggregationProvider.EventAggregator.Publish(_teams.ToList());
        }

        /// <summary>
        /// Determines whether this instance [can save teams].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance [can save teams]; otherwise, <c>false</c>.
        /// </returns>
        public bool CanSaveTeams()
        {
            return true;
        }

        /// <summary>
        /// Saves the teams.
        /// </summary>
        public void SaveTeams()
        {
            List<Team> teams = _teams.ToList();
            teams.Sort((t1, t2) => t1.Name.CompareTo(t2.Name));
            EventAggregationProvider.EventAggregator.Publish(teams);
        }
    }
}