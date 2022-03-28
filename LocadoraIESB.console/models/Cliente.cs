using System.Collections.Generic;
namespace LocadoraIESB.console.models
{
    public class Cliente
    {
        public Cliente(string nome, string cpf, string rg)
        {
            Nome = nome;
            Cpf = cpf;
            Rg = rg;
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public virtual List<Carro> Carros { get; set; }
        
        public override string ToString()
        {
            return $"Nome: {Nome}" +
                   $" | Cpf: {Cpf}";
        }
    }
}