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
    public class UserCollaboratorServiceBl: IUserCollaboratorBl
    {
        private readonly IUserCollaborator  collaborator;

        public UserCollaboratorServiceBl(IUserCollaborator person2)
        {
            this.collaborator = person2;
        }

        //Insertion
        public Task AddCollaborator(string cid, string nid, string email)
        {
            return collaborator.AddCollaborator(cid, nid, email);
        }

        //display all collaborators
        public Task<IEnumerable<UserCollaborator>> GetAllCollaborators()
        {
            return collaborator.GetAllCollaborators();
        }

        //delete
        public Task<string> DeleteCollaborator(string cid, string nid)
        {
            return (collaborator.DeleteCollaborator(cid,nid));
        }
    }
}
