using MySql.Data.MySqlClient;

namespace ApiCadastroClientes.Migration
{
    public class DatabaseCreate : IDisposable
    {
        private const string SERVER = "localhost";
        private const string DATABASE = "ApiCliente";
        private const string USER = "root";
        private const string PASSWORD = "";

        private readonly MySqlConnection _connection;
        private readonly string stringConnection = $"Server={SERVER}; uid={USER}; pwd={PASSWORD}";

        public DatabaseCreate()
        {
            _connection = new MySqlConnection(stringConnection);
            _connection.Open();
        }

        public void Create()
        {
            CreateDatabase();
            UpdateDatabase();
        }

        /// <summary>
        /// Execute SQL recebe comandos do tipo "INSERT", "UPDATE" e "DELETE" e retorna true para sucesso ou false erro
        /// </summary>
        /// <param name="sql">Comando SQL</param>
        /// <returns></returns>
        private bool ExecuteSQL(string sql)
        {
            try
            {
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

        private void CreateDatabase()
        {
            try
            {
                string sql = $"CREATE DATABASE IF NOT EXISTS {DATABASE}";
                ExecuteSQL(sql);
                
                _connection.ChangeDatabase(DATABASE);
                
                CreateTableDatabaseVersion();
            }
            catch
            {
                throw;
            }
            
        }

        private void CreateTableDatabaseVersion()
        {
            try
            {
                string sql = "CREATE TABLE IF NOT EXISTS CONFIGBANCO(ID INT NOT NULL AUTO_INCREMENT, VERSAO INT NOT NULL DEFAULT 0, PRIMARY KEY(ID))";
                ExecuteSQL(sql);
            }
            catch
            {
                throw;
            }
        }

        private int GetDataBaseVersion()
        {
            try
            {
                int version = 0;

                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = _connection;
                    cmd.CommandText = "SELECT * FROM CONFIGBANCO";
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        reader.Close();
                        ExecuteSQL("INSERT INTO CONFIGBANCO (VERSAO) VALUES(0)");
                        version = 0;
                    }
                    else
                    {
                        if (reader.Read())
                        {
                            version = reader.GetInt32("VERSAO");
                        }
                    }
                }

                return version;
            }
            catch
            {
                throw;
            }
        }

        private void UpdateDatabase()
        {
            try
            {
                int DatabaseVersion = GetDataBaseVersion();

                if (DatabaseVersion < 1)
                {
                    ExecuteSQL("CREATE TABLE IF NOT EXISTS CLIENTES(ID INT NOT NULL AUTO_INCREMENT, NOME VARCHAR(100) NOT NULL, EMAIL VARCHAR(255), TELEFONE VARCHAR(30) NOT NULL, LOGRADOURO VARCHAR(255), NUMERO VARCHAR(20), BAIRRO VARCHAR(255), CIDADE VARCHAR(255), UF VARCHAR(2), CEP VARCHAR(20), PRIMARY KEY(ID))");
                    DatabaseVersion++;
                    ExecuteSQL($"UPDATE CONFIGBANCO SET VERSAO = {DatabaseVersion}");
                }
            }
            catch
            {
                throw;
            }
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}
