using Gemniczak.Data.Repositories;
using Gemniczak.Domain;
using System.Data;

namespace Gemniczak.Data
{
    public class UnitOfWorkTransaction : IUnitOfWorkTransaction
    {
        private IDbTransaction? transaction;
        private IDbConnection? connection;
        private readonly Dictionary<BaseRepository, IDbConnection?> repositoriesDictionary = new Dictionary<BaseRepository, IDbConnection?>();
        private bool disposed;

        public UnitOfWorkTransaction(IDbConnection connection, params IRepository[] repositories)
        {
            this.connection = connection;
            ConfigureConnections(connection, repositories);
        }

        private void ConfigureConnections(IDbConnection connection, IRepository[] repositories)
        {
            connection.Open();
            transaction = connection.BeginTransaction();
            foreach (IRepository repository in repositories)
            {
                var repositoryBase = repository as BaseRepository;
                if (repositoryBase == null)
                {
                    throw new InvalidOperationException("Invalid BaseRepository");
                }
                repositoriesDictionary.Add(repositoryBase, repositoryBase.Connection);
                repositoryBase.SetTransaction(transaction);
            }
        }

        public void Commit()
        {
            try
            {
                transaction?.Commit();
            }
            catch
            {
                transaction?.Rollback();
                throw;
            }
            finally
            {
                transaction?.Dispose();
            }

        }

        public void Rollback()
        {
            try
            {
                transaction?.Rollback();
            }
            finally
            {
                transaction?.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (!disposing)
                {
                    if (transaction != null)
                    {
                        transaction.Dispose();
                        transaction = null;
                    }
                    if (connection != null)
                    {
                        connection = null;
                    }
                    disposed = true;
                    ResetRepositoriesConnections();
                }
            }
        }

        private void ResetRepositoriesConnections()
        {
            foreach (var repository in repositoriesDictionary)
            {
                if (repository.Value != null)
                {
                    repository.Key.SetConnection(repository.Value);
                }
            }
        }

        ~UnitOfWorkTransaction()
        {
            ResetRepositoriesConnections();
            Dispose(false);
        }
    }
}
