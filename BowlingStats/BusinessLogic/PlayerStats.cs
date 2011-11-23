using System;
using System.Collections.Generic;
using System.Linq;
using BowlingStats.Model;

namespace BowlingStats.BusinessLogic
{
    /// <summary>
    /// 
    /// </summary>
    public class PlayerStats
    {
        /// <summary>
        /// Gets the player average.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="scores">The scores.</param>
        /// <param name="getAverageToThisWeek">The get average to this week.</param>
        /// <returns></returns>
        public double GetPlayerAverage(Player player, IEnumerable<WeeklyScore> scores, int getAverageToThisWeek)
        {
            List<WeeklyScore> scoresForPlayer = scores.Where(s => s.Player.Equals(player) && !s.Absent && s.Week.WeekNumber <= getAverageToThisWeek).ToList();
            double average = player.InitialAverage;

            if (scoresForPlayer.Count > 0)
            {
                double totalPins = scoresForPlayer.Sum(s => s.Total);
                average = Math.Round(totalPins / (3 * scoresForPlayer.Count), 0, MidpointRounding.AwayFromZero);
            }

            return average;
        }

        public double GetPlayerAverage(Player player, IEnumerable<WeeklyScore> scores)
        {
            List<WeeklyScore> scoresForPlayer = scores.Where(s => s.Player.Equals(player) && !s.Absent).ToList();
            double average = player.InitialAverage;

            if (scoresForPlayer.Count > 0)
            {
                double totalPins = scoresForPlayer.Sum(s => s.Total);
                average = Math.Round(totalPins / (3 * scoresForPlayer.Count), 0, MidpointRounding.AwayFromZero);
            }

            return average;
        }

        public int GetTotalPinsForPlayer(Player player, List<WeeklyScore> scores)
        {
            return scores.Where(weeklyScore => weeklyScore.Player.Equals(player) && !weeklyScore.Absent).Sum(weeklyScore => weeklyScore.Total);
        }

        public int GetTotalGamesForPlayer(Player player, List<WeeklyScore> scores)
        {
            return scores.Where(weeklyScore => weeklyScore.Player.Equals(player) && !weeklyScore.Absent).Sum(weeklyScore => 3);
        }

        public int GetTotalGamesWithPinsBetween200And250ForPlayer(Player player, List<WeeklyScore> scores)
        {
            int totalGamesWithPinsBetween200And250ForPlayer = 0;

            foreach (WeeklyScore weeklyScore in
                scores.Where(weeklyScore => weeklyScore.Player.Equals(player) && !weeklyScore.Absent))
            {
                if (IsGameBetween200And250(weeklyScore.Game1))
                {
                    totalGamesWithPinsBetween200And250ForPlayer++;
                }
                if (IsGameBetween200And250(weeklyScore.Game2))
                {
                    totalGamesWithPinsBetween200And250ForPlayer++;
                }
                if (IsGameBetween200And250(weeklyScore.Game3))
                {
                    totalGamesWithPinsBetween200And250ForPlayer++;
                }
            }

            return totalGamesWithPinsBetween200And250ForPlayer;
        }

        private bool IsGameBetween200And250(int pins)
        {
            return pins >= 200 && pins < 250;
        }

        private bool IsGameOver250(int pins)
        {
            return pins >= 250;
        }

        public int GetTotalGamesWithPinsOver250ForPlayer(Player player, List<WeeklyScore> scores)
        {
            int totalGamesWithPinsOver250ForPlayer = 0;

            foreach (WeeklyScore weeklyScore in scores)
            {
                if (weeklyScore.Player.Equals(player) && !weeklyScore.Absent)
                {
                    if (IsGameOver250(weeklyScore.Game1))
                    {
                        totalGamesWithPinsOver250ForPlayer++;
                    }
                    if (IsGameOver250(weeklyScore.Game2))
                    {
                        totalGamesWithPinsOver250ForPlayer++;
                    }
                    if (IsGameOver250(weeklyScore.Game3))
                    {
                        totalGamesWithPinsOver250ForPlayer++;
                    }
                }
            }

            return totalGamesWithPinsOver250ForPlayer;
        }

        public List<Point> GetTop3HighGamePlayers(List<WeeklyScore> scores, string gender)
        {
            List<Point> points = new List<Point>();

            foreach (WeeklyScore lastWeekScore in scores)
            {
                Player player = lastWeekScore.Player;

                if (!lastWeekScore.Absent && player.Gender == gender)
                {
                    points.Add(new Point(player.Name, lastWeekScore.Game1));
                    points.Add(new Point(player.Name, lastWeekScore.Game2));
                    points.Add(new Point(player.Name, lastWeekScore.Game3));
                }
            }

            points.Sort((p1, p2) => p2.Points.CompareTo(p1.Points));
            return points.Take(3).ToList();
        }
       
        public List<Point> GetTop3HighSeriesPlayers(List<WeeklyScore> scores, string gender)
        {
            List<Point> points = (from lastWeekScore in scores
                                  let player = lastWeekScore.Player
                                  where !lastWeekScore.Absent && player.Gender == gender
                                  select new Point(player.Name, lastWeekScore.Total)).ToList();

            points.Sort((p1, p2) => p2.Points.CompareTo(p1.Points));
            return points.Take(3).ToList();
        }
    }
}