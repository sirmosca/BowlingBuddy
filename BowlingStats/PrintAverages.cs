using System;
using System.Collections.Generic;
using System.Linq;
using BowlingStats.BusinessLogic;
using BowlingStats.Model;
using Microsoft.Office.Interop.Excel;

namespace BowlingStats
{
    /// <summary>
    /// 
    /// </summary>
    public class PrintAverages
    {
        /// <summary>
        /// Prints the averages for current weeek.
        /// </summary>
        /// <param name="weekId">The week id.</param>
        /// <param name="scores">The scores.</param>
        /// <param name="players">The players.</param>
        /// <param name="teams">The teams.</param>
        public void PrintAveragesForCurrentWeeek(int weekId, IEnumerable<WeeklyScore> scores, List<Player> players, IEnumerable<Team> teams)
        {
            Application excelApplication = new Application {Visible = true};
            Workbook excelWorkbook = excelApplication.Workbooks.Add(1);
            const int startingRowIndex = 1;
            int rowIndex = startingRowIndex;
            Worksheet excelWorksheet = (Worksheet) excelWorkbook.Sheets[1];

//            Range range = excelWorksheet.Range["a1", "g1"];
//            range.Font.Bold = true;
//            range = excelWorksheet.Range["a1", "g41"];
//            range.BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlColorIndex.xlColorIndexAutomatic, XlColorIndex.xlColorIndexAutomatic);
//
//            range = excelWorksheet.Range["i1", "j1"];
//            range.Font.Bold = true;
//            range = excelWorksheet.Range["i1", "j12"];
//            range.BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlColorIndex.xlColorIndexAutomatic, XlColorIndex.xlColorIndexAutomatic);

//            range.Borders[XlBordersIndex.xlEdgeBottom].Color = Color.FromRgb(0, 0, 0);
//            range.Borders[XlBordersIndex.xlEdgeLeft].Color = Color.FromRgb(0, 0, 0);
//            range.Borders[XlBordersIndex.xlEdgeRight].Color = Color.FromRgb(0, 0, 0);
//            range.Borders[XlBordersIndex.xlEdgeTop].Color = Color.FromRgb(0, 0, 0);
//            range.Borders[XlBordersIndex.xlInsideHorizontal].Color = Color.FromRgb(0, 0, 0);
//            range.Borders[XlBordersIndex.xlInsideVertical].Color = Color.FromRgb(0, 0, 0);

            excelWorksheet.Cells[rowIndex, 1] = "Rank";
            excelWorksheet.Cells[rowIndex, 2] = "Name";
            excelWorksheet.Cells[rowIndex, 3] = "Average";
            excelWorksheet.Cells[rowIndex, 4] = "Total Pins";
            excelWorksheet.Cells[rowIndex, 5] = "Games";
            excelWorksheet.Cells[rowIndex, 6] = "200\'s";
            excelWorksheet.Cells[rowIndex, 7] = "250\'s";
            excelWorksheet.Cells[rowIndex, 9] = "Team";
            excelWorksheet.Cells[rowIndex, 10] = "Average";
            rowIndex++;

            //name, average, total pins, total games, games between 200 and 250, games above 250
            List<Tuple<string, double, int, int, int, int>> playerAverages;
            //team name, average
            List<Tuple<string, double>> teamAverages;
            PlayerStats playerStats = new PlayerStats();
            List<Player> subs = new List<Player>();

            subs.AddRange(players.Where(p => p.Team.Name == "Sub"));
            players.RemoveAll(p => p.Team.Name == "Sub");
            

            if (weekId == 1)
            {
                playerAverages = players.Select(player => new Tuple<string, double, int, int, int, int>(player.Name, player.InitialAverage, 0, 0, 0, 0)).ToList();
                teamAverages = GetTeamAveragesForFirstWeek(players, teams);
            }
            else
            {
                playerAverages = GetPlayerAverages(players, scores, playerStats);
                teamAverages = GetTeamAverages(scores, teams, players);
            }

            playerAverages.Sort((p1, p2) => Comparer<double>.Default.Compare(p2.Item2, p1.Item2));

            for (int i = 0; i < playerAverages.Count; i++)
            {
                Tuple<string, double, int, int, int, int> playerAverage = playerAverages[i];
                excelWorksheet.Cells[rowIndex, 1] = rowIndex - 1; //rank
                excelWorksheet.Cells[rowIndex, 2] = playerAverage.Item1; //name
                excelWorksheet.Cells[rowIndex, 3] = playerAverage.Item2; //average
                excelWorksheet.Cells[rowIndex, 4] = playerAverage.Item3; //total pins
                excelWorksheet.Cells[rowIndex, 5] = playerAverage.Item4; //total games
                excelWorksheet.Cells[rowIndex, 6] = playerAverage.Item5; //games 200-250
                excelWorksheet.Cells[rowIndex, 7] = playerAverage.Item6; //games 250-300

                if (i < teamAverages.Count)
                {
                    excelWorksheet.Cells[rowIndex, 9] = teamAverages[i].Item1;
                    excelWorksheet.Cells[rowIndex, 10] = teamAverages[i].Item2;
                }

                rowIndex++;
            }

            playerAverages = GetPlayerAverages(subs, scores, playerStats);
            playerAverages.Sort((p1, p2) => Comparer<double>.Default.Compare(p2.Item2, p1.Item2));

            rowIndex += 2;
            excelWorksheet.Cells[rowIndex, 1] = "Subs";
            rowIndex++;

            for (int i=0; i < playerAverages.Count; i++)
            {
                Tuple<string, double, int, int, int, int> playerAverage = playerAverages[i];
                excelWorksheet.Cells[rowIndex, 1] = i + 1; //rank
                excelWorksheet.Cells[rowIndex, 2] = playerAverage.Item1; //name
                excelWorksheet.Cells[rowIndex, 3] = playerAverage.Item2; //average
                excelWorksheet.Cells[rowIndex, 4] = playerAverage.Item3; //total pins
                excelWorksheet.Cells[rowIndex, 5] = playerAverage.Item4; //total games
                excelWorksheet.Cells[rowIndex, 6] = playerAverage.Item5; //games 200-250
                excelWorksheet.Cells[rowIndex, 7] = playerAverage.Item6; //games 250-300
                rowIndex++;
            }
        }

