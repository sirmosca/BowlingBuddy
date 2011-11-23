using System.Collections.Generic;
using BowlingStats.Model;
using Microsoft.Office.Interop.Excel;

namespace BowlingStats.BusinessLogic
{
    /// <summary>
    /// class to print schedule
    /// </summary>
    public class PrintScheduleReport
    {
        /// <summary>
        /// Prints the schedule report.
        /// </summary>
        /// <param name="matchups">The matchups.</param>
        public void Print(IEnumerable<WeeklyMatchup> matchups)
        {
            Application excelApplication = new Application { Visible = true };
            Workbook excelWorkbook = excelApplication.Workbooks.Add(1);
            Worksheet excelWorksheet = (Worksheet)excelWorkbook.Sheets[1];
            int rowIndex = 1;

            excelWorksheet.Cells[rowIndex, 1] = "Week #";
            excelWorksheet.Cells[rowIndex, 2] = "Week";
            excelWorksheet.Cells[rowIndex, 3] = "Team";
            excelWorksheet.Cells[rowIndex, 4] = "Team";
            rowIndex++;

            foreach (WeeklyMatchup weeklyMatchup in matchups)
            {
                excelWorksheet.Cells[rowIndex, 1] = weeklyMatchup.Week.WeekNumber;
                excelWorksheet.Cells[rowIndex, 2] = weeklyMatchup.Week.Description;
                excelWorksheet.Cells[rowIndex, 3] = weeklyMatchup.TeamOne.Name;
                excelWorksheet.Cells[rowIndex, 4] = weeklyMatchup.TeamTwo.Name;
                rowIndex++;
            }
        }
    }
}