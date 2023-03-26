namespace ApiCadastroClientes.Models
{
    public class Enderecos
    {
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public uint? Numero { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public uint? CEP { get; set; }

    }
}
