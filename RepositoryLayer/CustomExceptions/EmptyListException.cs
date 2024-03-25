using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.CustomExceptions
{
    public class EmptyListException:Exception
    {
        public EmptyListException() { }
        public EmptyListException(string message) : base(message)
        {

        }
    }
}
