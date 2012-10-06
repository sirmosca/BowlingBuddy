using BowlingStats.Model;
using NUnit.Framework;

namespace BowlingStats.UnitTests
{
    [TestFixture]
    public class TeamTests
    {
        [Test]
        public void GivenANewTeamWhenAddingTheTeamToTheLeagueThentheWeeklyPointsWillBeInitializedToEmptyList()
        {
            var team = new Team("adamo");
            Assert.NotNull(team.WeeklyPoints);
            Assert.IsTrue(team.WeeklyPoints.Count == 0);
        }
    }
}