using System.Collections.Generic;

namespace BowlingStats.Model
{
    public class Team
    {
        public Team(string teamName)
        {
            Name = teamName;
            WeeklyPoints = new List<WeeklyPoint>();
        }

        public string Name { get; set; }

        public List<WeeklyPoint> WeeklyPoints { get; set; }

        public bool Equals(Team other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(other.Name, Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != typeof (Team))
            {
                return false;
            }
            return Equals((Team) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }
}