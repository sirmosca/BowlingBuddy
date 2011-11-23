using System.Collections.Generic;
using System.Reflection;
using BowlingStats.Model;
using Microsoft.Office.Interop.Excel;

namespace BowlingStats.BusinessLogic
{
    /// <summary>
    /// print matchups report to excel
    /// </summary>
    public class PrintMatchupsReport
    {
        /// <summary>
        /// Prints the matchups report.
        /// </summary>
        /// <param name="matchupReportLineItems"></param>
        public void Print(IEnumerable<MatchupReportLineItem> matchupReportLineItems)
        {
            Application excelApplication = new Application { Visible = true };
            Workbook excelWorkbook = excelApplication.Workbooks.Add(1);
            int workSheetCounter = 1;

            foreach (MatchupReportLineItem matchupReportLineItem in matchupReportLineItems)
            {
                int rowIndex = 1;
                MatchupReportTeamData matchupReportData = matchupReportLineItem.MatchupReportData;
                excelWorkbook.Sheets.Add(Missing.Value, excelApplication.ActiveWorkbook.Worksheets[excelApplication.ActiveWorkbook.Worksheets.Count], 1, Missing.Value);
                Worksheet excelWorksheet = (Worksheet)excelWorkbook.Sheets[workSheetCounter];

                WriteData(
                    excelWorksheet,
                    matchupReportData.Team1Players,
                    matchupReportLineItem.PlayerAveragesDictionary,
                    matchupReportData.Team1.Name,
                    matchupReportData.Team1Average,
                    matchupReportData.Team1Handicap,
                    ref rowIndex);

                rowIndex++;

                WriteData(
                    excelWorksheet,
                    matchupReportData.Team2Players,
                    matchupReportLineItem.PlayerAveragesDictionary,
                    matchupReportData.Team2.Name,
                    matchupReportData.Team2Average,
                    matchupReportData.Team2Handicap,
                    ref rowIndex);

                workSheetCounter++;
            }
        }

        private void WriteData(
            _Worksheet excelWorksheet,
            IEnumerable<Player> players,
            IDictionary<Player, double> playerAveragesDictionary,
            string teamName,
            double teamAverage,
            double teamHandicap,
            ref int rowIndex)
        {
            excelWorksheet.Cells[rowIndex, 1] = teamName;
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
                excelWorksheet.Cells[rowIndex, 2] = playerAveragesDictionary[player];
                rowIndex++;
            }

            excelWorksheet.Cells[rowIndex, 1] = "Team";
            excelWorksheet.Cells[rowIndex, 2] = teamAverage;
            rowIndex += 2;
            excelWorksheet.Cells[rowIndex, 1] = "Handicap";
            excelWorksheet.Cells[rowIndex, 2] = teamHandicap;
            rowIndex += 2;
            excelWorksheet.Cells[rowIndex, 1] = "Total";
            excelWorksheet.Cells[rowIndex, 2] = teamHandicap + teamAverage;
            rowIndex += 2;
        }
    }
}