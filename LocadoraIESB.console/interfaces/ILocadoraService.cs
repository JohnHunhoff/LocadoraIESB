using System.Collections.Generic;
using LocadoraIESB.console.models;

namespace LocadoraIESB.console.interfaces
{
    public interface ILocadoraService
    {
        public List<Carro> ListarCarros();
        public Carro CadastraCarro(Carro car);
        public Cliente CadastraCliente(Cliente cliente);
        public List<Cliente> ListaClientes();
    }
}