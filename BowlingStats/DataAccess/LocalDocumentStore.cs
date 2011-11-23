using Raven.Client.Document;
using Raven.Client.Embedded;

namespace BowlingStats.DataAccess
{
    public class LocalDocumentStore
    {
        private const string PathToDocumentDatabase = @".\Data";
        private static EmbeddableDocumentStore _DocumentStore;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static DocumentStore Instance
        {
            get
            {
                if (_DocumentStore == null)
                {
                    _DocumentStore = new EmbeddableDocumentStore { DataDirectory = PathToDocumentDatabase };
                    _DocumentStore.Initialize();
                }

                return _DocumentStore;
            }
        }
    }
}