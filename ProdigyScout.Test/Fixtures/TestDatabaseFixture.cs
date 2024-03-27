using ProdigyScout.Data;
using ProdigyScout.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using ProdigyScout.Tests.Helpers;

namespace ProdigyScout.Tests.Fixture
{
    public class TestDatabaseFixture : IDisposable
    {
        private static readonly object _lock = new();
        private static bool _databaseInitialized;

        public TestDatabaseFixture()
        {
            // This is the same connection string from the appsettings.json file in the app
            // with a separate database name.
            Connection = new SqlConnection(@"Server=(localdb)\mssqllocaldb;Database=ProdigyScoutTests;Trusted_Connection=True;MultipleActiveResultSets=true");

            // In order to maintain a "known state" with the database, add 
            // data that can be used to assist with the assertions since this
            // minmics a production database.
            Seed();

            // Open the connection to the database which is used by
            // each of the unit tests.
            Connection.Open();
        }

        public void Dispose()
        {
            Connection.Dispose();
            GC.SuppressFinalize(this);
        }

        public DbConnection Connection
        {
            get;
        }

        public ProdigyScoutContext CreateContext(DbTransaction? transaction = null)
        {
            var context = new ProdigyScoutContext(new DbContextOptionsBuilder<ProdigyScoutContext>()
                .UseSqlServer(Connection).Options);

            if (transaction != null)
            {
                context.Database.UseTransaction(transaction);
            }

            return context;
        }

        private void Seed()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();

                        AddStudents(context);
                        context.SaveChanges();
                    }

                    _databaseInitialized = true;
                }
            }
        }

        private static void AddStudents(ProdigyScoutContext context)
        {
            context.Prospect
                .AddRange(
                    new Prospect { FirstName = Constants.FIRST_NAME, LastName = Constants.LAST_NAME_1 },
                    new Prospect { FirstName = Constants.FIRST_NAME, LastName = Constants.LAST_NAME_2 },
                    new Prospect { FirstName = Constants.FIRST_NAME, LastName = Constants.LAST_NAME_3 });
        }
    }
}