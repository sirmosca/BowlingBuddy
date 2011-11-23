using System.Collections.Generic;
using System.Linq;
using BowlingStats.Model;
using Raven.Client;

namespace BowlingStats.DataAccess
{
    public class PlayerRavenDataAccess
    {
        /// <summary>
        /// Gets all the players.
        /// </summary>
        /// <returns></returns>
        public List<Player> GetAllPlayers()
        {
            IDocumentStore documentStore = LocalDocumentStore.Instance;

            using (IDocumentSession session = documentStore.OpenSession())
            {
                return session.Query<Player>().ToList();
            }
        }

        /// <summary>
        /// Saves the players.
        /// </summary>
        /// <param name="players">The players.</param>
        public void SavePlayers(IEnumerable<Player> players)
        {
            IDocumentStore documentStore = LocalDocumentStore.Instance;

            using (IDocumentSession session = documentStore.OpenSession())
            {
                foreach (Player player in players)
                {
                    session.Store(player);    
                }
                
                session.SaveChanges();
            }
        }
    }
}