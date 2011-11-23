using System.Collections.Generic;
using BowlingStats.BusinessLogic;
using BowlingStats.Model;
using FakeItEasy;
using NUnit.Framework;

namespace BowlingStats.UnitTests
{
    [TestFixture]
    public class MatchupsReportTeamDataTests
    {
        [Test]
        public void given_the_first_week_of_the_season_when_no_weeks_have_been_played_then_the_initial_averages_should_print()
        {
            Team team1 = CreateTeam("team1");
            Team team2 = CreateTeam("team2");
            Player player1 = CreatePlayer("M", 1, "player1", team1, 150);
            Player player2 = CreatePlayer("M", 2, "player2", team1, 150);
            Player player3 = CreatePlayer("M", 3, "player3", team2, 200);
            Player player4 = CreatePlayer("M", 4, "player4", team2, 200);
            List<Player> playersOnTeam1 = new List<Player> { player1, player2 };
            List<Player> playersOnTeam2 = new List<Player> { player3, player4 };
            MatchupReportTeamData printMatchupData = new MatchupsData().GetTeamDataForMatchup(playersOnTeam1, playersOnTeam2, new List<WeeklyScore>(), 1);

            Assert.That(printMatchupData.Team1Average, Is.EqualTo(300));
            Assert.That(printMatchupData.Team1Handicap, Is.EqualTo(100));

            Assert.That(printMatchupData.Team2Average, Is.EqualTo(400));
            Assert.That(printMatchupData.Team2Handicap, Is.EqualTo(0));
        }

        [Test]
        public void given_the_second_week_of_the_season_when_one_week_has_been_played_then_the_average_from_week_one_should_print()
        {
            Team team1 = CreateTeam("team1");
            Team team2 = CreateTeam("team2");
            Player player1 = CreatePlayer("M", 1, "player1", team1, 150);
            Player player2 = CreatePlayer("M", 2, "player2", team1, 150);
            Player player3 = CreatePlayer("M", 3, "player3", team2, 200);
            Player player4 = CreatePlayer("M", 4, "player4", team2, 200);
            List<Player> playersOnTeam1 = new List<Player> { player1, player2 };
            List<Player> playersOnTeam2 = new List<Player> { player3, player4 };
            Week week = CreateWeek(1, "week 1");
            WeeklyScore scoreForPlayer1 = CreateWeeklyScore(100, 110, 120, player1, player1.Team, week);
            WeeklyScore scoreForPlayer2 = CreateWeeklyScore(100, 110, 120, player2, player2.Team, week);
            WeeklyScore scoreForPlayer3 = CreateWeeklyScore(200, 210, 220, player3, player3.Team, week);
            WeeklyScore scoreForPlayer4 = CreateWeeklyScore(200, 210, 220, player4, player4.Team, week);
            List<WeeklyScore> scores = new List<WeeklyScore> { scoreForPlayer1, scoreForPlayer2, scoreForPlayer3, scoreForPlayer4 };
            MatchupReportTeamData printMatchupData = new MatchupsData().GetTeamDataForMatchup(playersOnTeam1, playersOnTeam2, scores, 2);

            Assert.That(printMatchupData.Team1Average, Is.EqualTo(220));
            Assert.That(printMatchupData.Team1Handicap, Is.EqualTo(200));

            Assert.That(printMatchupData.Team2Average, Is.EqualTo(420));
            Assert.That(printMatchupData.Team2Handicap, Is.EqualTo(0));
        }

        [Test]
        public void given_the_third_week_of_the_season_when_two_weeks_has_been_played_then_the_average_from_week_three_should_print()
        {
            Team team1 = CreateTeam("team1");
            Team team2 = CreateTeam("team2");
            Player player1 = CreatePlayer("M", 1, "player1", team1, 150);
            Player player2 = CreatePlayer("M", 2, "player2", team1, 150);
            Player player3 = CreatePlayer("M", 3, "player3", team2, 200);
            Player player4 = CreatePlayer("M", 4, "player4", team2, 200);
            List<Player> playersOnTeam1 = new List<Player> { player1, player2 };
            List<Player> playersOnTeam2 = new List<Player> { player3, player4 };
            Week week1 = CreateWeek(1, "week 1");
            Week week2 = CreateWeek(2, "week 2");
            WeeklyScore scoreForPlayer1Week1 = CreateWeeklyScore(106, 109, 165, player1, player1.Team, week1);
            WeeklyScore scoreForPlayer2Week1 = CreateWeeklyScore(225, 201, 195, player2, player2.Team, week1);
            WeeklyScore scoreForPlayer3Week1 = CreateWeeklyScore(106, 175, 164, player3, player3.Team, week1);
            WeeklyScore scoreForPlayer4Week1 = CreateWeeklyScore(106, 155, 204, player4, player4.Team, week1);
            WeeklyScore scoreForPlayer1Week2 = CreateWeeklyScore(156, 199, 204, player1, player1.Team, week2);
            WeeklyScore scoreForPlayer2Week2 = CreateWeeklyScore(122, 126, 165, player2, player2.Team, week2);
            WeeklyScore scoreForPlayer3Week2 = CreateWeeklyScore(197, 167, 175, player3, player3.Team, week2);
            WeeklyScore scoreForPlayer4Week2 = CreateWeeklyScore(100, 165, 201, player4, player4.Team, week2);
            List<WeeklyScore> scores = new List<WeeklyScore>
                {
                    scoreForPlayer1Week1,
                    scoreForPlayer2Week1,
                    scoreForPlayer3Week1,
                    scoreForPlayer4Week1,
                    scoreForPlayer1Week2,
                    scoreForPlayer2Week2,
                    scoreForPlayer3Week2,
                    scoreForPlayer4Week2
                };
            MatchupReportTeamData printMatchupData = new MatchupsData().GetTeamDataForMatchup(playersOnTeam1, playersOnTeam2, scores, 3);

            Assert.That(printMatchupData.Team1Average, Is.EqualTo(329));
            Assert.That(printMatchupData.Team1Handicap, Is.EqualTo(0));

            Assert.That(printMatchupData.Team2Average, Is.EqualTo(319));
            Assert.That(printMatchupData.Team2Handicap, Is.EqualTo(10));
        }

        private Player CreatePlayer(string gender, int id, string name, Team team, double initialAverage)
        {
            Player player = A.Fake<Player>();
            player.Gender = gender;
            player.Id = id;
            player.Name = name;
            player.Team = team;
            player.InitialAverage = initialAverage;
            return player;
        }

        private Team CreateTeam(string name)
        {
            Team team = A.Fake<Team>();
            team.Name = name;
            return team;
        }

        private Week CreateWeek(int weekNumber, string description)
        {
            Week week = A.Fake<Week>();
            week.WeekNumber = weekNumber;
            week.Description = description;

            return week;
        }

        private WeeklyScore CreateWeeklyScore(int game1, int game2, int game3, Player player, Team team, Week week)
        {
            WeeklyScore score = A.Fake<WeeklyScore>();
            score.Game1 = game1;
            score.Game2 = game2;
            score.Game3 = game3;
            score.Player = player;
            score.Team = team;
            score.Week = week;

            return score;
        }
    }
}