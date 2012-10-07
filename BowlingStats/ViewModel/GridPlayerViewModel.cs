using System.Collections.ObjectModel;
using BowlingStats.Model;
using Caliburn.Micro;

namespace BowlingStats.ViewModel
{
    public class GridPlayerViewModel : PropertyChangedBase
    {
        private int _game1;
        private int _game2;
        private int _game3;
        private ObservableCollection<Player> _players;
        private Player _selectedPlayer;
        private Team _selectedTeam;
        private ObservableCollection<Team> _teams;

        /// <summary>
        /// Initializes a new instance of the <see cref="GridPlayerViewModel"/> class.
        /// </summary>
        public GridPlayerViewModel()
        {
            Players = new ObservableCollection<Player>();
            Teams = new ObservableCollection<Team>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GridPlayerViewModel"/> is absent.
        /// </summary>
        /// <value><c>true</c> if absent; otherwise, <c>false</c>.</value>
        public bool Absent { get; set; }

        /// <summary>
        /// Gets or sets the game1.
        /// </summary>
        /// <value>The game1.</value>
        public int Game1
        {
            get { return _game1; }
            set
            {
                _game1 = value;
                NotifyOfPropertyChange(() => Game1);
                NotifyOfPropertyChange(() => Total);
            }
        }

        /// <summary>
        /// Gets or sets the game2.
        /// </summary>
        /// <value>The game2.</value>
        public int Game2
        {
            get { return _game2; }
            set
            {
                _game2 = value;
                NotifyOfPropertyChange(() => Game2);
                NotifyOfPropertyChange(() => Total);
            }
        }

        /// <summary>
        /// Gets or sets the game3.
        /// </summary>
        /// <value>The game3.</value>
        public int Game3
        {
            get { return _game3; }
            set
            {
                _game3 = value;
                NotifyOfPropertyChange(() => Game3);
                NotifyOfPropertyChange(() => Total);
            }
        }

        /// <summary>
        /// Gets or sets the player id.
        /// </summary>
        /// <value>The player id.</value>
        public int PlayerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the player.
        /// </summary>
        /// <value>The name of the player.</value>
        public string PlayerName { get; set; }

        /// <summary>
        /// Gets or sets the grid players.
        /// </summary>
        /// <value>The grid players.</value>
        public ObservableCollection<Player> Players
        {
            get { return _players; }
            set
            {
                _players = value;
                NotifyOfPropertyChange(() => Players);
            }
        }

        /// <summary>
        /// Gets or sets the selected grid player.
        /// </summary>
        /// <value>The selected grid player.</value>
        public Player SelectedPlayer
        {
            get { return _selectedPlayer; }
            set
            {
                _selectedPlayer = value;
                UpdateSelectedGridPlayer(_selectedPlayer);
                NotifyOfPropertyChange(() => SelectedPlayer);
            }
        }

        /// <summary>
        /// Gets or sets the selected team.
        /// </summary>
        /// <value>
        /// The selected team.
        /// </value>
        public Team SelectedTeam
        {
            get { return _selectedTeam; }
            set
            {
                _selectedTeam = value;
                UpdateSelectedTeam(_selectedTeam);
            }
        }

        /// <summary>
        /// Gets or sets the name of the team.
        /// </summary>
        /// <value>The name of the team.</value>
        public string TeamName { get; set; }

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
        /// Gets or sets the total.
        /// </summary>
        /// <value>The total.</value>
        public int Total
        {
            get { return Game1 + Game2 + Game3; }
        }

        private void UpdateSelectedGridPlayer(Player selectedGridPlayer)
        {
            PlayerName = selectedGridPlayer.Name;
            PlayerId = selectedGridPlayer.Id;
        }

        private void UpdateSelectedTeam(Team selectedTeam)
        {
            TeamName = selectedTeam.Name;
        }
    }
}