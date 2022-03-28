using System;
using System.Linq;
using LocadoraIESB.console.context;
using LocadoraIESB.console.models;
using LocadoraIESB.console.services;
using Microsoft.EntityFrameworkCore;
using LocadoraIESB.console.enums;
using LocadoraIESB.console.Exceptions;
using Moq;
using Xunit;

namespace LocadoraIESB.tests.services
{
    public class LocadoraService
    {
        [Fact]
        public void DadoQuatroClientesETresCarrosUmNaoDeveConseguirAlugarCarro()
        {
            // arrange
            var opt = new DbContextOptionsBuilder<DbLocadoraContext>()
                .UseInMemoryDatabase("DbLocadoraAluguelTesteLocacao")
                .Options;
            var context = new DbLocadoraContext(opt);
            var service = new LocadoraServiceMoq(context);
            var car1 = new Carro(0, 0, 0, 0, "BMW", 2022, "1234test");
            var car2 = new Carro(0, 0, 0, 0, "Mercedes", 2022, "4321test");
            var car3 = new Carro(0, 0, 0, 0, "Hilux", 2022, "1234kkkk");
            

            var cliente1 = new Cliente("João do Teste", "05635626168", "5835721");
            var cliente2 = new Cliente("Moq do Joao", "05633216168", "5875621");
            var cliente3 = new Cliente("John of fake", "05656786168", "5111121");
            var cliente4 = new Cliente("Dummy", "11156786168", "1111121");


            var clienteCadastrado1 = service.CadastraCliente(cliente1);
            var clienteCadastrado2 = service.CadastraCliente(cliente2);
            var clienteCadastrado3 = service.CadastraCliente(cliente3);
            var clienteCadastrado4 = service.CadastraCliente(cliente4);
            
            
            var carroCadastrado1 = service.CadastraCarro(car1);
            var carroCadastrado2 = service.CadastraCarro(car2);
            var carroCadastrado3 = service.CadastraCarro(car3);
            
            //act
            service.LocarCarro(carroCadastrado1, 
                clienteCadastrado1, 
                DateTime.Now, 
                new DateTime(2022, 5, 1, 8, 30 ,52));
            
            service.LocarCarro(carroCadastrado2, 
                clienteCadastrado2, 
                DateTime.Now, 
                new DateTime(2022, 5, 1, 8, 30 ,52));
            
            service.LocarCarro(carroCadastrado3, 
                clienteCadastrado3, 
                DateTime.Now, 
                new DateTime(2022, 5, 1, 8, 30 ,52));



            Assert.Throws<CarroAlugadoException>(
                () => service.LocarCarro(carroCadastrado3,
                    clienteCadastrado4,
                    DateTime.Now,
                    new DateTime(2022, 5, 1, 8, 30, 52))
            );
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
            service.LocarCarro(carroCadastrado, 
                clienteCadastrado, 
                DateTime.Now, 
                new DateTime(2022, 5, 1, 8, 30 ,52));
            var listaCarrosNaoAlugados = service.ListarCarrosNaoAlugados();
            var carroAlugado = listaCarrosNaoAlugados.Where(c => c.Cliente != null);

            // assert
            Assert.Empty(carroAlugado);
        }
        
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
            service.LocarCarro(carroCadastrado, 
                clienteCadastrado, 
                DateTime.Now,
                new DateTime(2022, 5, 1, 8, 30 ,52));
            var listaCarrosNaoAlugados = service.ListarCarros();
            var locador = listaCarrosNaoAlugados.Where(c => c.Placa == carroCadastrado.Placa)
                .Select(c => c.Cliente)
                .FirstOrDefault(c => cliente.Cpf == clienteCadastrado.Cpf);


            // assert
            Assert.Equal(clienteCadastrado.Cpf, locador.Cpf);
        }
        
        [Fact]
        public void QuandoCarroJaAlugadoDeveConstarNaListaDeCarrosAlugadosDoCliente()
        {
            // arrange
            var opt = new DbContextOptionsBuilder<DbLocadoraContext>()
                .UseInMemoryDatabase("DbLocadoraLocadorLista")
                .Options;
            var context = new DbLocadoraContext(opt);
            var service = new LocadoraServiceMoq(context);
            var car1 = new Carro(0, 0, 0, 0, "BMW", 2022, "1234test");
            var cliente = new Cliente("João da Silva", "05635626168", "5835721");
            var clienteCadastrado = service.CadastraCliente(cliente);
            var carroCadastrado = service.CadastraCarro(car1);
            
            
            
            // act
            service.LocarCarro(carroCadastrado, 
                clienteCadastrado, 
                DateTime.Now, 
                new DateTime(2022, 5, 1, 8, 30 ,52));
            var clientePosLocacao = context.Clientes.Find(carroCadastrado.Id);
            var carroCliente = clientePosLocacao.Carros
                .Find(c => c.Placa == carroCadastrado.Placa);


            // assert
            Assert.Equal(carroCadastrado, carroCliente);
        }
        
        [Fact]
        public void QuandoClienteCadastradoDeveAparecerNoMetodoListaDeClientes()
        {
            // arrange
            var opt = new DbContextOptionsBuilder<DbLocadoraContext>()
                .UseInMemoryDatabase("DbLocadoraCadCliente")
                .Options;
            var context = new DbLocadoraContext(opt);
            var service = new LocadoraServiceMoq(context);
            
            var cliente = new Cliente("João da Silva", "05635626168", "5835721");
            var clienteCadastrado = service.CadastraCliente(cliente);



            // act
            var clienteLista = service.ListarClientes()
                .FirstOrDefault(c => c.Cpf == cliente.Cpf);


            // assert
            Assert.Equal(cliente, clienteLista);
        }
        
        [Fact]
        public void QuandoClienteCadastradoComCpfJaExistenteDeveLancarException()
        {
            // arrange
            var opt = new DbContextOptionsBuilder<DbLocadoraContext>()
                .UseInMemoryDatabase("DbLocadoraCadClienteDuplicado")
                .Options;
            var context = new DbLocadoraContext(opt);
            var service = new LocadoraServiceMoq(context);
            
            var cliente1 = new Cliente("João da Silva", "05635626168", "5835721");
            var cliente2 = new Cliente("João duplicado", "05635626168", "58235721");
            
            // act
            service.CadastraCliente(cliente1);


            // assert
            Assert.Throws<InvalidOperationException>(() => service.CadastraCliente(cliente2));
        }
    }
}