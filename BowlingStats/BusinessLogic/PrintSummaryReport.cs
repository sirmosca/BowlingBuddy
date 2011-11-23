using System.Collections.Generic;
using BowlingStats.Model;
using Microsoft.Office.Interop.Excel;
using System.Linq;

namespace BowlingStats.BusinessLogic
{
    public class PrintSummaryReport
    {
        public void Print(SummaryReportData summaryReportData, Week currentWeek)
        {
            Application excelApplication = new Application {Visible = true};
            Workbook excelWorkbook = excelApplication.Workbooks.Add(1);
            Worksheet excelWorksheet = (Worksheet) excelWorkbook.Sheets[1];
            int rowIndex = 1;

            excelWorksheet.Cells[rowIndex, 1] = "Strikers Bowling League";
            rowIndex += 2;
            excelWorksheet.Cells[rowIndex, 1] = "Team Standings";
            rowIndex++;
            excelWorksheet.Cells[rowIndex, 1] = "Team Name";
            excelWorksheet.Cells[rowIndex, 2] = "Points"; 
            rowIndex++;

            foreach (Team team in summaryReportData.Teams)
            {
                if (team.Name == "Sub") continue;
                List<WeeklyPoint> pointsPriorToThisWeek =
                    team.WeeklyPoints.Where(p => p.Week.WeekNumber < currentWeek.WeekNumber).ToList();
                double totalPoints = pointsPriorToThisWeek.Sum(p => p.Points);
                excelWorksheet.Cells[rowIndex, 1] = team.Name;
                excelWorksheet.Cells[rowIndex, 2] = totalPoints;
                rowIndex++;
            }

            rowIndex += 5;

            excelWorksheet.Cells[rowIndex, 1] = "Last Week";
            excelWorksheet.Cells[rowIndex, 4] = "Year to Date";
            rowIndex++;
            WriteIndividualHighGame(excelWorksheet, ref rowIndex, summaryReportData.LastWeekMensHighGames,
                                    summaryReportData.OverallMensHighGames, "M");
            rowIndex++;
            WriteIndividualHighGame(excelWorksheet, ref rowIndex, summaryReportData.LastWeekWomensHighGames,
                                    summaryReportData.OverallWomensHighGames, "F");
            rowIndex++;
            WriteIndividualHighSeries(excelWorksheet, ref rowIndex, summaryReportData.LastWeekMensHighSeries,
                                      summaryReportData.OverallMensHighSeries, "M");
            rowIndex++;
            WriteIndividualHighSeries(excelWorksheet, ref rowIndex, summaryReportData.LastWeekWomensHighSeries,
                                      summaryReportData.OverallWomensHighSeries, "F");
        }

        private void WriteIndividualHighSeries(Worksheet excelWorksheet, ref int rowIndex,
                                               List<Point> lastWeekHighSeries, List<Point> overallHighSeries,
                                               string gender)
        {
            excelWorksheet.Cells[rowIndex, 1] = "Name";
            excelWorksheet.Cells[rowIndex, 2] = "High Series";
            excelWorksheet.Cells[rowIndex, 3] = gender == "M" ? "Men" : "Women";
            excelWorksheet.Cells[rowIndex, 4] = "Name";
            excelWorksheet.Cells[rowIndex, 5] = "High Series";
            rowIndex++;

            for (int i = 0; i < lastWeekHighSeries.Count; i++)
            {
                excelWorksheet.Cells[rowIndex, 1] = lastWeekHighSeries[i].Name;
                excelWorksheet.Cells[rowIndex, 2] = lastWeekHighSeries[i].Points;
                excelWorksheet.Cells[rowIndex, 4] = overallHighSeries[i].Name;
                excelWorksheet.Cells[rowIndex, 5] = overallHighSeries[i].Points;
                rowIndex++;
            }
        }

        private void WriteIndividualHighGame(Worksheet excelWorksheet, ref int rowIndex, List<Point> lastWeekHighGames,
                                             List<Point> overallHighGame, string gender)
        {
            excelWorksheet.Cells[rowIndex, 1] = "Name";
            excelWorksheet.Cells[rowIndex, 2] = "High Game";
            excelWorksheet.Cells[rowIndex, 3] = gender == "M" ? "Men" : "Women";
            excelWorksheet.Cells[rowIndex, 4] = "Name";
            excelWorksheet.Cells[rowIndex, 5] = "High Game";
            rowIndex++;

            for (int i = 0; i < lastWeekHighGames.Count; i++)
            {
                excelWorksheet.Cells[rowIndex, 1] = lastWeekHighGames[i].Name;
                excelWorksheet.Cells[rowIndex, 2] = lastWeekHighGames[i].Points;
                excelWorksheet.Cells[rowIndex, 4] = overallHighGame[i].Name;
                excelWorksheet.Cells[rowIndex, 5] = overallHighGame[i].Points;
                rowIndex++;
            }
        }
    }
}