using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.CustomExceptions
{
    public class ArgumentsException : Exception
    {
        public ArgumentsException() { }
        public ArgumentsException(string message) : base(message)
        {

        }
    }
}
