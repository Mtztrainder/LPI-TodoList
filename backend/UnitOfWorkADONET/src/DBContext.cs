//Implementa a interface IDBContextFactory

using System;
using System.Data.Common;


namespace UnitOfWorkADONET
{
    public class DBContext : IDBContextFactory
    {
        private readonly string _connectionString = Environment.GetEnvironmentVariable("STR_CON");
        private readonly IDBContextFactory.TpProvider _tpProvider;

        public DBContext(IDBContextFactory.TpProvider tpProvider)
        {
            _tpProvider = tpProvider;
        }

        public DbConnection Create()
        {
            DbConnection connection = null;

            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new Exception("String de Conexão não fornecida.");
            }

            if (_tpProvider == IDBContextFactory.TpProvider.MySQL)
            {
                connection = new MySql.Data.MySqlClient.MySqlConnection();
            }
            else if (_tpProvider == IDBContextFactory.TpProvider.SQLServer)
            {
                connection = new Microsoft.Data.SqlClient.SqlConnection();
            }
            else if (_tpProvider == IDBContextFactory.TpProvider.Oracle)
            {
                connection = new Oracle.ManagedDataAccess.Client.OracleConnection();
            }

            if (connection == null)
                throw new Exception("Falhou ao criar a conexão");

            connection.ConnectionString = _connectionString;

            return connection;
        }
    }
}
