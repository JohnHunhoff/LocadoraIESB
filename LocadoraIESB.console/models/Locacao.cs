using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LocadoraIESB.console.models
{
    public class Locacao
    {
        public Locacao()
        {
        }



        [Key]
        public int Id { get; set; }
        public virtual Cliente Cliente{ get; set; }
        public virtual List<Carro> Carros  { get; set; }
        public DateTime DateTimeInicio { get; set; } = DateTime.Now;
        public DateTime DateTimeFim { get; set; }
        
    }
}