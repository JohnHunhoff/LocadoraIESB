using System;
using System.Collections.Generic;
using System.Linq;
using LocadoraIESB.console.context;
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
            throw new NotImplementedException();
        }

        public List<Carro> ListarCarrosNaoAlugados()
        {
            throw new NotImplementedException();
        }

        public void LocarCarro(Carro car, Cliente cliente)
        {
            throw new NotImplementedException();
        }

        public List<Cliente> ListaClientes()
        {
            return _context.Clientes.ToList();
        }
        
    }
}