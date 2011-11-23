using System;
using System.Collections.Generic;
using System.Linq;
using BowlingStats.Model;

namespace BowlingStats.BusinessLogic
{
    public class TeamStats
    {
        public double GetTeamAverage(IEnumerable<WeeklyScore> scores, IEnumerable<Player> players)
        {
            PlayerStats playerAverage = new PlayerStats();
            double average = players.Sum(player => playerAverage.GetPlayerAverage(player, scores));

            double roundedAverage = Math.Round(average, 0, MidpointRounding.AwayFromZero);

            return roundedAverage;
        }

        public double GetTeamAverage(IEnumerable<WeeklyScore> scores, IEnumerable<Player> players,
                                     int getAverageToThisWeek)
        {
            PlayerStats playerStats = new PlayerStats();
            double average = 0;// players.Sum(player => playerStats.GetPlayerAverage(player, scores, getAverageToThisWeek));

            foreach (Player player in players)
            {
                average += playerStats.GetPlayerAverage(player, scores, getAverageToThisWeek);
            }
            double roundedAverage = Math.Round(average, 0, MidpointRounding.AwayFromZero);

            return roundedAverage;
        }


        /// <summary>
        ///   Gets the team points.
        /// </summary>
        /// <param name = "scoresForHandicap">The scores for handicap.</param>
        /// <param name = "scoresIncludingSubs">The scores including subs.</param>
        /// <param name = "teams">The teams.</param>
        /// <param name = "matchups">The matchups.</param>
        /// <param name = "players">The players.</param>
        /// <returns></returns>
        public List<Points> GetTeamPoints(
            List<WeeklyScore> scoresForHandicap, List<WeeklyScore> scoresIncludingSubs, List<Team> teams,
            List<WeeklyMatchup> matchups, List<Player> players)
        {
            List<Points> teamPoints = teams.Select(team => new Points {Name = team.Name, TotalPoints = 0}).ToList();
            matchups.Sort((s1, s2) => Comparer<int>.Default.Compare(s1.Week.WeekNumber, s2.Week.WeekNumber));

            foreach (WeeklyMatchup weeklyMatchup in matchups)
            {
                WeeklyMatchup matchup = weeklyMatchup;
                List<WeeklyScore> team1Scores =
                    scoresIncludingSubs.Where(
                        s1 => s1.Team.Name == matchup.TeamOne.Name && s1.Week.WeekNumber == matchup.Week.WeekNumber).
                        ToList();
                List<WeeklyScore> team2Scores =
                    scoresIncludingSubs.Where(
                        s1 => s1.Team.Name == matchup.TeamTwo.Name && s1.Week.WeekNumber == matchup.Week.WeekNumber).
                        ToList();
                List<Player> team1Players = (from scoresIncludingSub in scoresIncludingSubs
                                             where
                                                 scoresIncludingSub.Week.WeekNumber == matchup.Week.WeekNumber &&
                                                 scoresIncludingSub.Team.Name == matchup.TeamOne.Name
                                             select players.Find(p => p.Id == scoresIncludingSub.Player.Id)).ToList();
                List<Player> team2Players = (from scoresIncludingSub in scoresIncludingSubs
                                             where
                                                 scoresIncludingSub.Week.WeekNumber == matchup.Week.WeekNumber &&
                                                 scoresIncludingSub.Team.Name == matchup.TeamTwo.Name
                                             select players.Find(p => p.Id == scoresIncludingSub.Player.Id)).ToList();

                if (weeklyMatchup.Week.WeekNumber == 1)
                {
                    double team1Average = team1Players.Sum(t1 => t1.InitialAverage);
                    double team2Average = team2Players.Sum(t2 => t2.InitialAverage);
                    double team1Handicap = team1Average < team2Average ? team2Average - team1Average : 0;
                    double team2Handicap = team2Average < team1Average ? team1Average - team2Average : 0;

                    LoadTeamPointsForWeek(teamPoints, team1Handicap, team2Handicap, matchup.TeamOne.Name,
                                          matchup.TeamTwo.Name, team1Scores, team2Scores);
                }
                else
                {
                    PlayerStats playerStats = new PlayerStats();
                    Dictionary<int, double> playerAverageDictionary1 = team1Players.ToDictionary(
                        player => player.Id,
                        player =>
                        playerStats.GetPlayerAverage(player,
                                                     scoresForHandicap.FindAll(
                                                         s =>
                                                         s.Player.Id == player.Id &&
                                                         s.Week.WeekNumber < matchup.Week.WeekNumber)));
                    Dictionary<int, double> playerAverageDictionary2 = team2Players.ToDictionary(
                        player => player.Id,
                        player =>
                        playerStats.GetPlayerAverage(player,
                                                     scoresForHandicap.FindAll(
                                                         s =>
                                                         s.Player.Id == player.Id &&
                                                         s.Week.WeekNumber < matchup.Week.WeekNumber)));

                    double team1Average = playerAverageDictionary1.Sum(keyValuePair => keyValuePair.Value);
                    double team2Average = playerAverageDictionary2.Sum(keyValuePair => keyValuePair.Value);
                    double team1Handicap = team1Average < team2Average ? team2Average - team1Average : 0;
                    double team2Handicap = team2Average < team1Average ? team1Average - team2Average : 0;

                    LoadTeamPointsForWeek(teamPoints, team1Handicap, team2Handicap, matchup.TeamOne.Name,
                                          matchup.TeamTwo.Name, team1Scores, team2Scores);
                }
            }

            return teamPoints;
        }

