using System.Collections.Generic;
using BowlingStats.BusinessLogic;
using BowlingStats.Model;
using Microsoft.Office.Interop.Excel;

namespace BowlingStats
{
    /// <summary>
    /// 
    /// </summary>
    public class PrintSummaries
    {
        /// <summary>
        /// Prints the summary.
        /// </summary>
        /// <param name="weekId">The week id.</param>
        /// <param name="scoresForHandicap">The scores.</param>
        /// <param name="scoresIncludingSubs">The scores including subs.</param>
        /// <param name="teams">The teams.</param>
        /// <param name="matchups">The matchups.</param>
        /// <param name="players">The players.</param>
        public void PrintSummary(int weekId,
                                 List<WeeklyScore> scoresForHandicap,
                                 List<WeeklyScore> scoresIncludingSubs,
                                 List<Team> teams,
                                 List<WeeklyMatchup> matchups,
                                 List<Player> players)
        {
            Application excelApplication = new Application {Visible = true};
            Workbook excelWorkbook = excelApplication.Workbooks.Add(1);
            Worksheet excelWorksheet = (Worksheet) excelWorkbook.Sheets[1];
            TeamStats teamStats = new TeamStats();
            PlayerStats playerStats = new PlayerStats();
            int rowIndex = 1;

            excelWorksheet.Cells[rowIndex, 1] = "Strikers Bowling League";
            rowIndex += 2;
            excelWorksheet.Cells[rowIndex, 1] = "Team Standings";
            rowIndex++;
            excelWorksheet.Cells[rowIndex, 1] = "Team Name";
            excelWorksheet.Cells[rowIndex, 2] = "Points";
            rowIndex++;

            List<Points> teamPoints = teamStats.GetTeamPoints(scoresForHandicap, scoresIncludingSubs, teams, matchups, players);
            teamPoints.Sort((t1, t2) => Comparer<double>.Default.Compare(t2.TotalPoints, t1.TotalPoints));

            foreach (Points teamPoint in teamPoints)
            {
                excelWorksheet.Cells[rowIndex, 1] = teamPoint.Name;
                excelWorksheet.Cells[rowIndex, 2] = teamPoint.TotalPoints;
                rowIndex++;
            }

            rowIndex += 5;
            excelWorksheet.Cells[rowIndex, 1] = "Last Week";
            excelWorksheet.Cells[rowIndex, 4] = "Year to Date";
            rowIndex++;
            WriteIndividualHighGame(excelWorksheet, ref rowIndex, scoresForHandicap, weekId, "M", playerStats);
            rowIndex++;
            WriteIndividualHighGame(excelWorksheet, ref rowIndex, scoresForHandicap, weekId, "F", playerStats);
            rowIndex++;
            WriteIndividualHighSeries(excelWorksheet, ref rowIndex, scoresForHandicap, weekId, "M", playerStats);
            rowIndex++;
            WriteIndividualHighSeries(excelWorksheet, ref rowIndex, scoresForHandicap, weekId, "F", playerStats);
        }

        private void WriteIndividualHighSeries(Worksheet excelWorksheet, ref int rowIndex, List<WeeklyScore> scores, int weekId, string gender, PlayerStats playerStats)
        {
            excelWorksheet.Cells[rowIndex, 1] = "Name";
            excelWorksheet.Cells[rowIndex, 2] = "High Series";
            excelWorksheet.Cells[rowIndex, 3] = gender == "M" ? "Men" : "Women";
            excelWorksheet.Cells[rowIndex, 4] = "Name";
            excelWorksheet.Cells[rowIndex, 5] = "High Series";
            rowIndex++;
            List<WeeklyScore> lastWeekScores = scores.FindAll(s1 => s1.Week.WeekNumber == weekId - 1);
            List<Points> lastWeekHighTotalPoints = new List<Points>();// playerStats.GetHighTotalGamePoints(lastWeekScores, gender);
            List<Points> malesHighTotalPoints = new List<Points>();//.GetHighTotalGamePoints(scores, gender);

            for (int i = 0; i < 3; i++)
            {
                excelWorksheet.Cells[rowIndex, 1] = lastWeekHighTotalPoints[i].Name;
                excelWorksheet.Cells[rowIndex, 2] = lastWeekHighTotalPoints[i].TotalPoints;
                excelWorksheet.Cells[rowIndex, 4] = malesHighTotalPoints[i].Name;
                excelWorksheet.Cells[rowIndex, 5] = malesHighTotalPoints[i].TotalPoints;
                rowIndex++;
            }
        }

        private void WriteIndividualHighGame(Worksheet excelWorksheet, ref int rowIndex, List<WeeklyScore> scores, int weekId, string gender, PlayerStats playerStats)
        {
            excelWorksheet.Cells[rowIndex, 1] = "Name";
            excelWorksheet.Cells[rowIndex, 2] = "High Game";
            excelWorksheet.Cells[rowIndex, 3] = gender == "M" ? "Men" : "Women";
            excelWorksheet.Cells[rowIndex, 4] = "Name";
            excelWorksheet.Cells[rowIndex, 5] = "High Game";
            rowIndex++;
            List<WeeklyScore> lastWeekScores = scores.FindAll(s1 => s1.Week.WeekNumber == weekId - 1);
            List<Points> lastWeekHighPoints = new List<Points>();// playerStats.GetHighGamePoints(lastWeekScores, gender);
            List<Points> overallHighPoints = new List<Points>();// playerStats.GetHighGamePoints(scores, gender);

            for (int i = 0; i < 3; i++)
            {
                excelWorksheet.Cells[rowIndex, 1] = lastWeekHighPoints[i].Name;
                excelWorksheet.Cells[rowIndex, 2] = lastWeekHighPoints[i].TotalPoints;
                excelWorksheet.Cells[rowIndex, 4] = overallHighPoints[i].Name;
                excelWorksheet.Cells[rowIndex, 5] = overallHighPoints[i].TotalPoints;
                rowIndex++;
            }
        }
    }
}