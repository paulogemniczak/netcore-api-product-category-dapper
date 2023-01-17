using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace Gemniczak.Data.Repositories
{
  internal class BaseRepository
  {
#if PRODUCAO
    internal const string ConnectionStringKey = "ConnectionStrings:Prod";
#elif HOMOLOGACAO
    internal const string ConnectionStringKey = "ConnectionStrings:Stag";
#else
    internal const string ConnectionStringKey = "ConnectionStrings:Dev";
#endif

    internal IDbConnection? Connection { get; set; }

    public IDbTransaction? Transaction { get; private set; }

    public BaseRepository(IConfiguration configuration)
    {
      IConfigurationSection connString = configuration.GetSection(ConnectionStringKey);
      if (string.IsNullOrWhiteSpace(connString.Value))
      {
        throw new ArgumentNullException($"Connection string not found");
      }
      Connection = new MySqlConnection(connString.Value);
    }

    public void SetTransaction(IDbTransaction transaction)
    {
      Transaction = transaction;
      Connection = transaction.Connection;
    }

    public void SetConnection(IDbConnection connection)
    {
      Connection = connection;
      Transaction = null;
    }
  }
}
