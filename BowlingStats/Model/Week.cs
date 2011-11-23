namespace BowlingStats.Model
{
    /// <summary>
    /// </summary>
    public class Week
    {
        /// <summary>
        ///   Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int WeekNumber { get; set; }

        /// <summary>
        ///   Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        public bool Equals(Week other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return other.WeekNumber == WeekNumber;
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
            return obj.GetType() == typeof (Week) && Equals((Week) obj);
        }

        public override int GetHashCode()
        {
            return WeekNumber;
        }
    }
}