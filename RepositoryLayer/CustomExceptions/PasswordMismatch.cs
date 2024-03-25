using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Exceptions
{
    public class PasswordMissmatchException : Exception
    {
        public PasswordMissmatchException()
        {

        }
        public PasswordMissmatchException(string message) : base(message)
        {

        }
    }
}
