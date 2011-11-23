using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BowlingStats.BusinessLogic
{
    public class Handicap
    {
        public double GetHandicap(double team1Average, double team2Average)
        {
            return team1Average < team2Average ? team2Average - team1Average : 0;
        }
    }
}
