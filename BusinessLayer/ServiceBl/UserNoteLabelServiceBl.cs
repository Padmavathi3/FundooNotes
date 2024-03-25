using BusinessLayer.InterfaceBl;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ServiceBl
{
    public class UserNoteLabelServiceBl:IUserNoteLabelBl
    {
        private readonly IUserNoteLabel person3;

        public UserNoteLabelServiceBl(IUserNoteLabel person3)
        {
            this.person3 = person3;
        }

        public Task CreateLabel(string id, string name, string email)
        {
            return person3.CreateLabel(id, name, email);
        }

        public Task<IEnumerable<UserNoteLabel>> GetUserNoteLabels()
        {
            return person3.GetUserNoteLabels();
        }

        public Task<int> UpdateName(string name, string id)
        {
            return person3.UpdateName(name, id);
        }

        public Task<int> DeleteLabel(string name, string id)
        {
            return person3.DeleteLabel(name, id);
        }
    }
}