        private List<Tuple<string, double, int, int, int, int>> GetPlayerAverages(IEnumerable<Player> players, IEnumerable<WeeklyScore> scores, PlayerStats playerStats)
        {
            List<Tuple<string, double, int, int, int, int>> playerAverages = (from player in players
                                   let playerId = player.Id
                                   let playerScores = scores.Where(s1 => s1.Player.Id == playerId)
                                   let average = playerStats.GetPlayerAverage(player, playerScores)
                                   let totalPins = playerScores.Where(score => !score.Absent).Sum(score => score.Total)
                                   let totalGamesPlayer = playerScores.Count(score => !score.Absent) * 3
                                   let totalGamesBetween200And250 = GetTotalGamesBetweenScoreRange(playerScores, 200, 250)
                                   let totalGamesBetween250And300 = GetTotalGamesBetweenScoreRange(playerScores, 250, 300)
                                   select
                                       new Tuple<string, double, int, int, int, int>(player.Name,
                                                                                     average,
                                                                                     totalPins,
                                                                                     totalGamesPlayer,
                                                                                     totalGamesBetween200And250,
                                                                                     totalGamesBetween250And300)).ToList();
            return playerAverages;
        }

        private List<Tuple<string, double>> GetTeamAveragesForFirstWeek(IEnumerable<Player> players, IEnumerable<Team> teams)
        {
            return (from team in teams
                    let playerAverages = players.Where(player => player.Team.Name == team.Name).Sum(player => player.InitialAverage)
                    select new Tuple<string, double>(team.Name, playerAverages)).ToList();
        }

        private List<Tuple<string, double>> GetTeamAverages(IEnumerable<WeeklyScore> scores, IEnumerable<Team> teams, IEnumerable<Player> players)
        {
            // 4 = players 
            //team name, average
            List<Tuple<string, double>> teamRecord = new List<Tuple<string, double>>();
            PlayerStats playerStats = new PlayerStats();

            foreach (Team team in teams)
            {
                string teamName = team.Name;
                IEnumerable<Player> playersOnTeam = players.Where(p1 => p1.Team.Name == teamName);
                double avg = playersOnTeam.Sum(player => playerStats.GetPlayerAverage(player, scores));
                avg = Math.Round(avg, 1);
                Tuple<string, double> record = new Tuple<string, double>(team.Name, avg);
                teamRecord.Add(record);
            }

            return teamRecord;
        }



        private int GetTotalGamesBetweenScoreRange(IEnumerable<WeeklyScore> playerScores, int lowBound, int highBound)
        {
            int total = 0;

            foreach (WeeklyScore score in playerScores.Where(score => score.Week.WeekNumber != 0 && !score.Absent))
            {
                if (score.Game1 >= lowBound && score.Game1 < highBound)
                {
                    total++;
                }
                if (score.Game2 >= lowBound && score.Game2 < highBound)
                {
                    total++;
                }
                if (score.Game3 >= lowBound && score.Game3 < highBound)
                {
                    total++;
                }
            }

            return total;
        }
    }
}