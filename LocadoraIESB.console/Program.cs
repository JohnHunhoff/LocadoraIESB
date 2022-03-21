using System;
using LocadoraIESB.console.interfaces;
using LocadoraIESB.console.services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LocadoraIESB.console
{
    class Program
    {
        private static LocadoraService _service = LocadoraService.GetInstance();
        

        static void Main(string[] args)
        {
            
        }
    }
}