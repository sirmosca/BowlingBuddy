using System.Collections.Generic;
using BowlingStats.BusinessLogic;
using BowlingStats.Model;
using FakeItEasy;
using NUnit.Framework;

namespace BowlingStats.UnitTests
{
    [TestFixture]
    public class PlayerStatsTests
    {
        [Test]
        public void given_a_player_when_no_scores_have_been_entered_then_the_initial_average_should_be_returned()
        {
            Team team1 = CreateTeam("team1");
            Player player1 = CreatePlayer("M", 1, "player1", team1, 150);
            PlayerStats playerAverage = new PlayerStats();
            double average = playerAverage.GetPlayerAverage(player1, new List<WeeklyScore>(), 1);
            Assert.That(average, Is.EqualTo(150));
        }

        [Test]
        public void given_a_player_when_one_week_of_scores_have_been_entered_then_the_average_from_week_one_should_be_returned()
        {
            Team team1 = CreateTeam("team1");
            Player player1 = CreatePlayer("M", 1, "player1", team1, 150);
            Week week = CreateWeek(1, "week 1");
            WeeklyScore score = CreateWeeklyScore(110, 154, 204, player1, team1, week, false);
            PlayerStats playerAverage = new PlayerStats();
            double average = playerAverage.GetPlayerAverage(player1, new List<WeeklyScore> { score }, week.WeekNumber);
            Assert.That(average, Is.EqualTo(156));
        }

        [Test]
        public void given_a_player_when_two_weeks_of_scores_have_been_entered_then_the_average_from_both_weeks_should_be_returned()
        {
            Team team1 = CreateTeam("team1");
            Player player1 = CreatePlayer("M", 1, "player1", team1, 150);
            Week week = CreateWeek(1, "week 1");
            WeeklyScore week1Score = CreateWeeklyScore(110, 154, 204, player1, team1, week, false);
            WeeklyScore week2Score = CreateWeeklyScore(155, 186, 178, player1, team1, week, false);
            PlayerStats playerAverage = new PlayerStats();
            double average = playerAverage.GetPlayerAverage(player1, new List<WeeklyScore> { week1Score, week2Score }, week.WeekNumber);
            Assert.That(average, Is.EqualTo(165));
        }

        [Test]
        public void given_a_player_when_two_weeks_of_scores_have_been_entered_with_an_absent_week_then_only_nonabsent_weeks_should_be_averaged()
        {
            Team team1 = CreateTeam("team1");
            Player player1 = CreatePlayer("M", 1, "player1", team1, 150);
            Week week1 = CreateWeek(1, "week 1");
            Week week2 = CreateWeek(2, "week 2");
            WeeklyScore week1Score = CreateWeeklyScore(110, 154, 204, player1, team1, week1, false);
            WeeklyScore week2Score = CreateWeeklyScore(155, 186, 178, player1, team1, week2, true);
            PlayerStats playerAverage = new PlayerStats();
            double average = playerAverage.GetPlayerAverage(player1, new List<WeeklyScore> { week1Score, week2Score }, week2.WeekNumber);
            Assert.That(average, Is.EqualTo(156));
        }

        [Test]
        public  void given_a_player_when_all_scores_are_supplied_then_get_only_the_player_average_until_the_week_specified()
        {
            Team team1 = CreateTeam("team1");
            Player player1 = CreatePlayer("M", 1, "player1", team1, 150);
            Week week1 = CreateWeek(1, "week 1");
            Week week2 = CreateWeek(2, "week 2");
            WeeklyScore week1Score = CreateWeeklyScore(110, 154, 204, player1, team1, week1, false);
            WeeklyScore week2Score = CreateWeeklyScore(155, 186, 178, player1, team1, week2, false);
            PlayerStats playerAverage = new PlayerStats();
            double average = playerAverage.GetPlayerAverage(player1, new List<WeeklyScore> { week1Score, week2Score }, week1.WeekNumber);
            Assert.That(average, Is.EqualTo(156));
        }

