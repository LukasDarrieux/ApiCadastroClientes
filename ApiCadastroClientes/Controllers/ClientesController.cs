using ApiCadastroClientes.Models;
using ApiCadastroClientes.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCadastroClientes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        [Route("create")]
        [HttpPost]
        public IActionResult PostCliente(Clientes cliente)
        {
            if (!ValidateCliente(cliente, out string mensagemErro))
            {
                var retorno = new
                {
                    status = 400,
                    message = mensagemErro
                };

                return BadRequest(retorno);
            }

            using (ClientesRepository clienteRepository = new ClientesRepository())
            {
                clienteRepository.Create(cliente);
            }

            var response = new
            {
                status = 200,
                message = "Cliente cadastrado com sucesso"
            };

            return Ok(response);
        }

        [Route("update")]
        [HttpPut]
        public IActionResult UpdateCliente(int Id, Clientes cliente)
        {
            string mensagemErro;
            if (!ValidateCliente(cliente, out mensagemErro))
            {
                var retorno = new
                {
                    status = 400,
                    message = mensagemErro
                };

                return BadRequest(retorno);
            }

            if (cliente.Id != Id)
            {
                var retorno = new
                {
                    status = 400,
                    message = "Id informado é diferente do Id do cliente"
                };

                return BadRequest(retorno);
            }

            using (ClientesRepository clienteRepository = new ClientesRepository())
            {
                clienteRepository.Update(Id, cliente);
            }

            var response = new
            {
                status = 200,
                message = "Cliente atualizado com sucesso"
            };

            return Ok(response);
        }

        [Route("delete")]
        [HttpDelete]
        public IActionResult DeleteCliente(int Id)
        {
            using (ClientesRepository clienteRepository = new ClientesRepository())
            {
                clienteRepository.Delete(Id);
            }

            var response = new
            {
                status = 200,
                message = "Cliente excluído com sucesso"
            };

            return Ok(response);
        }

        [HttpGet("{Id}")]
        public ActionResult<Clientes>? GetById(int Id)
        {
            if (Id <= 0)
            {
                var response = new
                {
                    status = 400,
                    message = "Id inválido"
                };

                return BadRequest(response);
            }

            Clientes? cliente = null;

            using (ClientesRepository clienteRepository = new ClientesRepository())
            {
                cliente = clienteRepository.Get(Id);
            }

            return Ok(cliente);
        }

        [HttpGet()]
        public ActionResult<List<Clientes>> GetAll()
        {
            
            List<Clientes> ListaCliente = new List<Clientes>();

            using (ClientesRepository clienteRepository = new ClientesRepository())
            {
                ListaCliente.AddRange(clienteRepository.GetAll());
            }

            return Ok(ListaCliente);
        }

        private bool ValidateCliente(Clientes cliente,  out string mensagemErro)
        {
            mensagemErro = "";

            if (cliente is null)
            {
                mensagemErro = "Cliente está com valor nulo";
                return false;
            }

            if (String.IsNullOrEmpty(cliente.Nome.Trim()))
            {
                mensagemErro = "Informe o nome do cliente";
                return false;
            }
            
            if (String.IsNullOrEmpty(cliente.Telefone))
            {
                mensagemErro = "Informe o telefone do cliente. Formato (00) 00000 0000";
                return false;
            }

            if (cliente.Endereco is null)
            {
                mensagemErro = "Endereço do cliente está com valor nulo";
                return false;
            }

            if (String.IsNullOrEmpty(cliente.Endereco.Logradouro.Trim()))
            {
                mensagemErro = "Informe o logradouro do cliente";
                return false;
            }

            if (String.IsNullOrEmpty(cliente.Endereco.Bairro.Trim()))
            {
                mensagemErro = "Informe o bairro do cliente";
                return false;
            }

            if (String.IsNullOrEmpty(cliente.Endereco.Cidade.Trim()))
            {
                mensagemErro = "Informe a cidade do cliente";
                return false;
            }

            if (String.IsNullOrEmpty(cliente.Endereco.UF.Trim()))
            {
                mensagemErro = "Informe o estado do cliente";
                return false;
            }

            if (cliente.Endereco.UF.Length != 2)
            {
                mensagemErro = "Informe a sigla do estado";
                return false;
            }
            
            return true;
        }
    }
}
