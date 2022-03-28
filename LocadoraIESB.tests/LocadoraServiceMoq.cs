using System;
using System.Collections.Generic;
using System.Linq;
using LocadoraIESB.console.context;
using LocadoraIESB.console.Exceptions;
using LocadoraIESB.console.interfaces;
using LocadoraIESB.console.models;

namespace LocadoraIESB.console.services
{
    public class LocadoraServiceMoq : ILocadoraService
    {
        readonly DbLocadoraContext _context;
        
        
        //private static readonly LocadoraService _myLocadoraService = new LocadoraServiceMoq();
        //public static LocadoraService GetInstance() => _myLocadoraService;

        public LocadoraServiceMoq(DbLocadoraContext context)
        {
            _context = context;
        }
        
        public List<Carro> ListarCarros()
        {
            return _context.Carros.ToList();
        }

        public Carro CadastraCarro(Carro car)
        {

            _context.Carros.Add(car);
            _context.SaveChanges();
            var novoCarro = _context.Carros.Find(car.Id);
            return novoCarro;
        }

        public Cliente CadastraCliente(Cliente cliente)
        {
            _context.Add(cliente);
            _context.SaveChanges();
            var novoCliente = _context.Clientes.Find(cliente.Id);
            return novoCliente;
        }

        public List<Cliente> ListarClientes()
        {
            return _context.Clientes.ToList();
        }

        public List<Carro> ListarCarrosNaoAlugados()
        {
            var carrosDisponiveis = _context.Carros
                .Where(c => c.Cliente == null)
                .ToList();

            return carrosDisponiveis;
        }

        public List<Locacao> RelatorioLocacaos()
        {
            return _context.Locacoes.ToList();
        }
        public void LocarCarro(Carro car, Cliente cliente, DateTime dataInicio, DateTime dataFim)
        {
            
            
            var carro = _context.Carros.Find(car.Id);
            var locador = _context.Clientes.Find(cliente.Id);
            if (carro.Cliente != null)
            {
                throw new CarroAlugadoException($"Carro de placa {carro.Placa} ja alugado pelo Cliente {carro.Cliente.Nome}");
            }
            var list = new List<Carro>(){car};
            carro.Cliente = locador;
            Locacao locacao = new Locacao();
            locacao.Carros = list; 
            locacao.Cliente = cliente;
            locacao.DateTimeInicio = dataInicio;
            locacao.DateTimeFim = dataFim;
            _context.Locacoes.Add(locacao);
            _context.SaveChanges();
        }
    }
}