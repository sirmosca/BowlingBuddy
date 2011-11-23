namespace BowlingStats.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class WeeklyMatchup
    {
        /// <summary>
        /// Gets or sets the week.
        /// </summary>
        /// <value>
        /// The week.
        /// </value>
        public Week Week { get; set; }

        /// <summary>
        /// Gets or sets the team one.
        /// </summary>
        /// <value>
        /// The team one.
        /// </value>
        public Team TeamOne { get; set; }

        /// <summary>
        /// Gets or sets the team two.
        /// </summary>
        /// <value>
        /// The team two.
        /// </value>
        public Team TeamTwo { get; set; }
    }
}