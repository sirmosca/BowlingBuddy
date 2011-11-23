using System;
using System.Collections.Generic;
using BowlingStats.Model;

namespace BowlingStats.BusinessLogic
{
    /// <summary>
    /// represents a line item on the print Matchups reports
    /// </summary>
    public class MatchupReportLineItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatchupReportLineItem"/> class.
        /// </summary>
        /// <param name="matchupReportData">The matchup report data.</param>
        /// <param name="playerAveragesDictionary">The player averages dictionary.</param>
        public MatchupReportLineItem(MatchupReportTeamData matchupReportData, Dictionary<Player, double> playerAveragesDictionary)
        {
            MatchupReportData = matchupReportData;
            PlayerAveragesDictionary = playerAveragesDictionary;
        }

        /// <summary>
        /// Gets the player averages dictionary.
        /// </summary>
        public Dictionary<Player, double> PlayerAveragesDictionary { get; private set; }

        /// <summary>
        /// Gets the matchup report data.
        /// </summary>
        public MatchupReportTeamData MatchupReportData { get; private set; }
    }
}