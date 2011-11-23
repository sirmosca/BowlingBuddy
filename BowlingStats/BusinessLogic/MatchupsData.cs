using System.Collections.Generic;
using System.Linq;
using BowlingStats.Model;

namespace BowlingStats.BusinessLogic
{
    /// <summary>
    /// Class that gets the data used for the print matchup report
    /// </summary>
    public class MatchupsData
    {
        /// <summary>
        /// Gets the team data for matchup for the scores provided up to the week number specified. Does not include the current week number.
        /// </summary>
        /// <param name="team1Players">The team1Players.</param>
        /// <param name="team2Players">The team2 players.</param>
        /// <param name="scores">The scores.</param>
        /// <param name="weekNumber">The week number.</param>
        /// <returns></returns>
        public MatchupReportTeamData GetTeamDataForMatchup(List<Player> team1Players, List<Player> team2Players, IEnumerable<WeeklyScore> scores, int weekNumber)
        {
            Team team1 = team1Players[0].Team;
            Team team2 = team2Players[0].Team;
            List<WeeklyScore> scoresForTeam1 = GetScoresToWeek(scores, weekNumber, team1);
            List<WeeklyScore> scoresForTeam2 = GetScoresToWeek(scores, weekNumber, team2);
            TeamStats teamAverage = new TeamStats();
            Handicap handicap = new Handicap();
            double team1Average = teamAverage.GetTeamAverage(scoresForTeam1, team1Players);
            double team2Average = teamAverage.GetTeamAverage(scoresForTeam2, team2Players);
            double team1Handicap = handicap.GetHandicap(team1Average, team2Average);// team1Average < team2Average ? team2Average - team1Average : 0;
            double team2Handicap = handicap.GetHandicap(team2Average, team1Average);// team2Average < team1Average ? team1Average - team2Average : 0;

            return new MatchupReportTeamData(team1, team1Players, team1Average, team1Handicap, team2, team2Players, team2Average, team2Handicap);
        }

        private List<WeeklyScore> GetScoresToWeek(IEnumerable<WeeklyScore> allScores, int weekNumber, Team team)
        {
            IEnumerable<WeeklyScore> scores =
                allScores.Where(weeklyScore => weeklyScore.Week.WeekNumber < weekNumber && weeklyScore.Team.Equals(team));
            return scores.ToList();
        }
    }
}