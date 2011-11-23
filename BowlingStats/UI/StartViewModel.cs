using System.Collections.Generic;
using System.Collections.ObjectModel;
using BowlingStats.DataAccess;
using BowlingStats.Model;
using Caliburn.Micro;

namespace BowlingStats.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class StartViewModel : PropertyChangedBase, IHandle<League>, IHandle<List<Player>>
    {
        private ObservableCollection<League> _leagues;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartViewModel"/> class.
        /// </summary>
        public StartViewModel()
        {
            EventAggregationProvider.EventAggregator.Subscribe(this);
            _leagues = new ObservableCollection<League>(GetLeagues());
        }

        /// <summary>
        /// Gets a value indicating whether this instance can add league.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can add league; otherwise, <c>false</c>.
        /// </value>
        public bool CanAddLeague
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance can modify players.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can modify players; otherwise, <c>false</c>.
        /// </value>
        public bool CanModifyPlayers
        {
            get { return true; }
        }

        /// <summary>
        /// Gets or sets the leagues.
        /// </summary>
        /// <value>
        /// The leagues.
        /// </value>
        public ObservableCollection<League> Leagues
        {
            get { return _leagues; }
            set
            {
                _leagues = value;
                NotifyOfPropertyChange(() => Leagues);
            }
        }

        /// <summary>
        /// Adds the league.
        /// </summary>
        public void AddLeague()
        {
            WindowManager manager = new WindowManager();
            manager.ShowWindow(new ConfigureLeagueViewModel(GetAllPlayers()), null);
        }

        /// <summary>
        /// Determines whether this instance [can delete league].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance [can delete league]; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDeleteLeague()
        {
            return true;
        }

        public bool CanEditLeague()
        {
            return true;
        }

        /// <summary>
        /// Deletes the league.
        /// </summary>
        public void DeleteLeague(League league)
        {
            new RavenDataAccess().DeleteLeague(league.Id);
            Leagues = new ObservableCollection<League>(GetLeagues());
        }

        public void EditLeague(League league)
        {
            WindowManager manager = new WindowManager();
            List<Player> allPlayers = GetAllPlayers();

            foreach (Player allPlayer in allPlayers)
            {
                Player currentPlayer = allPlayer;
                Player player = league.Players.Find(p => p != null && p.Id == currentPlayer.Id);

                if (player == null)
                {
                    league.Players.Add(currentPlayer);
                }
            }
            manager.ShowWindow(new ConfigureLeagueViewModel(league));
        }

        /// <summary>
        /// Modifies the players.
        /// </summary>
        public void ModifyPlayers()
        {
            List<Player> players = GetAllPlayers();
            WindowManager manager = new WindowManager();
            manager.ShowWindow(new ConfigurePlayerViewModel(players));
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(League message)
        {
            Leagues.Add(message);
            RavenDataAccess rda = new RavenDataAccess();
            rda.SaveLeague(message);
            Leagues = new ObservableCollection<League>(GetLeagues());
        }

        /// <summary>
        /// Handles the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Handle(List<Player> message)
        {
            PlayerRavenDataAccess playerRavenDataAccess = new PlayerRavenDataAccess();
            playerRavenDataAccess.SavePlayers(message);
            List<Player> players = GetAllPlayers();

            foreach (League league in _leagues)
            {
                league.Players = players;
            }
        }

        private List<Player> GetAllPlayers()
        {
            return new PlayerRavenDataAccess().GetAllPlayers();
        }

        private List<League> GetLeagues()
        {
            return new RavenDataAccess().GetAllLeagues();
        }
    }
}