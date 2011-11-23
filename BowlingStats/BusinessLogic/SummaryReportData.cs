using System.Collections.Generic;
using BowlingStats.Model;

namespace BowlingStats.BusinessLogic
{
    public class SummaryReportData
    {
        public SummaryReportData(List<Team> teams, List<Point> lastWeekMensHighSeries,
                                 List<Point> overallMensHighSeries, List<Point> lastWeekMensHighGames,
                                 List<Point> overallMensHighGames, List<Point> lastWeekWomensHighSeries,
                                 List<Point> overallWomensHighSeries, List<Point> lastWeekWomensHighGames,
                                 List<Point> overallWomensHighGames)
        {
            Teams = teams;
            LastWeekMensHighSeries = lastWeekMensHighSeries;
            OverallMensHighSeries = overallMensHighSeries;
            LastWeekMensHighGames = lastWeekMensHighGames;
            OverallMensHighGames = overallMensHighGames;
            LastWeekWomensHighSeries = lastWeekWomensHighSeries;
            OverallWomensHighSeries = overallWomensHighSeries;
            LastWeekWomensHighGames = lastWeekWomensHighGames;
            OverallWomensHighGames = overallWomensHighGames;
        }

        public List<Team> Teams { get; private set; }

        public List<Point> LastWeekMensHighSeries { get; private set; }

        public List<Point> OverallMensHighSeries { get; private set; }

        public List<Point> LastWeekMensHighGames { get; private set; }

        public List<Point> OverallMensHighGames { get; private set; }

        public List<Point> LastWeekWomensHighSeries { get; private set; }

        public List<Point> OverallWomensHighSeries { get; private set; }

        public List<Point> LastWeekWomensHighGames { get; private set; }

        public List<Point> OverallWomensHighGames { get; private set; }
    }
}