using MySql.Data.MySqlClient;
using System.Data;

namespace ApiCadastroClientes.Database
{
    /// <summary>
    /// Classe de acesso ao banco de dados MySQL
    /// </summary>
    public class Database : IDisposable
    {
        private const string SERVER = "localhost";
        private const string DATABASE = "ApiCliente";
        private const string USER = "root";
        private const string PASSWORD = "";

        private readonly MySqlConnection _connection;
        private readonly string stringConnection = $"Server={SERVER}; Database={DATABASE}; uid={USER}; pwd={PASSWORD}";

        public Database()
        {
            _connection = new MySqlConnection(stringConnection);
            OpenConnection();
        }

        private void OpenConnection()
        {
            try
            {
                if (!IsConnectionOpen())
                {
                    _connection.Open();
                }
            }
            catch
            {
                throw;
            }
        }

        private bool IsConnectionOpen()
        {
            try
            {
                return _connection.State == ConnectionState.Open;
            }
            catch
            {
                throw;
            }
        }

        private void CloseConnection()
        {
            try
            {
                _connection.Close();
            }
            catch
            {
                throw;
            }
        }

        public void Dispose()
        {
            CloseConnection();
            _connection.Dispose();
        }

        /// <summary>
        /// Execute SQL recebe comandos do tipo "INSERT", "UPDATE" e "DELETE" e retorna true para sucesso ou false erro
        /// </summary>
        /// <param name="sql">Comando SQL</param>
        /// <returns></returns>
        public bool ExecuteSQL(string sql)
        {
            try
            {
                if (!IsConnectionOpen()) return false;
                
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = _connection;
                    cmd.CommandText = sql;

                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Execute Query recebe um comando sql "SELECT" e retorna os resultados da consulta por um DataTable
        /// </summary>
        /// <param name="sql">Comando SQL</param>
        /// <returns></returns>
        public MySqlDataReader ExecuteQuery(string sql)
        {
            try
            {
                MySqlDataReader reader;

                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Connection = _connection;
                    reader = cmd.ExecuteReader();
                }

                return reader;
            }
            catch
            {
                throw;
            }
        }
    }
}