        public void LoadTeamPointsForWeek(
            List<Points> teamPoints,
            double team1Handicap,
            double team2Handicap,
            string teamOneName,
            string teamTwoName,
            List<WeeklyScore> team1Scores,
            List<WeeklyScore> team2Scores)
        {
            double team1Total = team1Handicap;
            double team2Total = team2Handicap;

            team1Total = team1Scores.Aggregate(team1Total, (current, team1Score) => current + team1Score.Game1);
            team2Total = team2Scores.Aggregate(team2Total, (current, team2Score) => current + team2Score.Game1);

            if (team1Total > team2Total)
            {
                Points p = teamPoints.Find(p1 => p1.Name == teamOneName);
                p.TotalPoints++;
            }
            else if (team2Total > team1Total)
            {
                Points p = teamPoints.Find(p1 => p1.Name == teamTwoName);
                p.TotalPoints++;
            }
            else
            {
                Points t1p = teamPoints.Find(p1 => p1.Name == teamOneName);
                t1p.TotalPoints += 0.5;
                Points t2p = teamPoints.Find(p2 => p2.Name == teamTwoName);
                t2p.TotalPoints += 0.5;
            }

            team1Total = team1Handicap;
            team2Total = team2Handicap;
            team1Total = team1Scores.Aggregate(team1Total, (current, team1Score) => current + team1Score.Game2);
            team2Total = team2Scores.Aggregate(team2Total, (current, team2Score) => current + team2Score.Game2);

            if (team1Total > team2Total)
            {
                Points p = teamPoints.Find(p1 => p1.Name == teamOneName);
                p.TotalPoints++;
            }
            else if (team2Total > team1Total)
            {
                Points p = teamPoints.Find(p1 => p1.Name == teamTwoName);
                p.TotalPoints++;
            }
            else
            {
                Points t1p = teamPoints.Find(p1 => p1.Name == teamOneName);
                t1p.TotalPoints += 0.5;
                Points t2p = teamPoints.Find(p2 => p2.Name == teamTwoName);
                t2p.TotalPoints += 0.5;
            }

            team1Total = team1Handicap;
            team2Total = team2Handicap;
            team1Total = team1Scores.Aggregate(team1Total, (current, team1Score) => current + team1Score.Game3);
            team2Total = team2Scores.Aggregate(team2Total, (current, team2Score) => current + team2Score.Game3);

            if (team1Total > team2Total)
            {
                Points p = teamPoints.Find(p1 => p1.Name == teamOneName);
                p.TotalPoints++;
            }
            else if (team2Total > team1Total)
            {
                Points p = teamPoints.Find(p1 => p1.Name == teamTwoName);
                p.TotalPoints++;
            }
            else
            {
                Points t1p = teamPoints.Find(p1 => p1.Name == teamOneName);
                t1p.TotalPoints += 0.5;
                Points t2p = teamPoints.Find(p2 => p2.Name == teamTwoName);
                t2p.TotalPoints += 0.5;
            }

            team1Total = team1Handicap*3;
            team2Total = team2Handicap*3;
            team1Total = team1Scores.Aggregate(team1Total, (current, team1Score) => current + team1Score.Total);
            team2Total = team2Scores.Aggregate(team2Total, (current, team2Score) => current + team2Score.Total);

            if (team1Total > team2Total)
            {
                Points p = teamPoints.Find(p1 => p1.Name == teamOneName);
                p.TotalPoints++;
            }
            else if (team2Total > team1Total)
            {
                Points p = teamPoints.Find(p1 => p1.Name == teamTwoName);
                p.TotalPoints++;
            }
            else
            {
                Points t1p = teamPoints.Find(p1 => p1.Name == teamOneName);
                t1p.TotalPoints += 0.5;
                Points t2p = teamPoints.Find(p2 => p2.Name == teamTwoName);
                t2p.TotalPoints += 0.5;
            }
        }
    }
}