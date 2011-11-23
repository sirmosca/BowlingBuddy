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
    public class ConfigurePlayerViewModel : PropertyChangedBase
    {
        private ObservableCollection<Player> _players;
        private Player _selectedPlayer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurePlayerViewModel"/> class.
        /// </summary>
        /// <param name="currentPlayers">The current players.</param>
        public ConfigurePlayerViewModel(List<Player> currentPlayers)
        {
            _players = new ObservableCollection<Player>(currentPlayers);
        }

        /// <summary>
        /// Gets or sets the players.
        /// </summary>
        /// <value>The players.</value>
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
        /// <value>The selected player.</value>
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
            List<Player> players = _players.ToList();
            players.Sort((p1, p2) => p1.Name.CompareTo(p2.Name));
            EventAggregationProvider.EventAggregator.Publish(players);
        }
    }
}