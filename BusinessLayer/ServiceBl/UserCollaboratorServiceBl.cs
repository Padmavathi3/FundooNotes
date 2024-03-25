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
    public class UserCollaboratorServiceBl: IUserCollaboratorBl
    {
        private readonly IUserCollaborator  person2;

        public UserCollaboratorServiceBl(IUserCollaborator person2)
        {
            this.person2 = person2;
        }

        //Insertion
        public Task AddCollaborator(string cid, string nid, string email)
        {
            return person2.AddCollaborator(cid, nid, email);
        }

        //display all collaborators
        public Task<IEnumerable<UserCollaborator>> GetAllCollaborators()
        {
            return person2.GetAllCollaborators();
        }

        //delete
        public Task<int> DeleteCollaborator(string cid, string nid)
        {
            return (person2.DeleteCollaborator(cid,nid));
        }
    }
}
