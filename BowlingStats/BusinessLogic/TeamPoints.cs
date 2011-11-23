using System.Collections.Generic;
using System.Linq;
using BowlingStats.Model;

namespace BowlingStats.BusinessLogic
{
    public class TeamPoints
    {
        public void UpdatePointsForTeam(IEnumerable<WeeklyScore> team1Scores, IEnumerable<WeeklyScore> team2Scores,
                                        Week currentWeek, Team team1, Team team2, IEnumerable<Player> players)
        {
            List<Player> team1Players = players.Where(p => p.Team.Equals(team1)).ToList();
            IEnumerable<Player> team2Players = players.Where(p => p.Team.Equals(team2));
            double team1Average = GetTeamAveragePriorToTheStartOfTheWeek(team1Scores, team1Players, currentWeek);
            double team2Average = GetTeamAveragePriorToTheStartOfTheWeek(team2Scores, team2Players, currentWeek);
            Handicap handicap = new Handicap();
            double team1Handicap = handicap.GetHandicap(team1Average, team2Average);
            double team2Handicap = handicap.GetHandicap(team2Average, team1Average);

            double team1Points = 0;
            double team2Points = 0;

            double team1Game1 = team1Scores.Where(s => s.Week.WeekNumber == currentWeek.WeekNumber).Sum(s => s.Game1);
            double team1Game2 = team1Scores.Where(s => s.Week.WeekNumber == currentWeek.WeekNumber).Sum(s => s.Game2);
            double team1Game3 = team1Scores.Where(s => s.Week.WeekNumber == currentWeek.WeekNumber).Sum(s => s.Game3);
            double team1Total = team1Scores.Where(s => s.Week.WeekNumber == currentWeek.WeekNumber).Sum(s => s.Total);
            double team2Game1 = team2Scores.Where(s => s.Week.WeekNumber == currentWeek.WeekNumber).Sum(s => s.Game1);
            double team2Game2 = team2Scores.Where(s => s.Week.WeekNumber == currentWeek.WeekNumber).Sum(s => s.Game2);
            double team2Game3 = team2Scores.Where(s => s.Week.WeekNumber == currentWeek.WeekNumber).Sum(s => s.Game3);
            double team2Total = team2Scores.Where(s => s.Week.WeekNumber == currentWeek.WeekNumber).Sum(s => s.Total);

            CalculatePointsPerRule(team1Game1, team1Handicap, team2Game1, team2Handicap, ref team1Points,
                                   ref team2Points);
            CalculatePointsPerRule(team1Game2, team1Handicap, team2Game2, team2Handicap, ref team1Points,
                                   ref team2Points);
            CalculatePointsPerRule(team1Game3, team1Handicap, team2Game3, team2Handicap, ref team1Points,
                                   ref team2Points);
            CalculatePointsPerRule(team1Total, team1Handicap*3, team2Total, team2Handicap*3, ref team1Points,
                                   ref team2Points);

            WeeklyPoint team1WeeklyPoint = team1.WeeklyPoints.FirstOrDefault(wp => wp.Week.Equals(currentWeek));
            WeeklyPoint team2WeeklyPoint = team2.WeeklyPoints.FirstOrDefault(wp => wp.Week.Equals(currentWeek));

            if (team1WeeklyPoint != null)
            {
                team1WeeklyPoint.Points = team1Points;
            }
            else
            {
                team1WeeklyPoint = new WeeklyPoint {Points = team1Points, Week = currentWeek};
                team1.WeeklyPoints.Add(team1WeeklyPoint);
            }

            if (team2WeeklyPoint != null)
            {
                team2WeeklyPoint.Points = team2Points;
            }
            else
            {
                team2WeeklyPoint = new WeeklyPoint {Points = team2Points, Week = currentWeek};
                team2.WeeklyPoints.Add(team2WeeklyPoint);
            }
        }

        private double GetTeamAveragePriorToTheStartOfTheWeek(IEnumerable<WeeklyScore> weeklyScores, IEnumerable<Player> players, Week week)
        {
            TeamStats teamStats = new TeamStats();
            double average = teamStats.GetTeamAverage(weeklyScores, players, week.WeekNumber - 1);

            return average;
        }

        private void CalculatePointsPerRule(double team1Game, double team1Handicap, double team2Game,
                                            double team2Handicap, ref double team1Points, ref double team2Points)
        {
            if (team1Game + team1Handicap > team2Game + team2Handicap)
            {
                team1Points += 1;
            }
            else if (team1Game + team1Handicap < team2Game + team2Handicap)
            {
                team2Points += 1;
            }
            else
            {
                team1Points += 0.5;
                team2Points += 0.5;
            }
        }
    }
}