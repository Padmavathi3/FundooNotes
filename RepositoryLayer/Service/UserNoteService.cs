using Dapper;
using RepositoryLayer.Context;
using RepositoryLayer.CustomExceptions;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;
using RepositoryLayer.NestdMethodsFolder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class UserNoteService : IUserNote
    {
        private readonly DapperContext _context;
        public UserNoteService(DapperContext context)
        {
            _context = context;
        }

        //Logic for inserting records
        public async Task CreateNote(string id, string title, string description, DateTime reminder, string archieve, string pinned, string trash, string email)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description) || reminder == null || string.IsNullOrEmpty(archieve) || string.IsNullOrEmpty(pinned) || string.IsNullOrEmpty(trash))
            {
                throw new ArgumentsException("All parameters are required..........");
            }

            var query = "insert into UserNote(NoteId,Title,Description,reminder,isArchive,isPinned,isTrash,EmailId) values(@NoteId,@Title,@Description,@reminder,@isArchive,@isPinned,@isTrash,@EmailId)";


            var parameters = new DynamicParameters();
            parameters.Add("@NoteId", id, DbType.String);
            parameters.Add("@Title", title, DbType.String);
            parameters.Add("@Description", description, DbType.String);
            parameters.Add("@reminder", reminder, DbType.DateTime);
            parameters.Add("@isArchive", archieve, DbType.String);
            parameters.Add("@isPinned", pinned, DbType.String);
            parameters.Add("@isTrash", trash, DbType.String);
            parameters.Add("@EmailId", email, DbType.String);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------

        //Get the user note details based on id

        public async Task<IEnumerable<UserNote>> GetAllNotes(string id)
        {
            var query = "select * from UserNote WHERE NoteId = @NoteId";
            using (var connection = _context.CreateConnection())
            {
                var person = await connection.QueryAsync<UserNote>(query, new { NoteId = id });
                return person.ToList(); 
            }

        }
        //------------------------------------------------------------------------------------------------------------------------------

        //update

        public async Task<int> Update(string id,string emailid, string title,string description)
        {
            var users = await GetAllNotes(id);
            var check_email= "SELECT COUNT(*) FROM UserNote WHERE EmailId = @EmailId";
            if (!users.Any())
            {
                // If no users are found with the given email, throw custom exception
                throw new IdNotFoundException("note id does not exist.");
            }
            else
            {


                var query = "UPDATE UserNote SET Title = @NewTitle,Description=@NewDescription WHERE EmailId = @EmailId and NoteId=@NoteId";
                var parameters = new DynamicParameters();
                parameters.Add("@NewTitle", title, DbType.String);
                parameters.Add("@NewDescription", description, DbType.String);
                parameters.Add("@EmailId",emailid, DbType.String);
                parameters.Add("@NoteId",id, DbType.String);    

                int rowsAffected = 0;
                using (var connection = _context.CreateConnection())
                {
                    int emailCount = await connection.ExecuteScalarAsync<int>(check_email, new { EmailId = emailid });

                    if (emailCount == 0)
                    {
                        throw new EmailNotFoundException($"email '{emailid}' Is Not A Registerd User please Register First and try Again.");
                    }
                    rowsAffected = await connection.ExecuteAsync(query, parameters);
                    if (rowsAffected > 0)
                    {
                        return rowsAffected;
                    }
                    else
                    {
                        throw new ParameterException("title and description must be required..........");
                    }


                }
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------

        //delete the userNote based on email and user note id

        public async Task<int> DeleteNote(string id,string email)
        {
            var users = await GetAllNotes(id);
            var check_email = "SELECT COUNT(*) FROM UserNote WHERE EmailId = @EmailId";
            int rowsAffected = 0;
            if (!users.Any())
            {
                // If no users are found with the given email, throw custom exception
                throw new IdNotFoundException("noteId does not exist.");
            }
            else
            {
                var query = "delete from UserNote where EmailId =@EmailId and NoteId=@NoteId";
                using (var connection = _context.CreateConnection())
                {
                    int emailCount = await connection.ExecuteScalarAsync<int>(check_email, new { EmailId = email});

                    if (emailCount == 0)
                    {
                        throw new EmailNotFoundException($"email '{email}' Is Not A Registerd User please Register First and try Again.");
                    }
                    rowsAffected = await connection.ExecuteAsync(query, new { EmailId = email,NoteId=id });
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
        //-----------------------------------------------------------------------------------------------------------------------------------

        //send the note into trash

        public async Task<int> MoveToTrash(string id, string emailid)
        {
            var users = await GetAllNotes(id);
            var check_email = "SELECT COUNT(*) FROM UserNote WHERE EmailId = @EmailId";
            if (!users.Any())
            {
                // If no users are found with the given email, throw custom exception
                throw new IdNotFoundException("note id does not exist.");
           
            }
            else
            {


                var query = "UPDATE UserNote set isTrash = 'true' WHERE NoteId=@NoteId";
                var parameters = new DynamicParameters();
                parameters.Add("@EmailId", emailid, DbType.String);
                parameters.Add("@NoteId", id, DbType.String);

                int rowsAffected = 0;
                using (var connection = _context.CreateConnection())
                {
                    int emailCount = await connection.ExecuteScalarAsync<int>(check_email, new { EmailId = emailid });

                    if (emailCount == 0)
                    {
                        throw new EmailNotFoundException($"email '{emailid}' Is Not A Registerd User please Register First and try Again.");
                    }
                    rowsAffected = await connection.ExecuteAsync(query, parameters);
                    if (rowsAffected > 0)
                    {
                        return rowsAffected;
                    }
                    else
                    {
                        throw new ParameterException("title and description must be required..........");
                    }


                }
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------
    }
}


