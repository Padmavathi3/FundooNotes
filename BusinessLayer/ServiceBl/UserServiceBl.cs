using BusinessLayer.InterfaceBl;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ServiceBl
{
    public class UserServiceBl:IUserBl
    {
        private readonly IUser person;

        public UserServiceBl(IUser person)
        {
            this.person = person;
        }

        //Insertion
        public Task Insertion(string firstname, string lastname, string emailid, string password)
        {
            return person.Insertion(firstname, lastname, emailid, password);
        }

        //get all users
        public Task<IEnumerable<User>> GetUsers()
        {
            return person.GetUsers();
        }

        //reset password
        public Task<int> ResetPasswordByEmail(string emailid, string newPassword)
        {
            return person.ResetPasswordByEmail(emailid, newPassword);
        }

        //get users by email
        public Task<IEnumerable<User>> GetUsersByEmail(string email)
        {
            return person.GetUsersByEmail(email);
        }


        //delete user
        public Task<int> DeleteUserByEmail(string email)
        {
            return person.DeleteUserByEmail(email);
        }

        //login
        public Task<IEnumerable<User>> Login(string email, string password)
        {
            return person.Login(email, password);
        }

        //forgot passsword
        public Task<String> ChangePasswordRequest(string Email)
        {
            return person.ChangePasswordRequest(Email);
        }
        public Task<string> ChangePassword(string otp, string password)
        {
            return person.ChangePassword(otp, password);
        }

        public Task<IEnumerable<User>> GetUsersByToken(string token)
        {
            return person.GetUsersByToken(token);
        }
    }
}
