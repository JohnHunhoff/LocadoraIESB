using System.Collections.Generic;

namespace LocadoraIESB.console.models
{
    public class Cliente
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public virtual List<Carro> Carros { get; set; }
    }
}