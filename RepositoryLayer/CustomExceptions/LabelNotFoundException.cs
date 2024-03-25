using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.CustomExceptions
{
    public class LabelNotFoundException:Exception
    {
        public LabelNotFoundException() { }
        public LabelNotFoundException(string message) : base(message)
        {

        }
    }
}
