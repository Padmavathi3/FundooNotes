using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.CustomExceptions
{
    public class IdNotFoundException:Exception
    {
        public IdNotFoundException() { }
        public IdNotFoundException(string message) : base(message)
        {

        }
    }
}
