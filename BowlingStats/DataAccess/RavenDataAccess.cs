using System;
using System.Collections.Generic;
using System.IO;
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
            try
            {
                IDocumentStore documentStore = LocalDocumentStore.Instance;

                using (IDocumentSession session = documentStore.OpenSession())
                {
                    List<League> leagues = session.Query<League>().ToList();
                    return leagues;
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
            return new List<League>();
        }

        public void DeleteLeague(int leagueId)
        {
            try
            {
                IDocumentStore documentStore = LocalDocumentStore.Instance;

                using (IDocumentSession session = documentStore.OpenSession())
                {
                    League leagueToDelete = session.Load<League>(leagueId);
                    session.Delete(leagueToDelete);
                    session.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
            
        }

        /// <summary>
        /// Saves the league.
        /// </summary>
        /// <param name="league">The league.</param>
        /// <returns></returns>
        public League SaveLeague(League league)
        {
            try
            {
                IDocumentStore documentStore = LocalDocumentStore.Instance;

                using (IDocumentSession session = documentStore.OpenSession())
                {
                    session.Store(league);
                    session.SaveChanges();
                }

                return league;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
            
            return new League();
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private void WriteLog(Exception ex)
        {
            using (FileStream writer = new FileStream("aaaaa.txt", FileMode.OpenOrCreate) )
            {
                writer.Write(GetBytes(ex.Message), 0, ex.Message.Length);
            }
        }
    }
}