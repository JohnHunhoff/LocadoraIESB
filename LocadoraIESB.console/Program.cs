using System;
using System.Linq;
using LocadoraIESB.console.enums;
using LocadoraIESB.console.models;
using LocadoraIESB.console.services;
using LocadoraIESB.console.Util;

using static System.Console;

namespace LocadoraIESB.console
{
    class Program
    {
        
        private static LocadoraService _service = LocadoraService.GetInstance();
        
        static void Main(string[] args)
        {
           
            while (true)
            {
                var option = MainMenu();
                ExecutaAcao(option);
            }
        }
        


        private static void ExecutaAcao(Options option)
        {
            switch (option)
            {
                case Options.CadastraVeiculo:
                    CadastraVeiculo();
                    break;
                case Options.CadastraCliente:
                    CadastraCliente();
                    break;
                case Options.RealizaLocacao:
                    LocarCarro();
                    break;
                case Options.RelatorioLocacao:
                    RelatorioLocacao();
                    break;
                case Options.Sair:
                    System.Environment.Exit(0);
                    break;
            }
        }

        private static void RelatorioLocacao()
        {
            WriteLine("Carros Locados");
            foreach (var loc in _service.RelatorioLocacaos())
            {
                WriteLine("Locador");
                WriteLine(loc.Cliente);
                WriteLine("Carro");
                loc.Carros.ForEach(c => WriteLine(c));
                WriteLine($"Data de Inicio : {loc.DateTimeInicio:d}");
                WriteLine($"Data de Fim : {loc.DateTimeFim:d}");
                if(loc.DateTimeFim > DateTime.Now) WriteLine("Devolução Atrasada");

            }
        }

        private static void LocarCarro()
        {
            WriteLine("Digite o Cpf do cliente");
            var cpf = ReadLine();

            var cliente = _service.ListaClientes().FirstOrDefault(c => c.Cpf == cpf);
            WriteLine("Cliente: " + cliente.Nome);

            foreach (var c in _service.ListarCarrosNaoAlugados())
            {
                Console.WriteLine();
                Console.WriteLine(c);
                Console.WriteLine("##############################################");
            }
            
            WriteLine("Digite a placa de um dos carros acima");
            var placa = ReadLine();
            var carro = _service.ListarCarrosNaoAlugados().FirstOrDefault(c => c.Placa == placa);

            WriteLine("Digite a data de Devolucao no formato AAAA-MM-DD");
            var dateF = ReadLine();
            var dateList = dateF.Split("-");
            var dateFim = new DateTime(
                int.Parse(dateList[0]),
                int.Parse(dateList[1]),
                int.Parse(dateList[2]),
                23,
                59,
                59
            );
            
            WriteLine("Digite a data para retirada do carro AAAA-MM-DD, *Caso vá retirar agora pode digitar 1");
            var dateI = ReadLine();
            if (int.Parse(dateI) == 1)
            {
                var dateInicio = DateTime.Now;
                _service.LocarCarro(carro, cliente, dateInicio, dateFim);
            }
            else
            {
                var dateListInicio = dateI.Split("-");
                var dateInicio = new DateTime(
                    int.Parse(dateListInicio[0]),
                    int.Parse(dateListInicio[1]),
                    int.Parse(dateListInicio[2]),
                    23,
                    59,
                    59
                );
                _service.LocarCarro(carro, cliente, dateInicio, dateFim);
            }

        }

        private static void CadastraCliente()
        {
            WriteLine("Informe os dados do Cliente");
            WriteLine("Informe o Cpf");
            var cpf = ReadLine();
            
            WriteLine("Informe o Nome");
            var nome = ReadLine();
            
            WriteLine("Informe o RG");
            var rg = ReadLine();

            _service.CadastraCliente(new Cliente(nome, cpf, rg));
            Console.WriteLine("SUCESSO!!!");
        }

        private static void CadastraVeiculo()
        {
            WriteLine("Informe os valores ");
            WriteLine("Categoria do carro ");  
            foreach (var c in EnumUtil.GetValues<Categoria>()){ WriteLine($"{(int) c}) {c}");}
            var categoria = (Categoria)int.Parse(Console.ReadLine());
            
            WriteLine("Transmissão do carro ");  
            foreach (var t in EnumUtil.GetValues<Transmissao>()){ WriteLine($"{(int) t}) {t}");}
            var transmissao = (Transmissao)int.Parse(Console.ReadLine());
            
            WriteLine("Combustivel do carro ");  
            foreach (var c in EnumUtil.GetValues<Combustivel>()){ WriteLine($"{(int) c}) {c}");}
            var combustivel = (Combustivel)int.Parse(Console.ReadLine());
            
            WriteLine("Marca do carro");  
            foreach (var m in EnumUtil.GetValues<Marca>()){ WriteLine($"{(int) m}) {m}");}
            var marca = (Marca)int.Parse(Console.ReadLine());
            
            Write("Modelo do Carro: ");
            var modelo = ReadLine();
            WriteLine("\n");
            
            Write("Ano XXXX: ");
            var ano = int.Parse(ReadLine());
            WriteLine();
            
            Write("Placa do Veiculo: ");
            var placa = ReadLine();

            var X = _service.CadastraCarro(new Carro(

                categoria,
                transmissao,
                combustivel,
                marca,
                modelo,
                ano,
                placa
            ));
            
            Console.WriteLine("SUCESSO!!!");
        }

        private static Options MainMenu()
        {
            WriteLine(
                "##### SELECIONE A OPÇÃO DESEJADA ####\n" +
                "1) Cadastrar um Novo Veiculo\n" +
                "2) Cadastrar um Novo Cliente\n" +
                "3) Realizar a locação de um Veículo\n" +
                "4) Relatório de locação\n" +
                "5) Sair"
            );
            var option = Int32.Parse(ReadLine());
            return (Options) option;
        }

    }
}