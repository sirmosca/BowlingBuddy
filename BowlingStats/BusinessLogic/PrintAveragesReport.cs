using System.Collections.Generic;
using System.Linq;
using Microsoft.Office.Interop.Excel;

namespace BowlingStats.BusinessLogic
{
    public class PrintAveragesReport
    {
        public void Print(IEnumerable<PlayerAveragesReportData> playerAveragesReportDatas,
                          List<TeamAveragesReportData> teamAveragesReportDatas)
        {
            Application excelApplication = new Application {Visible = true};
            Workbook excelWorkbook = excelApplication.Workbooks.Add(1);
            int rowIndex = 1;
            Worksheet excelWorksheet = (Worksheet) excelWorkbook.Sheets[1];
            excelWorksheet.Cells[rowIndex, 1] = "Rank";
            excelWorksheet.Cells[rowIndex, 2] = "Name";
            excelWorksheet.Cells[rowIndex, 3] = "Average";
            excelWorksheet.Cells[rowIndex, 4] = "Total Pins";
            excelWorksheet.Cells[rowIndex, 5] = "Games";
            excelWorksheet.Cells[rowIndex, 6] = @"200's";
            excelWorksheet.Cells[rowIndex, 7] = @"250's";
            excelWorksheet.Cells[rowIndex, 9] = "Team";
            excelWorksheet.Cells[rowIndex, 10] = "Average"; 
            rowIndex++;

            List<PlayerAveragesReportData> currentBowlers =
                playerAveragesReportDatas.Where(p => p.TeamName != "Sub").ToList();
            List<PlayerAveragesReportData> subBowlers =
                playerAveragesReportDatas.Where(p => p.TeamName == "Sub").ToList();
            
            PrintBowlerAverage(currentBowlers, ref rowIndex, excelWorksheet);
            rowIndex += 2;
            excelWorksheet.Cells[rowIndex, 2] = "Subs";
            rowIndex++;
            PrintBowlerAverage(subBowlers, ref rowIndex, excelWorksheet);
            rowIndex = 2;
            PrintTeamAverage(teamAveragesReportDatas, ref rowIndex, excelWorksheet);
        }

        private void PrintTeamAverage(List<TeamAveragesReportData> teamAveragesReportDatas,
                                      ref int rowIndex,
                                      Worksheet excelWorksheet)
        {
            teamAveragesReportDatas.Sort((t1, t2) => t2.Average.CompareTo(t1.Average));
            foreach (TeamAveragesReportData teamAveragesReportData in teamAveragesReportDatas)
            {
                excelWorksheet.Cells[rowIndex, 9] = teamAveragesReportData.TeamName;
                excelWorksheet.Cells[rowIndex, 10] = teamAveragesReportData.Average;
                rowIndex++;
            }
        }

        private void PrintBowlerAverage(List<PlayerAveragesReportData> bowlers,
                                        ref int rowIndex,
                                        Worksheet excelWorksheet)
        {
            bowlers.Sort((p1, p2) => p2.Average.CompareTo(p1.Average));
            for (int i = 0; i < bowlers.Count; i++)
            {
                PlayerAveragesReportData data = bowlers[i];
                excelWorksheet.Cells[rowIndex, 1] = i + 1;
                excelWorksheet.Cells[rowIndex, 2] = data.Name;
                excelWorksheet.Cells[rowIndex, 3] = data.Average;
                excelWorksheet.Cells[rowIndex, 4] = data.TotalPins;
                excelWorksheet.Cells[rowIndex, 5] = data.Games;
                excelWorksheet.Cells[rowIndex, 6] = data.GamesBetween200And250;
                excelWorksheet.Cells[rowIndex, 7] = data.GamesGreaterThanOrEqualTo250;
                rowIndex++;
            }
        }
    }
}