        [Test]
        public void given_a_player_when_all_scores_are_supplied_then_get_only_total_pins()
        {
            Team team1 = CreateTeam("team1");
            Player player1 = CreatePlayer("M", 1, "player1", team1, 150);
            Week week1 = CreateWeek(1, "week 1");
            Week week2 = CreateWeek(2, "week 2");
            WeeklyScore week1Score = CreateWeeklyScore(110, 154, 204, player1, team1, week1, false);
            WeeklyScore week2Score = CreateWeeklyScore(155, 186, 178, player1, team1, week2, false);
            List<WeeklyScore> scores = new List<WeeklyScore>() {week1Score, week2Score};
            PlayerStats playerAverage = new PlayerStats();
            int totalPins = playerAverage.GetTotalPinsForPlayer(player1, scores);
            Assert.That(totalPins, Is.EqualTo(987));
        }

        [Test]
        public void given_a_player_when_all_scores_are_supplied_then_get_only_total_pins_in_which_the_player_is_not_absent()
        {
            Team team1 = CreateTeam("team1");
            Player player1 = CreatePlayer("M", 1, "player1", team1, 150);
            Week week1 = CreateWeek(1, "week 1");
            Week week2 = CreateWeek(2, "week 2");
            WeeklyScore week1Score = CreateWeeklyScore(110, 154, 204, player1, team1, week1, false);
            WeeklyScore week2Score = CreateWeeklyScore(155, 186, 178, player1, team1, week2, true);
            List<WeeklyScore> scores = new List<WeeklyScore>() { week1Score, week2Score };
            PlayerStats playerAverage = new PlayerStats();
            int totalPins = playerAverage.GetTotalPinsForPlayer(player1, scores);
            Assert.That(totalPins, Is.EqualTo(468));
        }

        [Test]
        public void given_a_player_when_all_scores_are_supplied_then_get_only_total_pins_for_the_player()
        {
            Team team1 = CreateTeam("team1");
            Player player1 = CreatePlayer("M", 1, "player1", team1, 150);
            Player player2 = CreatePlayer("M", 2, "player2", team1, 150);
            Week week1 = CreateWeek(1, "week 1");
            Week week2 = CreateWeek(2, "week 2");
            WeeklyScore week1Score = CreateWeeklyScore(110, 154, 204, player1, team1, week1, false);
            WeeklyScore week2Score = CreateWeeklyScore(155, 186, 178, player2, team1, week2, false);
            List<WeeklyScore> scores = new List<WeeklyScore>() { week1Score, week2Score };
            PlayerStats playerAverage = new PlayerStats();
            int totalPins = playerAverage.GetTotalPinsForPlayer(player1, scores);
            Assert.That(totalPins, Is.EqualTo(468));
        }

        [Test]
        public void given_a_player_when_all_scores_are_supplied_then_get_only_total_games()
        {
            Team team1 = CreateTeam("team1");
            Player player1 = CreatePlayer("M", 1, "player1", team1, 150);
            Week week1 = CreateWeek(1, "week 1");
            Week week2 = CreateWeek(2, "week 2");
            WeeklyScore week1Score = CreateWeeklyScore(110, 154, 204, player1, team1, week1, false);
            WeeklyScore week2Score = CreateWeeklyScore(155, 186, 178, player1, team1, week2, false);
            List<WeeklyScore> scores = new List<WeeklyScore>() { week1Score, week2Score };
            PlayerStats playerAverage = new PlayerStats();
            int totalPins = playerAverage.GetTotalGamesForPlayer(player1, scores);
            Assert.That(totalPins, Is.EqualTo(6));
        }

