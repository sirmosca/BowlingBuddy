using System;
using System.Collections.Generic;
using BowlingStats.Model;

namespace BowlingStats.BusinessLogic
{
    /// <summary>
    /// Class used for holding data in order to print on the matchup report
    /// </summary>
    public class MatchupReportTeamData
    {
        public MatchupReportTeamData(Team team1, List<Player> team1Players, double team1Average, double team1Handicap, Team team2, List<Player> team2Players, double team2Average, double team2Handicap)
        {
            Team1 = team1;
            Team1Players = team1Players;
            Team1Average = team1Average;
            Team1Handicap = team1Handicap;
            Team2 = team2;
            Team2Players = team2Players;
            Team2Average = team2Average;
            Team2Handicap = team2Handicap;
        }

        /// <summary>
        /// Gets the team1.
        /// </summary>
        public Team Team1 { get; private set; }

        /// <summary>
        /// Gets the team1 players.
        /// </summary>
        public List<Player> Team1Players { get; private set; }

        /// <summary>
        /// Gets or sets the team1 average.
        /// </summary>
        /// <value>
        /// The team1 average.
        /// </value>
        public double Team1Average { get; private set; }

        /// <summary>
        /// Gets or sets the team1 handicap.
        /// </summary>
        /// <value>
        /// The team1 handicap.
        /// </value>
        public double Team1Handicap { get; private set; }

        /// <summary>
        /// Gets or sets the team2.
        /// </summary>
        /// <value>
        /// The team2.
        /// </value>
        public Team Team2 { get; private set; }

        /// <summary>
        /// Gets or sets the team2 players.
        /// </summary>
        /// <value>
        /// The team2 players.
        /// </value>
        public List<Player> Team2Players { get; private set; }

        /// <summary>
        /// Gets or sets the team2 average.
        /// </summary>
        /// <value>
        /// The team2 average.
        /// </value>
        public double Team2Average { get; private set; }

        /// <summary>
        /// Gets or sets the team2 handicap.
        /// </summary>
        /// <value>
        /// The team2 handicap.
        /// </value>
        public double Team2Handicap { get; private set; }
    }
}