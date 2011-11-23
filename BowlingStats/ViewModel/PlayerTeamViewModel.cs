using System.Collections.ObjectModel;
using BowlingStats.Model;
using Caliburn.Micro;

namespace BowlingStats.ViewModel
{
    public class PlayerTeamViewModel : PropertyChangedBase
    {
        private ObservableCollection<Player> _players;
        private Player _selectedPlayer;
        private Team _selectedTeam;
        private ObservableCollection<Team> _teams;

        /// <summary>
        /// Gets or sets the name of the player.
        /// </summary>
        /// <value>
        /// The name of the player.
        /// </value>
        public string PlayerName { get; set; }

        /// <summary>
        /// Gets or sets the players.
        /// </summary>
        /// <value>
        /// The players.
        /// </value>
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
        /// Gets or sets the selected player.
        /// </summary>
        /// <value>
        /// The selected player.
        /// </value>
        public Player SelectedPlayer
        {
            get { return _selectedPlayer; }
            set
            {
                _selectedPlayer = value;
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
                NotifyOfPropertyChange(() => SelectedTeam);
            }
        }
        /// <summary>
        /// Gets or sets the name of the team.
        /// </summary>
        /// <value>
        /// The name of the team.
        /// </value>
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
    }
}