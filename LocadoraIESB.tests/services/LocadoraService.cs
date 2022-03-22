using System;
using System.Linq;
using LocadoraIESB.console.context;
using LocadoraIESB.console.models;
using LocadoraIESB.console.services;
using Microsoft.EntityFrameworkCore;
using LocadoraIESB.console.enums;
using Xunit;

namespace LocadoraIESB.tests.services
{
    public class LocadoraService
    {
        [Fact]
        public void QuandoMetodoCadastraCarroForInvocadoDevePersistirDadosNoBanco()
        {
            // arrange
            var opt = new DbContextOptionsBuilder<DbLocadoraContext>()
                .UseInMemoryDatabase("DbLocadora")
                .Options;
            var context = new DbLocadoraContext(opt);
            var service = new LocadoraServiceMoq(context);
            var car = new Carro(0, 0, 0, 0, "BMW", 2022, "123fasdv");
            
            // act
            var carroCadastrado = service.CadastraCarro(car);
            
            // assert
            var carroInDb = context.Carros.Find(carroCadastrado.Id);
            Assert.Equal(carroCadastrado, carroInDb);

        }

        [Fact]
        public void DadoDoisCarrosComPlacaIgualDeveLacarException()
        {
            // arrange
            var opt = new DbContextOptionsBuilder<DbLocadoraContext>()
                .UseInMemoryDatabase("DbLocadoraError")
                .Options;
            var context = new DbLocadoraContext(opt);
            var service = new LocadoraServiceMoq(context);
            var car1 = new Carro(0, 0, 0, 0, "BMW", 2022, "123fasdv");
            var car2 = new Carro(0, 0, 0, 0, "Mercedes", 2022, "123fasdv");
            // act
            service.CadastraCarro(car1);
            
            // assert
            Assert.Throws<InvalidOperationException>(() => service.CadastraCarro(car2));
        }
        
        [Fact]
        public void DadoCarroJaAlugadoClienteDeveSerRegistradoComoLocador()
        {
            // arrange
            var opt = new DbContextOptionsBuilder<DbLocadoraContext>()
                .UseInMemoryDatabase("DbLocadoraLocador")
                .Options;
            var context = new DbLocadoraContext(opt);
            var service = new LocadoraServiceMoq(context);
            var car1 = new Carro(0, 0, 0, 0, "BMW", 2022, "1234test");
            var cliente = new Cliente("João da Silva", "05635626168", "5835721");
            var clienteCadastrado = service.CadastraCliente(cliente);
            var carroCadastrado = service.CadastraCarro(car1);
            
            
            
            // act
            service.LocarCarro(carroCadastrado, clienteCadastrado);
            var listaCarrosNaoAlugados = service.ListarCarros();
            var locador = listaCarrosNaoAlugados.Where(c => c.Placa == carroCadastrado.Placa)
                .Select(c => c.Cliente)
                .FirstOrDefault(c => cliente.Cpf == clienteCadastrado.Cpf);


            // assert
            Assert.Equal(clienteCadastrado.Cpf, locador.Cpf);
        }

        [Fact]
        public void DadoCarroJaAlugadoNaoDeveSerListadoNoMetodoListarCarrosNaoAlugados()
        {
            // arrange
            var opt = new DbContextOptionsBuilder<DbLocadoraContext>()
                .UseInMemoryDatabase("DbLocadoraAluguel")
                .Options;
            var context = new DbLocadoraContext(opt);
            var service = new LocadoraServiceMoq(context);
            var car1 = new Carro(0, 0, 0, 0, "BMW", 2022, "1234test");
            var car2 = new Carro(0, 0, 0, 0, "Mercedes", 2022, "4321test");
            var cliente = new Cliente("João da Silva", "05635626168", "5835721");
            var clienteCadastrado = service.CadastraCliente(cliente);
            var carroCadastrado = service.CadastraCarro(car1);
            
            service.CadastraCarro(car2);
            
            // act
            service.LocarCarro(carroCadastrado, clienteCadastrado);
            var listaCarrosNaoAlugados = service.ListarCarrosNaoAlugados();
            var carroAlugado = listaCarrosNaoAlugados.Where(c => c.Cliente != null);

            // assert
            Assert.Empty(carroAlugado);
        }
    }
}