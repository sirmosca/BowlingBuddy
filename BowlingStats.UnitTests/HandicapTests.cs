using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BowlingStats.BusinessLogic;
using BowlingStats.Model;
using FakeItEasy;
using NUnit.Framework;

namespace BowlingStats.UnitTests
{
    [TestFixture]
    public class HandicapTests
    {
        [Test]
        public void given_week_one_when_determining_team_handicap_then_use_initial_averages()
        {
            Team team1 = CreateTeam("team 1");
            Team team2 = CreateTeam("team 2");
            Player player1 = CreatePlayer("M", 1, "player 1", team1, 100);
            Player player2 = CreatePlayer("M", 2, "player 2", team1, 100);
            Player player3 = CreatePlayer("M", 3, "player 3", team2, 110);
            Player player4 = CreatePlayer("M", 4, "player 4", team2, 110);
            List<Player> team1Players = new List<Player>() {player1, player2};
            List<Player> team2Players = new List<Player>() {player3, player4};
            Handicap handicap = new Handicap();
            TeamStats teamStats = new TeamStats();
            double team1Average = teamStats.GetTeamAverage(new List<WeeklyScore>(), team1Players);
            double team2Average = teamStats.GetTeamAverage(new List<WeeklyScore>(), team2Players);

            double team1Handicap = handicap.GetHandicap(team1Average, team2Average);
            double team2Handicap = handicap.GetHandicap(team2Average, team1Average);

            Assert.That(team1Handicap, Is.EqualTo(20));
            Assert.That(team2Handicap, Is.EqualTo(0));
        }

        [Test]
        public void given_week_two_when_determining_team_handicap_then_use_current_averages()
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
            WeeklyScore scoreForPlayer1 = CreateWeeklyScore(100, 110, 120, player1, player1.Team, week, false);
            WeeklyScore scoreForPlayer2 = CreateWeeklyScore(100, 110, 120, player2, player2.Team, week, false);
            WeeklyScore scoreForPlayer3 = CreateWeeklyScore(200, 210, 220, player3, player3.Team, week, false);
            WeeklyScore scoreForPlayer4 = CreateWeeklyScore(200, 210, 220, player4, player4.Team, week, false);
            List<WeeklyScore> scores = new List<WeeklyScore> { scoreForPlayer1, scoreForPlayer2, scoreForPlayer3, scoreForPlayer4 };
            
            Handicap handicap = new Handicap();
            TeamStats teamStats = new TeamStats();
            double team1Average = teamStats.GetTeamAverage(scores, playersOnTeam1);
            double team2Average = teamStats.GetTeamAverage(scores, playersOnTeam2);

            double team1Handicap = handicap.GetHandicap(team1Average, team2Average);
            double team2Handicap = handicap.GetHandicap(team2Average, team1Average);

            Assert.That(team1Handicap, Is.EqualTo(200));
            Assert.That(team2Handicap, Is.EqualTo(0));
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

        private WeeklyScore CreateWeeklyScore(int game1, int game2, int game3, Player player, Team team, Week week, bool absent)
        {
            WeeklyScore score = A.Fake<WeeklyScore>();
            score.Game1 = game1;
            score.Game2 = game2;
            score.Game3 = game3;
            score.Player = player;
            score.Team = team;
            score.Week = week;
            score.Absent = absent;

            return score;
        }
    }
}
