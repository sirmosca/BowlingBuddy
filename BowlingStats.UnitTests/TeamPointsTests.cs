using System.Collections.Generic;
using System.Linq;
using BowlingStats.BusinessLogic;
using BowlingStats.Model;
using FakeItEasy;
using NUnit.Framework;

namespace BowlingStats.UnitTests
{
    [TestFixture]
    public class TeamPointsTests
    {
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
            team.WeeklyPoints = new List<WeeklyPoint>();
            return team;
        }

        private Week CreateWeek(int weekNumber, string description)
        {
            Week week = A.Fake<Week>();
            week.WeekNumber = weekNumber;
            week.Description = description;

            return week;
        }

        private WeeklyScore CreateWeeklyScore(int game1, int game2, int game3, Player player, Team team, Week week,
                                              bool absent)
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

        [Test]
        public void
            given_weekly_scores_for_each_team_when_determining_the_team_points_for_each_team_for_the_third_week_summary_report_then_assign_points_according_to_rules_using_week_1_averages_to_calculate_handicap
            ()
        {
            TeamPoints teamPoints = new TeamPoints();
            Week week1 = CreateWeek(1, "week1");
            Week week2 = CreateWeek(2, "week2");
            Team team1 = CreateTeam("team1");
            Team team2 = CreateTeam("team2");
            Player player1 = CreatePlayer("M", 1, "p1", team1, 150);
            Player player2 = CreatePlayer("M", 2, "p2", team1, 120);
            Player player3 = CreatePlayer("M", 3, "p3", team2, 150);
            Player player4 = CreatePlayer("M", 4, "p4", team2, 150);
            List<WeeklyScore> team1Scores = new List<WeeklyScore>();
            List<WeeklyScore> team2Scores = new List<WeeklyScore>();
            List<Player> players = new List<Player> {player1, player2, player3, player4};

            WeeklyScore score1 = CreateWeeklyScore(100, 150, 100, player1, team1, week1, false);
            WeeklyScore score2 = CreateWeeklyScore(120, 120, 120, player2, team1, week1, false);
            WeeklyScore score3 = CreateWeeklyScore(110, 110, 110, player3, team2, week1, false);
            WeeklyScore score4 = CreateWeeklyScore(130, 115, 130, player4, team2, week1, false);

            WeeklyScore score5 = CreateWeeklyScore(140, 160, 190, player1, team1, week2, false);
            WeeklyScore score6 = CreateWeeklyScore(120, 160, 190, player2, team1, week2, false);
            WeeklyScore score7 = CreateWeeklyScore(180, 150, 90, player3, team2, week2, false);
            WeeklyScore score8 = CreateWeeklyScore(190, 180, 160, player4, team2, week2, false);

            team1Scores.Add(score1);
            team1Scores.Add(score2);
            team1Scores.Add(score5);
            team1Scores.Add(score6);
            team2Scores.Add(score3);
            team2Scores.Add(score4);
            team2Scores.Add(score7);
            team2Scores.Add(score8);

            teamPoints.UpdatePointsForTeam(team1Scores, team2Scores, week2, team1, team2, players);

            List<WeeklyPoint> team1WeeklyPoints = team1.WeeklyPoints.Where(p => p.Week == week2).ToList();
            Assert.That(team1WeeklyPoints.Count, Is.EqualTo(1));
            WeeklyPoint team1WeeklyPointForWeek2 = team1WeeklyPoints.FirstOrDefault(p => p.Week == week2);
            Assert.That(team1WeeklyPointForWeek2.Points, Is.EqualTo(2));


            List<WeeklyPoint> team2WeeklyPoints = team2.WeeklyPoints.Where(p => p.Week == week2).ToList();
            Assert.That(team2WeeklyPoints.Count, Is.EqualTo(1));
            WeeklyPoint team2WeeklyPointForWeek2 = team2WeeklyPoints.FirstOrDefault(p => p.Week == week2);
            Assert.That(team2WeeklyPointForWeek2.Points, Is.EqualTo(2));
        }

        [Test]
        public void
            given_weekly_scores_for_each_team_when_determining_the_team_points_for_each_team_for_the_second_week_summary_report_then_assign_points_according_to_rules_using_initial_averages_to_calculate_handicap
            ()
        {
            TeamPoints teamPoints = new TeamPoints();
            Week week1 = CreateWeek(1, "week1");
            Week week2 = CreateWeek(2, "week2");
            Team team1 = CreateTeam("team1");
            Team team2 = CreateTeam("team2");
            Player player1 = CreatePlayer("M", 1, "p1", team1, 150);
            Player player2 = CreatePlayer("M", 2, "p2", team1, 120);
            Player player3 = CreatePlayer("M", 3, "p3", team2, 150);
            Player player4 = CreatePlayer("M", 4, "p4", team2, 150);
            List<WeeklyScore> team1Scores = new List<WeeklyScore>();
            List<WeeklyScore> team2Scores = new List<WeeklyScore>();
            List<Player> players = new List<Player> {player1, player2, player3, player4};

            WeeklyScore score1 = CreateWeeklyScore(100, 150, 100, player1, team1, week1, false);
            WeeklyScore score2 = CreateWeeklyScore(120, 120, 120, player2, team1, week1, false);
            WeeklyScore score3 = CreateWeeklyScore(110, 110, 110, player3, team2, week1, false);
            WeeklyScore score4 = CreateWeeklyScore(130, 115, 130, player4, team2, week1, false);

            team1Scores.Add(score1);
            team1Scores.Add(score2);
            team2Scores.Add(score3);
            team2Scores.Add(score4);

            teamPoints.UpdatePointsForTeam(team1Scores, team2Scores, week1, team1, team2, players);

            List<WeeklyPoint> team1WeeklyPoints = team1.WeeklyPoints.Where(p => p.Week == week1).ToList();
            Assert.That(team1WeeklyPoints.Count, Is.EqualTo(1));
            Assert.That(team1WeeklyPoints[0].Points, Is.EqualTo(4));


            List<WeeklyPoint> team2WeeklyPoints = team2.WeeklyPoints.Where(p => p.Week == week1).ToList();
            Assert.That(team2WeeklyPoints.Count, Is.EqualTo(1));
            Assert.That(team2WeeklyPoints[0].Points, Is.EqualTo(0));
        }
    }
}