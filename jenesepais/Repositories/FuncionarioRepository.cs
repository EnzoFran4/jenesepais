using FuncionarioAPI.Models;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace FuncionarioAPI.Repositories
{
    public class FuncionarioRepository
    {
        private const string FilePath = "funcionarios.json";

        public List<Funcionario> CarregarDados()
        {
            if (!File.Exists(FilePath))
                return new List<Funcionario>();

            var jsonData = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<List<Funcionario>>(jsonData) ?? new List<Funcionario>();
        }

        public void SalvarDados(List<Funcionario> funcionarios)
        {
            var jsonData = JsonConvert.SerializeObject(funcionarios, Formatting.Indented);
            File.WriteAllText(FilePath, jsonData);
        }

        public bool ValidarCPF(string cpf)
        {
            // Lógica simplificada para validação de CPF
            return Regex.IsMatch(cpf, @"^\d{11}$");
        }
    }
}
