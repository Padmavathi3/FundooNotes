using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.CustomExceptions
{
    public  class EmailNotFoundException:Exception
    {
        public EmailNotFoundException() { }
        public EmailNotFoundException(string message) : base(message)
        {

        }
    }
}
