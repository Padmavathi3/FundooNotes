using RepositoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IUser
    {
        //Insertion
        public Task Insertion(string firstname, string lastname, string emailid, string password);

        //Get
        public Task<IEnumerable<User>> GetUsers();

       //Reset Password
        public Task<int> ResetPasswordByEmail(string emailid, string newPassword);

       //Get user details based on email
        public Task<IEnumerable<User>> GetUsersByEmail(string email);

        //delete
        public Task<int> DeleteUserByEmail(string email);

        //login
        public Task<IEnumerable<User>> Login(string email, string password);
        
        //forgotPassword

        public Task<String> ChangePasswordRequest(string Email);
        public Task<string> ChangePassword(string otp, string password);

        //get details baseod on token
        public Task<IEnumerable<User>> GetUsersByToken(string token);
    }
}
