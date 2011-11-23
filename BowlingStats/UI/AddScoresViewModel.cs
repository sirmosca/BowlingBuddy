using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using BowlingStats.BusinessLogic;
using BowlingStats.Model;
using BowlingStats.ViewModel;
using Caliburn.Micro;

namespace BowlingStats.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class AddScoresViewModel : PropertyChangedBase
    {
        private List<WeeklyScore> _allScores;
        private ObservableCollection<GridPlayerViewModel> _gridPlayers;
        private List<WeeklyMatchup> _matchups;
        private List<Player> _players;
        private Week _selectedWeek;
        private List<Team> _teams;
        private List<Week> _weeks;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddScoresViewModel"/> class.
        /// </summary>
        /// <param name="weeks">The weeks.</param>
        /// <param name="players">The players.</param>
        /// <param name="teams">The teams.</param>
        /// <param name="matchups">The matchups.</param>
        /// <param name="allScores">All scores.</param>
        public AddScoresViewModel(List<Week> weeks, List<Player> players, List<Team> teams, List<WeeklyMatchup> matchups, List<WeeklyScore> allScores)
        {
            _weeks = weeks;
            _players = players;
            _teams = teams;
            _matchups = matchups;
            _allScores = allScores;

            InitializeGridPlayers(new List<GridPlayerViewModel>());
        }

        /// <summary>
        /// Determines whether this instance [can save scores].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance [can save scores]; otherwise, <c>false</c>.
        /// </returns>
        public bool CanSaveScores
        {
            get { return SelectedWeek != null; }
        }

        /// <summary>
        /// Gets or sets the grid players.
        /// </summary>
        /// <value>
        /// The grid players.
        /// </value>
        public ObservableCollection<GridPlayerViewModel> GridPlayers
        {
            get { return _gridPlayers; }
            set
            {
                _gridPlayers = value;
                NotifyOfPropertyChange(() => GridPlayers);
            }
        }

        /// <summary>
        /// Gets or sets the selected week.
        /// </summary>
        /// <value>The selected week.</value>
        public Week SelectedWeek
        {
            get { return _selectedWeek; }
            set
            {
                _selectedWeek = value;
                LoadScoresForSelectedWeek(value);
                NotifyOfPropertyChange(() => SelectedWeek);
                NotifyOfPropertyChange(() => CanSaveScores);
                NotifyOfPropertyChange(() => CanPrintSummary);
                NotifyOfPropertyChange(() => CanPrintAverages);
                NotifyOfPropertyChange(() => CanPrintMatchups);
            }
        }

        /// <summary>
        /// Gets the weeks.
        /// </summary>
        /// <value>The weeks.</value>
        public List<Week> Weeks
        {
            get { return _weeks; }
        }

        /// <summary>
        /// Determines whether this instance [can print averages].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance [can print averages]; otherwise, <c>false</c>.
        /// </returns>
        public bool CanPrintAverages
        {
            get { return SelectedWeek != null; }
        }

        /// <summary>
        /// Determines whether this instance [can print matchups].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance [can print matchups]; otherwise, <c>false</c>.
        /// </returns>
        public bool CanPrintMatchups
        {
            get { return SelectedWeek != null; }
        }

        /// <summary>
        /// Determines whether this instance [can print summary].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance [can print summary]; otherwise, <c>false</c>.
        /// </returns>
        public bool CanPrintSummary
        {
            get { return SelectedWeek != null && SelectedWeek.WeekNumber > 1; }
        }

        /// <summary>
        /// Prints the averages.
        /// </summary>
        public void PrintAverages()
        {
            PlayerStats playerAverage = new PlayerStats();

            List<WeeklyScore> scoresPriorToThisWeek = _allScores.Where(s => s.Week.WeekNumber < SelectedWeek.WeekNumber).ToList();
            List<PlayerAveragesReportData> playerAveragesReportDatas = (from player in _players
                                                                        let average = playerAverage.GetPlayerAverage(player, scoresPriorToThisWeek)
                                                                        let totalPins = playerAverage.GetTotalPinsForPlayer(player, scoresPriorToThisWeek)
                                                                        let totalGames = playerAverage.GetTotalGamesForPlayer(player, scoresPriorToThisWeek)
                                                                        let gamesBetween200And250 = playerAverage.GetTotalGamesWithPinsBetween200And250ForPlayer(player, scoresPriorToThisWeek)
                                                                        let gamesGreaterThanOrEqualTo250 = playerAverage.GetTotalGamesWithPinsOver250ForPlayer(player, scoresPriorToThisWeek)
                                                                        select new PlayerAveragesReportData(player.Name, average, totalPins, totalGames, gamesBetween200And250, gamesGreaterThanOrEqualTo250, player.Team.Name)).ToList();

            List<TeamAveragesReportData> teamAveragesReportData = (from currentTeam in _teams
                                                                   let playersForTeam = _players.Where(p => p.Team.Equals(currentTeam)).ToList()
                                                                   let teamAverage = playersForTeam.Sum(player => playerAverage.GetPlayerAverage(player, scoresPriorToThisWeek))
                                                                   where currentTeam.Name != "Sub"
                                                                   select new TeamAveragesReportData(currentTeam.Name, teamAverage)).ToList();

            new PrintAveragesReport().Print(playerAveragesReportDatas, teamAveragesReportData);
        }

        /// <summary>
        /// Prints the summary.
        /// </summary>
        public void PrintSummary()
        {
            PlayerStats playerStats = new PlayerStats();
            List<WeeklyScore> lastWeeksScores =
                _allScores.Where(s => s.Week.WeekNumber == SelectedWeek.WeekNumber - 1).ToList();
            List<Point> lastWeekMensHighSeries = playerStats.GetTop3HighSeriesPlayers(lastWeeksScores, "M");
            List<Point> overallMensHighSeries = playerStats.GetTop3HighSeriesPlayers(_allScores, "M");
            List<Point> lastWeekMensHighGames = playerStats.GetTop3HighGamePlayers(lastWeeksScores, "M");
            List<Point> overallMensHighGames = playerStats.GetTop3HighGamePlayers(_allScores, "M");
            List<Point> lastWeekWomensHighSeries = playerStats.GetTop3HighSeriesPlayers(lastWeeksScores, "F");
            List<Point> overallWomensHighSeries = playerStats.GetTop3HighSeriesPlayers(_allScores, "F");
            List<Point> lastWeekWomensHighGames = playerStats.GetTop3HighGamePlayers(lastWeeksScores, "F");
            List<Point> overallWomensHighGames = playerStats.GetTop3HighGamePlayers(_allScores, "F");
            SummaryReportData summaryReportData = new SummaryReportData(_teams, lastWeekMensHighSeries,
                                                                        overallMensHighSeries, lastWeekMensHighGames,
                                                                        overallMensHighGames, lastWeekWomensHighSeries,
                                                                        overallWomensHighSeries, lastWeekWomensHighGames,
                                                                        overallWomensHighGames);

            new PrintSummaryReport().Print(summaryReportData, SelectedWeek);
        }

        /// <summary>
        /// Prints the matchups.
        /// </summary>
        public void PrintMatchups()
        {
            MatchupsData getPrintMatchupsData = new MatchupsData();
            PlayerStats playerAverage = new PlayerStats();
            List<MatchupReportLineItem> lineItems = (from weeklyMatchup in _matchups
                                                     where weeklyMatchup.Week.WeekNumber == SelectedWeek.WeekNumber
                                                     select weeklyMatchup
                                                     into matchup 
                                                     let team1Players = _players.Where(p => p.Team.Equals(matchup.TeamOne)).ToList()
                                                     let team2Players = _players.Where(p => p.Team.Equals(matchup.TeamTwo)).ToList()
                                                     let matchupReportData =
                                                         getPrintMatchupsData.GetTeamDataForMatchup(team1Players, team2Players, _allScores, matchup.Week.WeekNumber)
                                                     let playerAveragesDictionary =
                                                         team1Players.Union(team2Players).ToDictionary(
                                                             p => p, p => playerAverage.GetPlayerAverage(p, _allScores, SelectedWeek.WeekNumber - 1))
                                                     select new MatchupReportLineItem(matchupReportData, playerAveragesDictionary)).ToList();

            new PrintMatchupsReport().Print(lineItems);
        }

        public void SaveScores()
        {
            SaveScores(SelectedWeek);
            EventAggregationProvider.EventAggregator.Publish(_allScores);
        }

        private void GridPlayersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems == null)
            {
                return;
            }

            foreach (GridPlayerViewModel model in from object newItem in e.NewItems select newItem as GridPlayerViewModel)
            {
                model.Teams = new ObservableCollection<Team>(_teams);
                model.Players = new ObservableCollection<Player>(_players);
            }
        }

        private void InitializeGridPlayers(List<GridPlayerViewModel> viewModels)
        {
            GridPlayers = new ObservableCollection<GridPlayerViewModel>(viewModels);
            _gridPlayers.CollectionChanged += GridPlayersCollectionChanged;
        }

        private void LoadScoresForSelectedWeek(Week week)
        {
            List<WeeklyScore> scoresForWeek = _allScores.Where(s => s.Week.WeekNumber == week.WeekNumber).ToList();
            List<GridPlayerViewModel> gridPlayerViewModels;

            if (scoresForWeek.Count == 0)
            {
                gridPlayerViewModels = (from player in _players
                                        where player.Team.Name != "Sub"
                                        select
                                            new GridPlayerViewModel
                                                {
                                                    PlayerId = player.Id,
                                                    PlayerName = player.Name,
                                                    Players = new ObservableCollection<Player>(_players),
                                                    TeamName = player.Team.Name,
                                                    Teams = new ObservableCollection<Team>(_teams),
                                                    SelectedPlayer = player,
                                                    SelectedTeam = player.Team
                                                }).ToList();
            }
            else
            {
                gridPlayerViewModels =
                    scoresForWeek.Select(
                        weeklyScore =>
                        new GridPlayerViewModel
                            {
                                Absent = weeklyScore.Absent,
                                Game1 = weeklyScore.Game1,
                                Game2 = weeklyScore.Game2,
                                Game3 = weeklyScore.Game3,
                                PlayerId = weeklyScore.Player.Id,
                                PlayerName = weeklyScore.Player.Name,
                                Players = new ObservableCollection<Player>(_players),
                                SelectedPlayer = weeklyScore.Player,
                                SelectedTeam = weeklyScore.Team,
                                TeamName = weeklyScore.Team.Name,
                                Teams = new ObservableCollection<Team>(_teams)
                            }).ToList();
            }

            InitializeGridPlayers(gridPlayerViewModels);
        }

        private void SaveScores(Week week)
        {
            if (week == null)
            {
                return;
            }

            List<WeeklyScore> scores = (from gridPlayerViewModel in _gridPlayers
                                        let model = gridPlayerViewModel
                                        let player = _players.FirstOrDefault(p => p.Id == model.PlayerId)
                                        let team = _teams.FirstOrDefault(t => t.Name == model.TeamName)
                                        select
                                            new WeeklyScore
                                                {
                                                    Absent = gridPlayerViewModel.Absent,
                                                    Game1 = gridPlayerViewModel.Game1,
                                                    Game2 = gridPlayerViewModel.Game2,
                                                    Game3 = gridPlayerViewModel.Game3,
                                                    Player = player,
                                                    Team = team,
                                                    Week = SelectedWeek
                                                }).ToList();

            List<WeeklyScore> scoresToRemove = _allScores.Where(weeklyScore => weeklyScore.Week.WeekNumber == week.WeekNumber).ToList();

            foreach (WeeklyScore weeklyScore in scoresToRemove)
            {
                _allScores.Remove(weeklyScore);
            }

            _allScores.AddRange(scores);

            TeamPoints teamPoints = new TeamPoints();

            IEnumerable<WeeklyMatchup> thisWeeksMatchups = _matchups.Where(m => m.Week.Equals(SelectedWeek));

            foreach (WeeklyMatchup weeklyMatchup in thisWeeksMatchups)
            {
                WeeklyMatchup matchup = weeklyMatchup;
                Team team1 = _teams.Find(t => t.Equals(matchup.TeamOne));
                Team team2 = _teams.Find(t => t.Equals(matchup.TeamTwo));
                List<WeeklyScore> team1Scores = _allScores.Where(s => s.Team.Equals(team1)).ToList();
                List<WeeklyScore> team2Scores = _allScores.Where(s => s.Team.Equals(team2)).ToList();
                teamPoints.UpdatePointsForTeam(team1Scores, team2Scores, SelectedWeek, team1, team2, _players);
            }
        }
    }
}