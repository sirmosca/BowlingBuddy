using System.Collections.Generic;
using System.Linq;
using BowlingStats.Model;
using Raven.Client;

namespace BowlingStats.DataAccess
{
    /// <summary>
    /// Data access for RavenDB
    /// </summary>
    public class RavenDataAccess
    {
        public List<League> GetAllLeagues()
        {
            IDocumentStore documentStore = LocalDocumentStore.Instance;

            using (IDocumentSession session = documentStore.OpenSession())
            {
                return session.Query<League>().ToList();
            }
        }

        public void DeleteLeague(int leagueId)
        {
            IDocumentStore documentStore = LocalDocumentStore.Instance;

            using (IDocumentSession session = documentStore.OpenSession())
            {
                League leagueToDelete = session.Load<League>(leagueId);
                session.Delete(leagueToDelete);
                session.SaveChanges();
            }
        }

        /// <summary>
        /// Saves the league.
        /// </summary>
        /// <param name="league">The league.</param>
        /// <returns></returns>
        public League SaveLeague(League league)
        {
            IDocumentStore documentStore = LocalDocumentStore.Instance;

            using (IDocumentSession session = documentStore.OpenSession())
            {
                session.Store(league);
                session.SaveChanges();
            }

            return league;
        }
    }
}