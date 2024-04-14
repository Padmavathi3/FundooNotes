using BusinessLayer.InterfaceBl;
using ModelLayer.Entities;
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
        private readonly IUserNoteLabel label;

        public UserNoteLabelServiceBl(IUserNoteLabel person3)
        {
            this.label = person3;
        }

        public Task CreateLabel(string id, string name, string email)
        {
            return label.CreateLabel(id, name, email);
        }

        public Task<IEnumerable<UserNoteLabel>> GetUserNoteLabels()
        {
            return label.GetUserNoteLabels();
        }

        public Task<string> UpdateName(string name, string id)
        {
            return label.UpdateName(name, id);
        }

        public Task<string> DeleteLabel(string name, string id)
        {
            return label.DeleteLabel(name, id);
        }
    }
}