        [Test]
        public void given_a_player_when_all_scores_are_supplied_then_get_only_total_games_in_which_the_player_is_not_absent()
        {
            Team team1 = CreateTeam("team1");
            Player player1 = CreatePlayer("M", 1, "player1", team1, 150);
            Week week1 = CreateWeek(1, "week 1");
            Week week2 = CreateWeek(2, "week 2");
            WeeklyScore week1Score = CreateWeeklyScore(110, 154, 204, player1, team1, week1, false);
            WeeklyScore week2Score = CreateWeeklyScore(155, 186, 178, player1, team1, week2, true);
            List<WeeklyScore> scores = new List<WeeklyScore>() { week1Score, week2Score };
            PlayerStats playerAverage = new PlayerStats();
            int totalPins = playerAverage.GetTotalGamesForPlayer(player1, scores);
            Assert.That(totalPins, Is.EqualTo(3));
        }

        [Test]
        public void given_a_player_when_all_scores_are_supplied_then_get_only_total_games_for_the_player()
        {
            Team team1 = CreateTeam("team1");
            Player player1 = CreatePlayer("M", 1, "player1", team1, 150);
            Player player2 = CreatePlayer("M", 2, "player2", team1, 150);
            Week week1 = CreateWeek(1, "week 1");
            Week week2 = CreateWeek(2, "week 2");
            WeeklyScore week1Score = CreateWeeklyScore(110, 154, 204, player1, team1, week1, false);
            WeeklyScore week2Score = CreateWeeklyScore(155, 186, 178, player2, team1, week2, false);
            List<WeeklyScore> scores = new List<WeeklyScore>() { week1Score, week2Score };
            PlayerStats playerAverage = new PlayerStats();
            int totalPins = playerAverage.GetTotalGamesForPlayer(player1, scores);
            Assert.That(totalPins, Is.EqualTo(3));
        }

        [Test]
        public void given_a_player_when_all_scores_are_supplied_then_get_only_total_pins_between_200_and_250()
        {
            Team team1 = CreateTeam("team1");
            Player player1 = CreatePlayer("M", 1, "player1", team1, 150);
            Week week1 = CreateWeek(1, "week 1");
            Week week2 = CreateWeek(2, "week 2");
            WeeklyScore week1Score = CreateWeeklyScore(110, 154, 204, player1, team1, week1, false);
            WeeklyScore week2Score = CreateWeeklyScore(155, 186, 178, player1, team1, week2, false);
            List<WeeklyScore> scores = new List<WeeklyScore>() { week1Score, week2Score };
            PlayerStats playerAverage = new PlayerStats();
            int totalPins = playerAverage.GetTotalGamesWithPinsBetween200And250ForPlayer(player1, scores);
            Assert.That(totalPins, Is.EqualTo(1));
        }

        [Test]
        public void given_a_player_when_all_scores_are_supplied_then_get_only_total_pins_between_200_and_250_in_which_the_player_is_not_absent()
        {
            Team team1 = CreateTeam("team1");
            Player player1 = CreatePlayer("M", 1, "player1", team1, 150);
            Week week1 = CreateWeek(1, "week 1");
            Week week2 = CreateWeek(2, "week 2");
            WeeklyScore week1Score = CreateWeeklyScore(110, 154, 204, player1, team1, week1, false);
            WeeklyScore week2Score = CreateWeeklyScore(155, 212, 178, player1, team1, week2, true);
            List<WeeklyScore> scores = new List<WeeklyScore>() { week1Score, week2Score };
            PlayerStats playerAverage = new PlayerStats();
            int totalPins = playerAverage.GetTotalGamesWithPinsBetween200And250ForPlayer(player1, scores);
            Assert.That(totalPins, Is.EqualTo(1));
        }

        [Test]
        public void given_a_player_when_all_scores_are_supplied_then_get_only_total_pins_between_200_and_250_for_the_player()
        {
            Team team1 = CreateTeam("team1");
            Player player1 = CreatePlayer("M", 1, "player1", team1, 150);
            Player player2 = CreatePlayer("M", 2, "player2", team1, 150);
            Week week1 = CreateWeek(1, "week 1");
            Week week2 = CreateWeek(2, "week 2");
            WeeklyScore week1Score = CreateWeeklyScore(110, 154, 204, player1, team1, week1, false);
            WeeklyScore week2Score = CreateWeeklyScore(155, 215, 178, player2, team1, week2, false);
            List<WeeklyScore> scores = new List<WeeklyScore>() { week1Score, week2Score };
            PlayerStats playerAverage = new PlayerStats();
            int totalPins = playerAverage.GetTotalGamesWithPinsBetween200And250ForPlayer(player1, scores);
            Assert.That(totalPins, Is.EqualTo(1));
        }

