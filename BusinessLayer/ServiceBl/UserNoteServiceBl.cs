using BusinessLayer.InterfaceBl;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ServiceBl
{
    public class UserNoteServiceBl:IUserNoteBl
    {
        private readonly IUserNote person1;

        public UserNoteServiceBl(IUserNote person1)
        {
            this.person1 = person1;
        }

        public Task CreateNote(string id, string title, string description, DateTime reminder, string archieve, string pinned, string trash, string email)
        {
            return person1.CreateNote(id, title, description, reminder, archieve, pinned, trash, email);
        }

        public Task<IEnumerable<UserNote>> GetAllNotes(string id)
        {
            return person1.GetAllNotes(id);
        }

        public Task<int> Update(string id, string emailid, string title, string description)
        {
            return person1.Update(id,emailid, title, description);
        }

        public Task<int> DeleteNote(string id, string email)
        {
            return person1.DeleteNote(id,email);
        }

        public Task<int> MoveToTrash(string id, string emailid)
        {
            return person1.MoveToTrash(id,emailid);
        }
    }
}
