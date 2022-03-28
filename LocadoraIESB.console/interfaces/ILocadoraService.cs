using System;
using System.Collections.Generic;
using LocadoraIESB.console.models;

namespace LocadoraIESB.console.interfaces
{
    public interface ILocadoraService
    {
        public List<Carro> ListarCarros();
        public Carro CadastraCarro(Carro car);
        public Cliente CadastraCliente(Cliente cliente);
        public List<Cliente> ListarClientes();
        public List<Carro> ListarCarrosNaoAlugados();
        public void LocarCarro(Carro car, Cliente cliente, DateTime dataInicio, DateTime dataFim);
        public List<Locacao> RelatorioLocacaos();

    }
}