        [Test]
        public void given_a_player_when_all_scores_are_supplied_then_get_only_total_pins_over_250()
        {
            Team team1 = CreateTeam("team1");
            Player player1 = CreatePlayer("M", 1, "player1", team1, 150);
            Week week1 = CreateWeek(1, "week 1");
            Week week2 = CreateWeek(2, "week 2");
            WeeklyScore week1Score = CreateWeeklyScore(110, 256, 204, player1, team1, week1, false);
            WeeklyScore week2Score = CreateWeeklyScore(155, 186, 258, player1, team1, week2, false);
            List<WeeklyScore> scores = new List<WeeklyScore>() { week1Score, week2Score };
            PlayerStats playerAverage = new PlayerStats();
            int totalPins = playerAverage.GetTotalGamesWithPinsOver250ForPlayer(player1, scores);
            Assert.That(totalPins, Is.EqualTo(2));
        }

        [Test]
        public void given_a_player_when_all_scores_are_supplied_then_get_only_total_pins_over_250_in_which_the_player_is_not_absent()
        {
            Team team1 = CreateTeam("team1");
            Player player1 = CreatePlayer("M", 1, "player1", team1, 150);
            Week week1 = CreateWeek(1, "week 1");
            Week week2 = CreateWeek(2, "week 2");
            WeeklyScore week1Score = CreateWeeklyScore(110, 255, 204, player1, team1, week1, false);
            WeeklyScore week2Score = CreateWeeklyScore(155, 212, 255, player1, team1, week2, true);
            List<WeeklyScore> scores = new List<WeeklyScore>() { week1Score, week2Score };
            PlayerStats playerAverage = new PlayerStats();
            int totalPins = playerAverage.GetTotalGamesWithPinsOver250ForPlayer(player1, scores);
            Assert.That(totalPins, Is.EqualTo(1));
        }

        [Test]
        public void given_a_player_when_all_scores_are_supplied_then_get_only_total_pins_over_250_for_the_player()
        {
            Team team1 = CreateTeam("team1");
            Player player1 = CreatePlayer("M", 1, "player1", team1, 150);
            Player player2 = CreatePlayer("M", 2, "player2", team1, 150);
            Week week1 = CreateWeek(1, "week 1");
            Week week2 = CreateWeek(2, "week 2");
            WeeklyScore week1Score = CreateWeeklyScore(110, 255, 204, player1, team1, week1, false);
            WeeklyScore week2Score = CreateWeeklyScore(155, 215, 255, player2, team1, week2, false);
            List<WeeklyScore> scores = new List<WeeklyScore>() { week1Score, week2Score };
            PlayerStats playerAverage = new PlayerStats();
            int totalPins = playerAverage.GetTotalGamesWithPinsOver250ForPlayer(player1, scores);
            Assert.That(totalPins, Is.EqualTo(1));
        }

