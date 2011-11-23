namespace BowlingStats.BusinessLogic
{
    public class PlayerAveragesReportData
    {
        public PlayerAveragesReportData(string name,
                                        double average,
                                        int totalPins,
                                        int games,
                                        int gamesBetween200And250,
                                        int gamesGreaterThanOrEqualTo250,
                                        string teamName)
        {
            Name = name;
            Average = average;
            TotalPins = totalPins;
            Games = games;
            GamesBetween200And250 = gamesBetween200And250;
            GamesGreaterThanOrEqualTo250 = gamesGreaterThanOrEqualTo250;
            TeamName = teamName;
        }

        public string TeamName { get; private set; }

        public string Name { get; private set; }

        public double Average { get; private set; }

        public int TotalPins { get; private set; }

        public int Games { get; private set; }

        public int GamesBetween200And250 { get; private set; }

        public int GamesGreaterThanOrEqualTo250 { get; private set; }
    }
}