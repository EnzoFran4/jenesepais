using FuncionarioAPI.Models;
using FuncionarioAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FuncionarioAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FuncionarioController : ControllerBase
    {
        private readonly FuncionarioRepository _repository;

        public FuncionarioController()
        {
            _repository = new FuncionarioRepository();
        }

        [HttpGet]
        public IActionResult ListarFuncionarios()
        {
            var funcionarios = _repository.CarregarDados();
            return Ok(funcionarios);
        }

        [HttpGet("{cpf}")]
        public IActionResult VisualizarFuncionario(string cpf)
        {
            var funcionarios = _repository.CarregarDados();
            var funcionario = funcionarios.FirstOrDefault(f => f.CPF == cpf);
            if (funcionario == null)
                return NotFound(new { message = "Funcionário não encontrado" });

            return Ok(funcionario);
        }

        [HttpPost]
        public IActionResult CriarFuncionario([FromBody] Funcionario novoFuncionario)
        {
            if (!_repository.ValidarCPF(novoFuncionario.CPF))
                return BadRequest(new { message = "CPF inválido" });

            var funcionarios = _repository.CarregarDados();
            if (funcionarios.Any(f => f.CPF == novoFuncionario.CPF))
                return BadRequest(new { message = "Funcionário já existe" });

            funcionarios.Add(novoFuncionario);
            _repository.SalvarDados(funcionarios);
            return CreatedAtAction(nameof(VisualizarFuncionario), new { cpf = novoFuncionario.CPF }, novoFuncionario);
        }

        [HttpPut("{cpf}")]
        public IActionResult AtualizarFuncionario(string cpf, [FromBody] Funcionario funcionarioAtualizado)
        {
            var funcionarios = _repository.CarregarDados();
            var funcionario = funcionarios.FirstOrDefault(f => f.CPF == cpf);

            if (funcionario == null)
                return NotFound(new { message = "Funcionário não encontrado" });

            if (!_repository.ValidarCPF(funcionarioAtualizado.CPF))
                return BadRequest(new { message = "CPF inválido" });

            funcionario.Nome = funcionarioAtualizado.Nome;
            funcionario.CTPS = funcionarioAtualizado.CTPS;
            funcionario.RG = funcionarioAtualizado.RG;
            funcionario.Funcao = funcionarioAtualizado.Funcao;
            funcionario.Setor = funcionarioAtualizado.Setor;
            funcionario.Sala = funcionarioAtualizado.Sala;
            funcionario.Telefone = funcionarioAtualizado.Telefone;
            funcionario.Endereco = funcionarioAtualizado.Endereco;
            funcionario.Cidade = funcionarioAtualizado.Cidade;
            funcionario.Bairro = funcionarioAtualizado.Bairro;
            funcionario.Numero = funcionarioAtualizado.Numero;
            funcionario.CEP = funcionarioAtualizado.CEP;
            funcionario.UF = funcionarioAtualizado.UF;

            _repository.SalvarDados(funcionarios);
            return Ok(funcionario);
        }

        [HttpDelete("{cpf}")]
        public IActionResult ExcluirFuncionario(string cpf)
        {
            var funcionarios = _repository.CarregarDados();
            var funcionario = funcionarios.FirstOrDefault(f => f.CPF == cpf);

            if (funcionario == null)
                return NotFound(new { message = "Funcionário não encontrado" });

            funcionarios.Remove(funcionario);
            _repository.SalvarDados(funcionarios);
            return Ok(new { message = "Funcionário excluído" });
        }
    }
}
