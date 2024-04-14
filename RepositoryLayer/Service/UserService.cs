using Dapper;
using ModelLayer.Entities;
using RepositoryLayer.Context;
using RepositoryLayer.CustomExceptions;
using RepositoryLayer.Exceptions;
using RepositoryLayer.Interface;
using RepositoryLayer.NestdMethodsFolder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class UserService:IUser
    {
        private readonly DapperContext _context;
        private static string otp;
        private static string mailid;
        private static User entity;
        public UserService(DapperContext context)
        {
            _context = context;
        }

        //Logic for inserting records
        public async Task<int> Insertion(string firstname, string lastname, string emailid, string password)
        {
            var query = "insert into Person(FirstName, LastName, EmailId, Password) values(@FirstName, @LastName, @EmailId, @Password)";

            string encryptedPassword = NestedMethodsClass.EncryptPassword(password);

            var parameters = new DynamicParameters();
            parameters.Add("@FirstName", firstname, DbType.String);
            parameters.Add("@LastName", lastname, DbType.String);
            parameters.Add("@EmailId", emailid, DbType.String);
            parameters.Add("@Password", encryptedPassword, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(query, parameters);
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------

        //logic for Display the all users

        public async Task<IEnumerable<User>> GetUsers()
        {
            var query = "SELECT * FROM Person";

            using (var connection = _context.CreateConnection())
            {
                var persons = await connection.QueryAsync<User>(query);

                if (persons.Any())
                {
                    return persons;
                }
                else
                {
                    throw new EmptyListException("No user is present in the table.");
                }
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------

        //update password using email

        private async Task<string> ResetPasswordByEmail(string emailid, string newPassword)
        {
            var users = await GetUsersByEmail(emailid);
            if (!users.Any())
            {
                // If no users are found with the given email, throw custom exception
                throw new EmailNotFoundException("Email does not exist.");
            }
            else
            {  
                var query = "UPDATE Person SET Password = @NewPassword WHERE EmailId = @Email";
                var parameters = new DynamicParameters();
                parameters.Add("@NewPassword", newPassword, DbType.String);
                parameters.Add("@Email", emailid, DbType.String);
                int rowsAffected = 0;
                using (var connection = _context.CreateConnection())
                {

                    rowsAffected = await connection.ExecuteAsync(query, parameters);
                    if (rowsAffected > 0)
                    {
                        return $"password is updated";
                    }
                    else
                    {
                        throw new ParameterException("Password must be required..........");
                    }


                }
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------

        //Get the user details based on email

        public async Task<IEnumerable<User>> GetUsersByEmail(string email)
        {
            var query = "select * from Person WHERE EmailId = @EmailId";
            using (var connection = _context.CreateConnection())
            {
                var persons = await connection.QueryAsync<User>(query, new { EmailId = email });
                if (persons.Any())
                {
                    return persons;
                }
                else
                {
                    throw new EmptyListException("No user is present in the table.");
                }
            }

        }
        //------------------------------------------------------------------------------------------------------------------------------

        //delete the user based on email

        public async Task<string> DeleteUserByEmail(string email)
        {
            var users = await GetUsersByEmail(email);
            int rowsAffected = 0;
            if (!users.Any())
            {
                // If no users are found with the given email, throw custom exception
                throw new EmailNotFoundException("Email does not exist.");
            }
            else
            {
                var query = "delete from Person where EmailId =@EmailId";
                using (var connection = _context.CreateConnection())
                {
                    rowsAffected=await connection.ExecuteAsync(query, new { EmailId = email });
                    if (rowsAffected > 0)
                    {
                        return $"rowsAffected user is deleted";
                    }
                    else
                    {
                        throw new NoRowEffected("LogOut is not done successfully..........");
                    }

                }
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------
        //login
        public async Task<IEnumerable<User>> Login(string email, string password)
        {

            var query = "SELECT * FROM Person WHERE EmailId = @EmailId";

            using (var connection = _context.CreateConnection())
            {
                var users = await connection.QueryAsync<User>(query, new { EmailId = email });

                // Iterate through each user retrieved
                if (users.Any())
                {
                    foreach (var user in users)
                    {
                        // Decrypt the stored password
                        string storedPassword = NestedMethodsClass.DecryptPassword(user.Password);

                        // Compare the decrypted stored password with the encrypted provided password
                        if (password == storedPassword)
                        {
                            // If matched, return a list containing the matched user
                            return new List<User> { user };

                        }
                        else
                        {
                            throw new PasswordMissmatchException("passkey not match");
                        }
                    }
                }
                else
                    throw new UserNotFoundException("user not found in data base please create account");

                // If no matching user is found, return an empty list
                return Enumerable.Empty<User>();
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------

        public Task<String> ChangePasswordRequest(string Email)
        {
            try
            {
                entity = GetUsersByEmail(Email).Result.FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new UserNotFoundException("UserNotFoundByEmailId" + e.Message);
            }

            string generatedotp = "";
            Random r = new Random();

            for (int i = 0; i < 6; i++)
            {
                generatedotp += r.Next(0, 10);
            }
            otp = generatedotp;
            mailid = Email;
            NestedMethodsClass.sendMail(Email, generatedotp);
            Console.WriteLine(otp);
            return Task.FromResult("MailSent ✔️");

        }

        //----------------------------------------------------------------------------------------------------------------------

        public async Task<string> ChangePassword(string otp, string password)
        {
            if (string.IsNullOrEmpty(otp))
            {
                return "Generate OTP First";
            }

            if (NestedMethodsClass.DecryptPassword(entity.Password).Equals(password))
            {
                throw new PasswordMissmatchException("Don't give the existing password");
            }

            if (!Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[a-zA-Z\d!@#$%^&*]{8,16}$"))
            {
                return "Password does not meet complexity requirements";
            }

            if (!otp.Equals(otp))
            {
                return "OTP mismatch";
            }

            try
            {
                var result = await ResetPasswordByEmail(mailid, NestedMethodsClass.EncryptPassword(password));
                entity = null;
                otp = null;
                mailid = null;
                return result;
            }
            catch (EmailNotFoundException ex)
            {
                return ex.Message;
            }
            catch (ParameterException ex)
            {
                return ex.Message;
            }
        }


    }
}
