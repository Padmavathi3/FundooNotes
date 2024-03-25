using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IUserNoteLabel
    {
        //Add Label
        public Task CreateLabel(string id,string name,string email);

        //Get
        public Task<IEnumerable<UserNoteLabel>> GetUserNoteLabels();

        //update label name
        public Task<int> UpdateName(string name, string id);

        //delete
        public Task<int> DeleteLabel(string name,string id);
    }
}