        [Test]
        public void given_a_list_of_scores_when_printing_the_top_3_high_scores_for_men_then_only_the_top_3_high_scores_for_men_should_print()
        {
            Team team1 = CreateTeam("team1");
            Team team2 = CreateTeam("team2");
            Week week1 = CreateWeek(1, "week1");
            Player player1 = CreatePlayer("M", 1, "player1", team1, 150);
            Player player2 = CreatePlayer("F", 2, "player2", team1, 150);
            WeeklyScore score1 = CreateWeeklyScore(100, 101, 102, player1, team1, week1, false);
            WeeklyScore score2 = CreateWeeklyScore(103, 104, 105, player2, team1, week1, false);
            WeeklyScore score3 = CreateWeeklyScore(106, 107, 108, player1, team1, week1, false);
            WeeklyScore score4 = CreateWeeklyScore(109, 110, 111, player2, team1, week1, false);
            WeeklyScore score5 = CreateWeeklyScore(112, 113, 114, player1, team1, week1, false);
            WeeklyScore score6 = CreateWeeklyScore(115, 116, 117, player2, team1, week1, false);
            WeeklyScore score7 = CreateWeeklyScore(118, 119, 120, player1, team1, week1, true);
            WeeklyScore score8 = CreateWeeklyScore(121, 122, 123, player2, team1, week1, false);
            List<WeeklyScore> scores = new List<WeeklyScore>()
                                           {score1, score2, score3, score4, score5, score6, score7, score8};
            PlayerStats playerStats = new PlayerStats();
            List<Point> top3Scores = playerStats.GetTop3HighGamePlayers(scores, "M");
            Assert.That(3, Is.EqualTo(top3Scores.Count));
            Assert.IsNotNull(top3Scores.Find(p => p.Points == 112));
            Assert.IsNotNull(top3Scores.Find(p => p.Points == 113));
            Assert.IsNotNull(top3Scores.Find(p => p.Points == 114));
        }

        [Test]
        public void given_a_list_of_scores_when_printing_the_top_3_high_scores_for_women_then_only_the_top_3_high_scores_for_women_should_print()
        {
            Team team1 = CreateTeam("team1");
            Team team2 = CreateTeam("team2");
            Week week1 = CreateWeek(1, "week1");
            Player player1 = CreatePlayer("F", 1, "player1", team1, 150);
            Player player2 = CreatePlayer("M", 2, "player2", team1, 150);
            WeeklyScore score1 = CreateWeeklyScore(100, 101, 102, player1, team1, week1, false);
            WeeklyScore score2 = CreateWeeklyScore(103, 104, 105, player2, team1, week1, false);
            WeeklyScore score3 = CreateWeeklyScore(106, 107, 108, player1, team1, week1, false);
            WeeklyScore score4 = CreateWeeklyScore(109, 110, 111, player2, team1, week1, false);
            WeeklyScore score5 = CreateWeeklyScore(112, 113, 114, player1, team1, week1, false);
            WeeklyScore score6 = CreateWeeklyScore(115, 116, 117, player2, team1, week1, false);
            WeeklyScore score7 = CreateWeeklyScore(118, 119, 120, player1, team1, week1, true);
            WeeklyScore score8 = CreateWeeklyScore(121, 122, 123, player2, team1, week1, true);
            List<WeeklyScore> scores = new List<WeeklyScore>() { score1, score2, score3, score4, score5, score6, score7, score8 };
            PlayerStats playerStats = new PlayerStats();
            List<Point> top3Scores = playerStats.GetTop3HighGamePlayers(scores, "F");
            Assert.That(3, Is.EqualTo(top3Scores.Count));
            Assert.IsNotNull(top3Scores.Find(p => p.Points == 112));
            Assert.IsNotNull(top3Scores.Find(p => p.Points == 113));
            Assert.IsNotNull(top3Scores.Find(p => p.Points == 114));
        }

