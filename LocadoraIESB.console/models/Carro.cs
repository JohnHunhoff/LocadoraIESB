using System.ComponentModel.DataAnnotations;
using LocadoraIESB.console.enums;

namespace LocadoraIESB.console.models
{
    public class Carro
    {
        [Key]
        public int Id { get; set; }
        public Categoria Categoria { get; set; }
        public Transmissao Transmissao { get; set; }
        public Combustivel Combustivel { get; set; }
        public Marca Marca { get; set; }
        public string Modelo { get; set; }
        public int Year { get; set; }
        public string Placa { get; set; }
        public virtual Cliente Cliente { get; set; }
        
        public Carro(Categoria categoria, Transmissao transmissao, Combustivel combustivel, Marca marca, string modelo, int year, string placa)
        {
            Categoria = categoria;
            Transmissao = transmissao;
            Combustivel = combustivel;
            Marca = marca;
            Modelo = modelo;
            Year = year;
            Placa = placa;
        }

        public override string ToString()
        {
            return $"Placa: {Placa}" +
                   $"| Ano: {Year}" +
                   $"| Modelo: {Modelo}" +
                   $"| Combustivel: {Combustivel}" +
                   $"| Transmissao: {Transmissao}";
        }
    }
}