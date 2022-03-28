using System;

namespace LocadoraIESB.console.Exceptions
{
    public class CarroAlugadoException : Exception
    {
        public CarroAlugadoException()
        {
            
        }

        public CarroAlugadoException(string message)
            : base(message)
        {
            
        }
        
        public CarroAlugadoException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}