        [Test]
        public void given_a_list_of_scores_when_printing_the_top_3_high_series_for_men_then_only_the_top_3_high_series_for_men_should_print()
        {
            Team team1 = CreateTeam("team1");
            Week week1 = CreateWeek(1, "week1");
            Player player1 = CreatePlayer("M", 1, "player1", team1, 150);
            Player player2 = CreatePlayer("F", 2, "player2", team1, 150);
            Player player3 = CreatePlayer("M", 3, "player3", team1, 150);
            Player player4 = CreatePlayer("F", 4, "player4", team1, 150);
            Player player5 = CreatePlayer("M", 5, "player5", team1, 150);
            Player player6 = CreatePlayer("F", 6, "player6", team1, 150);
            Player player7 = CreatePlayer("M", 7, "player7", team1, 150);
            Player player8 = CreatePlayer("F", 8, "player8", team1, 150);
            WeeklyScore score1 = CreateWeeklyScore(100, 101, 102, player1, team1, week1, false);
            WeeklyScore score2 = CreateWeeklyScore(103, 104, 105, player2, team1, week1, false);
            WeeklyScore score3 = CreateWeeklyScore(106, 107, 108, player3, team1, week1, false);
            WeeklyScore score4 = CreateWeeklyScore(109, 110, 111, player4, team1, week1, false);
            WeeklyScore score5 = CreateWeeklyScore(112, 113, 114, player5, team1, week1, false);
            WeeklyScore score6 = CreateWeeklyScore(115, 116, 117, player6, team1, week1, false);
            WeeklyScore score7 = CreateWeeklyScore(118, 119, 120, player7, team1, week1, true);
            WeeklyScore score8 = CreateWeeklyScore(121, 122, 123, player8, team1, week1, false);
            List<WeeklyScore> scores = new List<WeeklyScore>() { score1, score2, score3, score4, score5, score6, score7, score8 };
            PlayerStats playerStats = new PlayerStats();
            List<Point> top3Scores = playerStats.GetTop3HighSeriesPlayers(scores, "M");
            Assert.That(3, Is.EqualTo(top3Scores.Count));
            Assert.IsNotNull(top3Scores.Find(p => p.Points == 339));
            Assert.IsNotNull(top3Scores.Find(p => p.Points == 321));
            Assert.IsNotNull(top3Scores.Find(p => p.Points == 303));
        }

        [Test]
        public void given_a_list_of_scores_when_printing_the_top_3_high_series_for_women_then_only_the_top_3_high_series_for_women_should_print()
        {
            Team team1 = CreateTeam("team1");
            Team team2 = CreateTeam("team2");
            Week week1 = CreateWeek(1, "week1");
            Player player1 = CreatePlayer("M", 1, "player1", team1, 150);
            Player player2 = CreatePlayer("F", 2, "player2", team1, 150);
            Player player3 = CreatePlayer("M", 3, "player3", team1, 150);
            Player player4 = CreatePlayer("F", 4, "player4", team1, 150);
            Player player5 = CreatePlayer("M", 5, "player5", team1, 150);
            Player player6 = CreatePlayer("F", 6, "player6", team1, 150);
            Player player7 = CreatePlayer("M", 7, "player7", team1, 150);
            Player player8 = CreatePlayer("F", 8, "player8", team1, 150);
            WeeklyScore score1 = CreateWeeklyScore(100, 101, 102, player1, team1, week1, false);
            WeeklyScore score2 = CreateWeeklyScore(103, 104, 105, player2, team1, week1, false);
            WeeklyScore score3 = CreateWeeklyScore(106, 107, 108, player3, team1, week1, false);
            WeeklyScore score4 = CreateWeeklyScore(109, 110, 111, player4, team1, week1, false);
            WeeklyScore score5 = CreateWeeklyScore(112, 113, 114, player5, team1, week1, false);
            WeeklyScore score6 = CreateWeeklyScore(115, 116, 117, player6, team1, week1, false);
            WeeklyScore score7 = CreateWeeklyScore(118, 119, 120, player7, team1, week1, true);
            WeeklyScore score8 = CreateWeeklyScore(121, 122, 123, player8, team1, week1, true);
            List<WeeklyScore> scores = new List<WeeklyScore>() { score1, score2, score3, score4, score5, score6, score7, score8 };
            PlayerStats playerStats = new PlayerStats();
            List<Point> top3Scores = playerStats.GetTop3HighSeriesPlayers(scores, "M");
            Assert.That(3, Is.EqualTo(top3Scores.Count));
            Assert.IsNotNull(top3Scores.Find(p => p.Points == 339));
            Assert.IsNotNull(top3Scores.Find(p => p.Points == 321));
            Assert.IsNotNull(top3Scores.Find(p => p.Points == 303));
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