using Caliburn.Micro;

namespace BowlingStats
{
    /// <summary>
    /// 
    /// </summary>
    public class EventAggregationProvider
    {
        private static EventAggregator _EventAggregator;

        /// <summary>
        /// Gets the event aggregator.
        /// </summary>
        public static EventAggregator EventAggregator
        {
            get
            {
                if (_EventAggregator == null)
                {
                    _EventAggregator = new EventAggregator();
                }

                return _EventAggregator;
            }
        }
    }
}