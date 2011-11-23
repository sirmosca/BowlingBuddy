using System.Collections.Generic;

namespace BowlingStats.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class League
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the players.
        /// </summary>
        /// <value>
        /// The players.
        /// </value>
        public List<Player> Players { get; set; }

        /// <summary>
        /// Gets or sets the matchups.
        /// </summary>
        /// <value>
        /// The matchups.
        /// </value>
        public List<WeeklyMatchup> Matchups { get; set; }

        /// <summary>
        /// Gets or sets the weekly scores.
        /// </summary>
        /// <value>
        /// The weekly scores.
        /// </value>
        public List<WeeklyScore> WeeklyScores { get; set; }

        /// <summary>
        /// Gets or sets the weeks.
        /// </summary>
        /// <value>
        /// The weeks.
        /// </value>
        public List<Week> Weeks { get; set; }

        /// <summary>
        /// Gets or sets the teams.
        /// </summary>
        /// <value>
        /// The teams.
        /// </value>
        public List<Team> Teams { get; set; }
    }
}