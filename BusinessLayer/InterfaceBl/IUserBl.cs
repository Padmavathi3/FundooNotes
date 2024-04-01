using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.InterfaceBl
{
    public interface IUserBl
    {
        //Insertion
        public Task Insertion(string firstname, string lastname, string emailid, string password);

        //Get
        public Task<IEnumerable<User>> GetUsers();

        //dispay user values based on email
        public Task<IEnumerable<User>> GetUsersByEmail(string email);

        //reset password
        public Task<int> ResetPasswordByEmail(string emailid, string newPassword);

        //delete
        public Task<int> DeleteUserByEmail(string email);

        //login
        public Task<IEnumerable<User>> Login(string email, string password);

        //forgotPassword

        public Task<String> ChangePasswordRequest(string Email);
        public Task<string> ChangePassword(string otp, string password);

        public Task<IEnumerable<User>> GetUsersByToken(string token);

    }
}
