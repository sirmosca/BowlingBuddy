using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using BowlingStats.Model;
using BowlingStats.ViewModel;
using Caliburn.Micro;

namespace BowlingStats.UI
{
    public class ConfigurePlayerTeamViewModel : PropertyChangedBase
    {
        private List<Player> _currentPlayers;
        private List<Team> _currentTeams;
        private ObservableCollection<PlayerTeamViewModel> _players;
        private PlayerTeamViewModel _selectedPlayer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurePlayerTeamViewModel"/> class.
        /// </summary>
        /// <param name="teams">The teams.</param>
        /// <param name="players">The players.</param>
        public ConfigurePlayerTeamViewModel(List<Team> teams, List<Player> players)
        {
            _currentTeams = teams;
            _currentPlayers = players;
            _players = new ObservableCollection<PlayerTeamViewModel>();
            _players.CollectionChanged += CollectionChanged;
            Initialize(_currentPlayers);
        }

        /// <summary>
        /// Gets or sets the players.
        /// </summary>
        /// <value>The players.</value>
        public ObservableCollection<PlayerTeamViewModel> Players
        {
            get { return _players; }
            set
            {
                _players = value;
                NotifyOfPropertyChange(() => Players);
            }
        }

        /// <summary>
        /// Gets or sets the selected player.
        /// </summary>
        /// <value>The selected player.</value>
        public PlayerTeamViewModel SelectedPlayer
        {
            get { return _selectedPlayer; }
            set
            {
                _selectedPlayer = value;
                NotifyOfPropertyChange(() => SelectedPlayer);
            }
        }

        /// <summary>
        /// Determines whether this instance [can save player].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance [can save player]; otherwise, <c>false</c>.
        /// </returns>
        public bool CanSavePlayer()
        {
            return true;
        }

        /// <summary>
        /// Saves the player.
        /// </summary>
        public void SavePlayer()
        {
            List<Player> players = new List<Player>();

            foreach (PlayerTeamViewModel playerTeamViewModel in _players)
            {
                Player p = playerTeamViewModel.SelectedPlayer;
                p.Team = playerTeamViewModel.SelectedTeam;
                players.Add(p);
            }

            EventAggregationProvider.EventAggregator.Publish(players);
        }

        private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (PlayerTeamViewModel model in from object newItem in e.NewItems select newItem as PlayerTeamViewModel)
            {
                model.Teams = new ObservableCollection<Team>(_currentTeams);
                model.Players = new ObservableCollection<Player>(_currentPlayers);
            }
        }

        private void Initialize(IEnumerable<Player> players)
        {
            foreach (Player player in players)
            {
                if (player.Team == null)
                {
                    player.Team = new Team("Sub");
                }

                PlayerTeamViewModel playerTeamViewModel = new PlayerTeamViewModel { PlayerName = player.Name, TeamName = player.Team.Name, SelectedPlayer = player, SelectedTeam = player.Team };

                _players.Add(playerTeamViewModel);
            }
        }
    }
}