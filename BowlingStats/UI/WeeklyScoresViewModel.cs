using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BowlingStats.BusinessLogic;
using BowlingStats.Model;
using BowlingStats.ViewModel;
using Caliburn.Micro;

namespace BowlingStats.UI
{
    public class WeeklyScoresViewModel : PropertyChangedBase
    {
        private readonly List<WeeklyScore> _allScores;
        private readonly List<Player> _players;
        private readonly List<Team> _teams;
        private List<GridPlayerViewModel> _allGridPlayers;
        private ObservableCollection<GridPlayerViewModel> _awayTeamPlayers;
        private ObservableCollection<Team> _awayTeams;
        private ObservableCollection<GridPlayerViewModel> _homeTeamPlayers;
        private ObservableCollection<Team> _homeTeams;
        private List<WeeklyMatchup> _matchups;
        private Team _selectedAwayTeam;
        private Team _selectedHomeTeam;
        private Week _selectedWeek;
        private ObservableCollection<Week> _weeks;

        public WeeklyScoresViewModel(IEnumerable<Week> weeks,
                                     List<Team> teams,
                                     List<Player> players,
                                     List<WeeklyMatchup> matchups,
                                     List<WeeklyScore> allScores)
        {
            Weeks = new ObservableCollection<Week>(weeks);
            HomeTeams = new ObservableCollection<Team>(teams);
            AwayTeams = new ObservableCollection<Team>(teams);
            _allScores = allScores;
            _matchups = matchups;
            _players = players;
            _teams = teams;
        }

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

        public bool CanSaveScores
        {
            get { return SelectedWeek != null && SelectedHomeTeam != null && SelectedAwayTeam != null; }
        }

        public bool CanPrintSummary
        {
            get { return SelectedWeek != null && SelectedWeek.WeekNumber > 1; }
        }

        public bool IsWeekSelected { get { return SelectedWeek != null; } }

        public Week SelectedWeek
        {
            get { return _selectedWeek; }
            set
            {
                _selectedWeek = value;
                _allGridPlayers = CreateAllGridPlayers(value, _allScores);
                NotifyOfPropertyChange(() => SelectedWeek);
                NotifyOfPropertyChange(() => CanSaveScores);
                NotifyOfPropertyChange(() => CanPrintSummary);
                NotifyOfPropertyChange(() => CanPrintAverages);
                NotifyOfPropertyChange(() => CanPrintMatchups);
                NotifyOfPropertyChange(() => IsWeekSelected);
            }
        }

        public bool CanPrintMatchups
        {
            get { return SelectedWeek != null; }
        }

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

        public bool CanPrintAverages
        {
            get { return SelectedWeek != null; }
        }

        public Team SelectedHomeTeam
        {
            get { return _selectedHomeTeam; }
            set
            {
                _selectedHomeTeam = value;
                IEnumerable<GridPlayerViewModel> homeTeams = _allGridPlayers.Where(t => t.TeamName.Equals(value.Name));
                HomeTeamPlayers = new ObservableCollection<GridPlayerViewModel>(homeTeams);
                NotifyOfPropertyChange(() => SelectedHomeTeam);
            }
        }

        public Team SelectedAwayTeam
        {
            get { return _selectedAwayTeam; }
            set
            {
                _selectedAwayTeam = value;
                IEnumerable<GridPlayerViewModel> awayTeams = _allGridPlayers.Where(t => t.TeamName.Equals(value.Name));
                AwayTeamPlayers = new ObservableCollection<GridPlayerViewModel>(awayTeams);
                NotifyOfPropertyChange(() => SelectedAwayTeam);
            }
        }

        public ObservableCollection<GridPlayerViewModel> HomeTeamPlayers
        {
            get { return _homeTeamPlayers; }
            set
            {
                _homeTeamPlayers = value;
                NotifyOfPropertyChange(() => HomeTeamPlayers);
            }
        }

        public ObservableCollection<GridPlayerViewModel> AwayTeamPlayers
        {
            get { return _awayTeamPlayers; }
            set
            {
                _awayTeamPlayers = value;
                NotifyOfPropertyChange(() => AwayTeamPlayers);
            }
        }

        public ObservableCollection<Team> HomeTeams
        {
            get { return _homeTeams; }
            set
            {
                _homeTeams = value;
                NotifyOfPropertyChange(() => HomeTeams);
            }
        }

        public ObservableCollection<Team> AwayTeams
        {
            get { return _awayTeams; }
            set
            {
                _awayTeams = value;
                NotifyOfPropertyChange(() => AwayTeams);
            }
        }

        public ObservableCollection<Week> Weeks
        {
            get { return _weeks; }
            set
            {
                _weeks = value;
                NotifyOfPropertyChange(() => Weeks);
            }
        }

        public void SaveWeeklyScores()
        {
            if (SelectedWeek == null)
            {
                return;
            }

            UpdateScoresForThisWeek();

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

        private void UpdateScoresForThisWeek()
        {
            var scores = (from gridPlayerViewModel in _allGridPlayers
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

            // remove selected week's scores because i just add what was just retrieved. 
            var scoresToRemove = _allScores.Where(weeklyScore => weeklyScore.Week.WeekNumber == SelectedWeek.WeekNumber).ToList();

            foreach (WeeklyScore weeklyScore in scoresToRemove)
            {
                _allScores.Remove(weeklyScore);
            }

            _allScores.AddRange(scores);
        }

        private List<GridPlayerViewModel> CreateAllGridPlayers(Week selectedWeek, IEnumerable<WeeklyScore> allScores)
        {
            List<WeeklyScore> scoresForWeek =
                allScores.Where(s => s.Week.WeekNumber == selectedWeek.WeekNumber).ToList();
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

            return gridPlayerViewModels;
        }
    }
}