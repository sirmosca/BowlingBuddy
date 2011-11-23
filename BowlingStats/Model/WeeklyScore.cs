namespace BowlingStats.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class WeeklyScore
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="WeeklyScore"/> is absent.
        /// </summary>
        /// <value><c>true</c> if absent; otherwise, <c>false</c>.</value>
        public bool Absent { get; set; }

        /// <summary>
        /// Gets or sets the team.
        /// </summary>
        /// <value>
        /// The team.
        /// </value>
        public Team Team { get; set; }

        /// <summary>
        /// Gets or sets the week.
        /// </summary>
        /// <value>
        /// The week.
        /// </value>
        public Week Week { get; set; }

        /// <summary>
        /// Gets or sets the game1.
        /// </summary>
        /// <value>The game1.</value>
        public int Game1 { get; set; }

        /// <summary>
        /// Gets or sets the game2.
        /// </summary>
        /// <value>The game2.</value>
        public int Game2 { get; set; }

        /// <summary>
        /// Gets or sets the game3.
        /// </summary>
        /// <value>The game3.</value>
        public int Game3 { get; set; }

        /// <summary>
        /// Gets the total.
        /// </summary>
        /// <value>The total.</value>
        public int Total
        {
            get { return Game1 + Game2 + Game3; }
        }

        /// <summary>
        /// Gets or sets the player.
        /// </summary>
        /// <value>
        /// The player.
        /// </value>
        public Player Player { get; set; }
    }
}