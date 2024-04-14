using BusinessLayer.InterfaceBl;
using ModelLayer.Entities;
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
        private readonly IUserNote note;

        public UserNoteServiceBl(IUserNote note)
        {
            this.note = note;
        }

        public Task CreateNote(string id, string title, string description, DateTime reminder, string archieve, string pinned, string trash, string email)
        {
            return note.CreateNote(id, title, description, reminder, archieve, pinned, trash, email);
        }

        public Task<IEnumerable<UserNote>> GetAllNotes(string id)
        {
            return note.GetAllNotes(id);
        }

        public Task<string> Update(string id, string emailid, string title, string description)
        {
            return note.Update(id,emailid, title, description);
        }

        public Task<string> DeleteNote(string id, string email)
        {
            return note.DeleteNote(id,email);
        }

       
    }
}
