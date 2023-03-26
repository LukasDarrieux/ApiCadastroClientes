using ApiCadastroClientes.Models;
using System.Data;
using MySql.Data.MySqlClient;

namespace ApiCadastroClientes.Repository
{
    public class ClientesRepository : IDisposable
    {
        public ClientesRepository() 
        { 
        }

        public bool Create(Clientes Cliente)
        {
            try
            {
                if (Cliente is null) return false;

                using (Database.Database db = new())
                {
                    string sql = $"INSERT INTO CLIENTES (NOME, EMAIL, TELEFONE, LOGRADOURO, NUMERO, BAIRRO, CIDADE , UF, CEP) " +
                        $"VALUES('{Cliente.Nome}', '{Cliente.Email}', '{Cliente.Telefone}', " +
                        $"'{Cliente.Endereco.Logradouro}', '{Cliente.Endereco.Numero}', '{Cliente.Endereco.Bairro}', "+ 
                        $"'{Cliente.Endereco.Cidade}', '{Cliente.Endereco.UF}', '{Cliente.Endereco.CEP}')";

                    db.ExecuteSQL(sql);
                }

                return true;
            }
            catch
            {
                throw;
            }
            
        }

        public bool Update(int Id, Clientes Cliente)
        {
            try
            {
                if (Cliente is null) return false;

                using (Database.Database db = new())
                {
                    string sql = $"UPDATE CLIENTES SET NOME = '{Cliente.Nome}', EMAIL = '{Cliente.Email}', TELEFONE = '{Cliente.Telefone}', " +
                        $"LOGRADOURO = '{Cliente.Endereco.Logradouro}', NUMERO = '{Cliente.Endereco.Numero}', BAIRRO = '{Cliente.Endereco.Bairro}', " +
                        $"CIDADE = '{Cliente.Endereco.Cidade}', UF = '{Cliente.Endereco.UF}', CEP = '{Cliente.Endereco.CEP}' " +
                        $"WHERE ID = {Id}";
                       

                    db.ExecuteSQL(sql);
                }

                return true;
            }
            catch
            {
                throw;
            }

        }

        public bool Delete(int Id)
        {
            try
            {
                if (Id <= 0) return false;

                using (Database.Database db = new ())
                {
                    string sql = $"DELETE FROM CLIENTES WHERE ID = {Id}";

                    db.ExecuteSQL(sql);
                }

                return true;
            }
            catch
            {
                throw;
            }

        }

        public Clientes? Get(int Id)
        {
            try
            {
                if (Id <= 0) return null;

                Clientes? Cliente = null;

                using (Database.Database db = new())
                {
                    string sql = $"SELECT * FROM CLIENTES WHERE ID = {Id}";
                    using (MySqlDataReader Dados = db.ExecuteQuery(sql))
                    {
                        if (Dados is null) return null;

                        if (Dados.Read())
                        {
                            Cliente = new Clientes()
                            {
                                Id = Dados.GetUInt32("Id"),
                                Nome = Dados.GetString("Nome"),
                                Email = Dados.GetString("Email"),
                                Telefone = Dados.GetString("Telefone"),
                                Endereco = new Enderecos()
                                {
                                    Logradouro = Dados.GetString("Logradouro"),
                                    Numero = Dados.GetUInt16("Numero"),
                                    Bairro = Dados.GetString("Bairro"),
                                    Cidade = Dados.GetString("Cidade"),
                                    UF = Dados.GetString("UF"),
                                    CEP = Dados.GetUInt32("CEP")
                                } 
                            };
                        }
                    }
                }
                return Cliente;
            }
            catch
            {
                throw;
            }
        }

        public List<Clientes>? GetAll()
        {
            try
            {

                List<Clientes> ListaClientes = new();

                using (Database.Database db = new())
                {
                    string sql = $"SELECT * FROM CLIENTES";
                    using (MySqlDataReader Dados = db.ExecuteQuery(sql))
                    {
                        if (Dados is null) return null;

                        while (Dados.Read())
                        {
                            var cliente = new Clientes()
                            {
                                Id = Dados.GetUInt32("Id"),
                                Nome = Dados.GetString("Nome"),
                                Email = Dados.GetString("Email"),
                                Telefone = Dados.GetString("Telefone"),
                                Endereco = new Enderecos()
                                {
                                    Logradouro = Dados.GetString("Logradouro"),
                                    Numero = Dados.GetUInt16("Numero"),
                                    Bairro = Dados.GetString("Bairro"),
                                    Cidade = Dados.GetString("Cidade"),
                                    UF = Dados.GetString("UF"),
                                    CEP = Dados.GetUInt32("CEP")
                                }
                            };
                            
                            ListaClientes.Add(cliente);
                        }
                    }
                }
                return ListaClientes;
            }
            catch
            {
                throw;
            }
        }

        public void Dispose()
        {
            return;
        }
    }
}
