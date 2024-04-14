using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IUserNote
    {
        public Task CreateNote(string id, string title, string description, DateTime reminder, string archieve, string pinned, string trash, string email);

        public Task<IEnumerable<UserNote>> GetAllNotes(string id);
        public Task<string> Update(string id, string emailid, string title, string description);
        public Task<string> DeleteNote(string id, string email);

       

    }
}
