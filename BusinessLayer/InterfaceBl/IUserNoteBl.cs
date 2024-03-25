using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.InterfaceBl
{
    public interface IUserNoteBl
    {
        public Task CreateNote(string id, string title, string description, DateTime reminder, string archieve, string pinned, string trash, string email);

        public Task<IEnumerable<UserNote>> GetAllNotes(string id);
        public Task<int> Update(string id, string emailid, string title, string description);
        public Task<int> DeleteNote(string id, string email);
        public Task<int> MoveToTrash(string id, string emailid);
    }
}
