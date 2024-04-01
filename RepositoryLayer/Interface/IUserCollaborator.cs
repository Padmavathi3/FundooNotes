﻿using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IUserCollaborator
    {
        public Task AddCollaborator(string cid, string nid, string email);
        public Task<IEnumerable<UserCollaborator>> GetAllCollaborators();
        public Task<int> DeleteCollaborator(string cid,string nid);
    }
}