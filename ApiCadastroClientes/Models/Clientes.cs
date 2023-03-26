namespace ApiCadastroClientes.Models
{
    public class Clientes
    {
        public uint Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public Enderecos Endereco { get; set; }
    }
}
