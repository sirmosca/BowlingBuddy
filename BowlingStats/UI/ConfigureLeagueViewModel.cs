using System;
using System.Collections.Generic;
using BowlingStats.Model;
using Caliburn.Micro;

namespace BowlingStats.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigureLeagueViewModel : PropertyChangedBase, IHandle<List<Team>>, IHandle<List<Week>>, IHandle<List<WeeklyMatchup>>, IHandle<List<Player>>, IHandle<List<WeeklyScore>>
    {
        private League _league;
        private string _leagueName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureLeagueViewModel"/> class.
        /// </summary>
        /// <param name="players">The players.</param>
        public ConfigureLeagueViewModel(List<Player> players)
        {
            _league = new League
                { Players = players, Matchups = new List<WeeklyMatchup>(), Teams = new List<Team>(), WeeklyScores = new List<WeeklyScore>(), Weeks = new List<Week>() };
            EventAggregationProvider.EventAggregator.Subscribe(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureLeagueViewModel"/> class.
        /// </summary>
        /// <param name="league">The league.</param>
        public ConfigureLeagueViewModel(League league)
        {
            _league = league;
            _leagueName = league.Name;
            EventAggregationProvider.EventAggregator.Subscribe(this);
        }

        /// <summary>
        /// Gets or sets the name of the league.
        /// </summary>
        /// <value>
        /// The name of the league.
        /// </value>
        public string LeagueName
        {
            get { return _leagueName; }
            set
            {
                _leagueName = value;
                _league.Name = _leagueName;
                NotifyOfPropertyChange(() => LeagueName);
            }
        }

        /// <summary>
        /// Determines whether this instance [can Configure players].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance [can Configure players]; otherwise, <c>false</c>.
        /// </returns>
        public bool CanConfigurePlayers()
        {
            return true;
        }

        /// <summary>
        /// Determines whether this instance [can configure schedule].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance [can configure schedule]; otherwise, <c>false</c>.
        /// </returns>
        public bool CanConfigureSchedule()
        {
            return true;
        }

        /// <summary>
        /// Determines whether this instance [can Configure teams].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance [can Configure teams]; otherwise, <c>false</c>.
        /// </returns>
        public bool CanConfigureTeams()
        {
            return true;
        }

        /// <summary>
        /// Determines whether this instance [can configure weeks].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance [can configure weeks]; otherwise, <c>false</c>.
        /// </returns>
        public bool CanConfigureWeeks()
        {
            return true;
        }

        /// <summary>
        /// Determines whether this instance [can save league].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance [can save league]; otherwise, <c>false</c>.
        /// </returns>
        public bool CanSaveLeague()
        {
            return true;
        }

        /// <summary>
        /// Configures the players.
        /// </summary>
        public void ConfigurePlayers()
        {
            WindowManager manager = new WindowManager();
            manager.ShowDialog(new ConfigurePlayerTeamViewModel(_league.Teams, _league.Players));
        }

        /// <summary>
        /// Determines whether this instance [can add scores].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance [can add scores]; otherwise, <c>false</c>.
        /// </returns>
        public bool CanAddScores()
        {
            return true;
        }

        public void AddScores()
        {
            WindowManager manager = new WindowManager();
            manager.ShowDialog(new AddScoresViewModel(_league.Weeks, _league.Players, _league.Teams, _league.Matchups, _league.WeeklyScores));
        }

        /// <summary>
        /// Configures the schedule.
        /// </summary>
        public void ConfigureSchedule()
        {
            WindowManager manager = new WindowManager();
            manager.ShowWindow(new ConfigureScheduleViewModel(_league.Matchups, _league.Weeks, _league.Teams));
        }

        /// <summary>
        /// Configures the teams.
        /// </summary>
        public void ConfigureTeams()
        {
            WindowManager manager = new WindowManager();
            manager.ShowDialog(new ConfigureTeamViewModel(_league.Teams));
        }

        /// <summary>
        /// Configures the weeks.
        /// </summary>
        public void ConfigureWeeks()
        {
            WindowManager manager = new WindowManager();
            manager.ShowDialog(new ConfigureWeeksViewModel(_league.Weeks));
        }

        /// <summary>
        /// Saves the league.
        /// </summary>
        public void SaveLeague()
        {
            EventAggregationProvider.EventAggregator.Publish(_league);
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(List<Player> message)
        {
            _league.Players = message;
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(List<Team> message)
        {
            _league.Teams = message;
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(List<Week> message)
        {
            _league.Weeks = message;
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(List<WeeklyMatchup> message)
        {
            _league.Matchups = message;
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(List<WeeklyScore> message)
        {
            _league.WeeklyScores = message;
        }
    }
}