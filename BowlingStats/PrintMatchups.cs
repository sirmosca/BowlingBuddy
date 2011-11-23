using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BowlingStats.BusinessLogic;
using BowlingStats.Model;
using Microsoft.Office.Interop.Excel;

namespace BowlingStats
{
    public class PrintMatchups
    {
        /// <summary>
        /// Prints the matchups for current week.
        /// </summary>
        /// <param name="weekId">The week id.</param>
        /// <param name="matchups">The matchups.</param>
        /// <param name="players">The players.</param>
        /// <param name="scoresToWeek">The scores to week.</param>
        /// <param name="scoresForWeek">The scores for week.</param>
        public void PrintMatchupsForCurrentWeek(int weekId, IEnumerable<WeeklyMatchup> matchups, IEnumerable<Player> players, List<WeeklyScore> scoresToWeek, IEnumerable<WeeklyScore> scoresForWeek)
        {
            Application excelApplication = new Application {Visible = true};
            Workbook excelWorkbook = excelApplication.Workbooks.Add(1);

            if (weekId == 1)
            {
                int workSheetCounter = 1;
                foreach (WeeklyMatchup matchup in matchups)
                {
                    WeeklyMatchup currentMatchup = matchup;
                    excelWorkbook.Sheets.Add(Missing.Value, excelApplication.ActiveWorkbook.Worksheets[excelApplication.ActiveWorkbook.Worksheets.Count], 1, Missing.Value);
                    Worksheet excelWorksheet = (Worksheet) excelWorkbook.Sheets[workSheetCounter];
                    List<WeeklyScore> teamOneScores = scoresForWeek.Where(s => s.Team.Name == currentMatchup.TeamOne.Name).ToList();
                    List<WeeklyScore> teamTwoScores = scoresForWeek.Where(s => s.Team.Name == currentMatchup.TeamTwo.Name).ToList();
                    List<Player> team1Players = GetTeamPlayers(currentMatchup.TeamOne.Name, teamOneScores, players);
                    List<Player> team2Players = GetTeamPlayers(currentMatchup.TeamTwo.Name, teamTwoScores, players);
                    Dictionary<int, double> playerAverageDictionary1 = team1Players.ToDictionary(player => player.Id, player => player.InitialAverage);
                    Dictionary<int, double> playerAverageDictionary2 = team2Players.ToDictionary(player => player.Id, player => player.InitialAverage);
                    double team1Average = team1Players.Sum(t1 => t1.InitialAverage);
                    double team2Average = team2Players.Sum(t2 => t2.InitialAverage);
                    double team1Handicap = team1Average < team2Average ? team2Average - team1Average : 0;
                    double team2Handicap = team2Average < team1Average ? team1Average - team2Average : 0;
                    int rowIndex = 1;

                    WriteTeamData(excelWorksheet, team1Players, playerAverageDictionary1, team1Handicap, ref rowIndex, team1Average);
                    rowIndex += 2;
                    WriteTeamData(excelWorksheet, team2Players, playerAverageDictionary2, team2Handicap, ref rowIndex, team2Average);
                    workSheetCounter++;
                }
            }
            else
            {
                PlayerStats playerStats = new PlayerStats();
                int workSheetCounter = 1;
                foreach (WeeklyMatchup matchup in matchups)
                {
                    WeeklyMatchup currentMatchup = matchup;
                    excelWorkbook.Sheets.Add(Missing.Value, excelApplication.ActiveWorkbook.Worksheets[excelApplication.ActiveWorkbook.Worksheets.Count], 1, Missing.Value);
                    Worksheet excelWorksheet = (Worksheet) excelWorkbook.Sheets[workSheetCounter];
                    List<WeeklyScore> teamOneScores = scoresForWeek.Where(s => s.Team.Name == currentMatchup.TeamOne.Name).ToList();
                    List<WeeklyScore> teamTwoScores = scoresForWeek.Where(s => s.Team.Name == currentMatchup.TeamTwo.Name).ToList();
                    List<Player> team1Players = GetTeamPlayers(currentMatchup.TeamOne.Name, teamOneScores, players);
                    List<Player> team2Players = GetTeamPlayers(currentMatchup.TeamTwo.Name, teamTwoScores, players);
                    Dictionary<int, double> playerAverageDictionary1 = team1Players.ToDictionary(player => player.Id,
                                                                                                 player =>
                                                                                                 playerStats.GetPlayerAverage(player,
                                                                                                                              scoresToWeek.FindAll(s => s.Player.Id == player.Id)));
                    Dictionary<int, double> playerAverageDictionary2 = team2Players.ToDictionary(player => player.Id,
                                                                                                 player =>
                                                                                                 playerStats.GetPlayerAverage(player,
                                                                                                                              scoresToWeek.FindAll(s => s.Player.Id == player.Id)));
                    double team1Average = playerAverageDictionary1.Sum(keyValuePair => keyValuePair.Value);
                    double team2Average = playerAverageDictionary2.Sum(keyValuePair => keyValuePair.Value);
                    double team1Handicap = team1Average < team2Average ? team2Average - team1Average : 0;
                    double team2Handicap = team2Average < team1Average ? team1Average - team2Average : 0;

                    int rowIndex = 1;
                    WriteTeamData(excelWorksheet, team1Players, playerAverageDictionary1, team1Handicap, ref rowIndex, team1Average);
                    rowIndex += 2;
                    WriteTeamData(excelWorksheet, team2Players, playerAverageDictionary2, team2Handicap, ref rowIndex, team2Average);
                    workSheetCounter++;
                }
            }
        }

        private List<Player> GetTeamPlayers(string teamName, ICollection<WeeklyScore> teamOneScores, IEnumerable<Player> players)
        {
            List<Player> teamPlayers;

            if (teamOneScores.Count > 0)
            {
                teamPlayers = (from teamOneScore in teamOneScores
                                from player in players
                                where teamOneScore.Player.Id == player.Id
                                select player).ToList();
            }
            else
            {
                teamPlayers = players.Where(p1 => p1.Team.Name == teamName).ToList();
            }

            return teamPlayers;
        }

        private void WriteTeamData(Worksheet excelWorksheet, IList<Player> players, IDictionary<int, double> playerAverages, double handicap, ref int rowIndex, double teamAverage)
        {
            excelWorksheet.Cells[rowIndex, 1] = players[0].Team.Name;
            rowIndex += 2;
            excelWorksheet.Cells[rowIndex, 1] = "Name";
            excelWorksheet.Cells[rowIndex, 2] = "Average";
            excelWorksheet.Cells[rowIndex, 3] = "Game 1";
            excelWorksheet.Cells[rowIndex, 4] = "Game 2";
            excelWorksheet.Cells[rowIndex, 5] = "Game 3";
            excelWorksheet.Cells[rowIndex, 6] = "Total";
            rowIndex += 2;

            foreach (Player player in players)
            {
                excelWorksheet.Cells[rowIndex, 1] = player.Name;
                excelWorksheet.Cells[rowIndex, 2] = playerAverages[player.Id];
                rowIndex++;
            }
            excelWorksheet.Cells[rowIndex, 1] = "Team";
            excelWorksheet.Cells[rowIndex, 2] = teamAverage;

            rowIndex += 2;
            excelWorksheet.Cells[rowIndex, 1] = "Handicap";
            excelWorksheet.Cells[rowIndex, 2] = handicap;
            rowIndex += 2;
            excelWorksheet.Cells[rowIndex, 1] = "Total";
            excelWorksheet.Cells[rowIndex, 2] = teamAverage + handicap;
        }
    }
}