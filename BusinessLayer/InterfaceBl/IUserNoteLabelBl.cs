using ModelLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.InterfaceBl
{
    public interface IUserNoteLabelBl
    {
        //Add Label
        public Task CreateLabel(string id, string name, string email);

        //Get
        public Task<IEnumerable<UserNoteLabel>> GetUserNoteLabels();

        //update label name
        public Task<string> UpdateName(string name, string id);

        //delete
        public Task<string> DeleteLabel(string name, string id);
    }
}
