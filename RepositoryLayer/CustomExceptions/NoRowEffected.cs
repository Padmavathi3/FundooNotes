using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.CustomExceptions
{
    public class NoRowEffected:Exception
    {
        public NoRowEffected() { }
        public NoRowEffected(string message) : base(message)
        {

        }
    }
}
