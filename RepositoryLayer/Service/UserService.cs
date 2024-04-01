using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.CustomExceptions;
using RepositoryLayer.Entities;
using RepositoryLayer.Exceptions;
using RepositoryLayer.Interface;
using RepositoryLayer.NestdMethodsFolder;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
        public async Task Insertion(string firstname, string lastname, string emailid, string password)
        {
           
                if (string.IsNullOrEmpty(firstname) || string.IsNullOrEmpty(lastname) || string.IsNullOrEmpty(emailid) || string.IsNullOrEmpty(password))
                {
                    throw new ArgumentsException("All parameters (firstname, lastname, emailid, password) are required..........");
                }

                var query = "insert into Person(FirstName,LastName,EmailId,Password) values(@FirstName,@LastName,@EmailId,@Password)";

                string encryptedPassword = NestedMethodsClass.EncryptPassword(password);

                var parameters = new DynamicParameters();
                parameters.Add("@FirstName", firstname, DbType.String);
                parameters.Add("@LastName", lastname, DbType.String);
                parameters.Add("@EmailId", emailid, DbType.String);
                parameters.Add("@Password", encryptedPassword, DbType.String);

                using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(query, parameters);
                }
            
           
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------

        //logic for Display the all users

        public async Task<IEnumerable<User>> GetUsers()
        {
            var query = "SELECT * FROM Person";


            using (var connection = _context.CreateConnection())
            {
                var person = await connection.QueryAsync<User>(query);
                if (person != null)
                {
                    
                    return person.ToList();
                }
                else
                {
                    //return Enumerable.Empty<User>();
                    throw new EmptyListException("no one user is present in table.............");
                }

            }

        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------

        //update password using email

        public async Task<int> ResetPasswordByEmail(string emailid, string newPassword)
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
                        return rowsAffected;
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
                var person = await connection.QueryAsync<User>(query, new { EmailId = email });
                return person.ToList(); // Assuming there's only one user per email
            }

        }
        //------------------------------------------------------------------------------------------------------------------------------

        //delete the user based on email

        public async Task<int> DeleteUserByEmail(string email)
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
                        return rowsAffected;
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
        public Task<string> ChangePassword(string otp, string password)
        {
            if (otp.Equals(null))
            {
                return Task.FromResult("Generate Otp First");
            }
            if (NestedMethodsClass.DecryptPassword(entity.Password).Equals(password))
            {
                throw new PasswordMissmatchException("Dont give the existing password");
            }

            if (Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[a-zA-Z\d!@#$%^&*]{8,16}$"))
            {
                if (otp.Equals(otp))
                {
                    if (ResetPasswordByEmail(mailid, NestedMethodsClass.EncryptPassword(password)).Result == 1)
                    {
                        entity = null; otp = null; mailid = null;
                        return Task.FromResult("password changed successfully");
                    }
                }
                else
                {
                    return Task.FromResult("otp miss matching");
                }
            }
            else
            {
                return Task.FromResult("regex is mismatching");
            }
            return Task.FromResult("password not changed");

        }
        //----------------------------------------------------------------------------------------------------------------------------------

        public async Task<IEnumerable<User>> GetUsersByToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("HJWD248JBFKHGSKNasdfghjkl!@#$%^&*3456789!nr")),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            try
            {
                // Validate token and extract claims
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                string userEmail = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                // Retrieve user from database based on email
                /*var users = await GetUsersByEmail(email);
                return users;*/
                var users = await GetUsersByEmail(userEmail);
                return users;
            }
            catch (Exception ex)
            {
                // Handle token validation errors
                // Log the exception
                throw;
            }
        }
    }
    
}
