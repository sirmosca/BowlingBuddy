namespace BowlingStats.BusinessLogic
{
    public class TeamAveragesReportData
    {
        public TeamAveragesReportData(string teamName, double average)
        {
            TeamName = teamName;
            Average = average;
        }

        public string TeamName { get; private set; }

        public double Average { get; private set; }
    }
}