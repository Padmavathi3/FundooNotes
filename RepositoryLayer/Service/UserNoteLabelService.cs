using Dapper;
using RepositoryLayer.Context;
using RepositoryLayer.CustomExceptions;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class UserNoteLabelService:IUserNoteLabel
    {
        private readonly DapperContext _context;
        public UserNoteLabelService(DapperContext context)
        {
            _context = context;
        }

        //Add Label
        public async Task CreateLabel(string id, string name, string email)
        {
            var query = "insert into Label(NoteId,LabelName,Email) values(@NoteId,@LabelName,@Email)";


            var parameters = new DynamicParameters();
            parameters.Add("@NoteId", id, DbType.String);
            parameters.Add("@LabelName",name, DbType.String);
            parameters.Add("@Email", email, DbType.String);

            //check if email exist in user table or not
            var check_email = "select count(*) from UserNote where EmailId=@EmailId and NoteId=@NoteId";
            using (var connection = _context.CreateConnection())
            {
                int emailCount = await connection.ExecuteScalarAsync<int>(check_email, new { EmailId = email,NoteId=id });

                if (emailCount == 0)
                {
                    throw new EmailNotFoundException($" email '{email}' Is Not Registerd with this noteid");
                }
                try
                {

                    await connection.ExecuteAsync(query, parameters);
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while adding collaborator. Please try again later.", ex);
                }
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------------

        //display label names
        public async Task<IEnumerable<UserNoteLabel>> GetUserNoteLabels()
        {
            var query = "SELECT * FROM Label";


            using (var connection = _context.CreateConnection())
            {
                var person = await connection.QueryAsync<UserNoteLabel>(query);
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
        //---------------------------------------------------------------------------------------------------------------------------------------

        //update the label name
        public async Task<int> UpdateName(string name, string id)
        {
            var check_label = "SELECT COUNT(*) FROM Label WHERE NoteId=@NoteId";
            var query = "UPDATE Label SET LabelName = @LabelName WHERE NoteId = @NoteId";
            var parameters = new DynamicParameters();
            parameters.Add("@NoteId", id, DbType.String);
            parameters.Add("@LabelName", name, DbType.String);
            int rowsAffected = 0;
            using (var connection = _context.CreateConnection())
            {
                int labelCount = await connection.ExecuteScalarAsync<int>(check_label, new {NoteId = id });

                if (labelCount == 0)
                {
                    throw new IdNotFoundException($"id is not present with this noteid");
                }
                try
                {
                    rowsAffected = await connection.ExecuteAsync(query, parameters);
                    if (rowsAffected > 0)
                    {
                        return rowsAffected;
                    }
                    else
                    {
                        throw new ParameterException("updated field  value must be required..........");
                    }
                }
                catch (Exception ex) 
                {
                    throw new Exception("An error occurred while deleting the collaborator. Please try again later.", ex);
                }


            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------

        //delete label

        public async Task<int> DeleteLabel(string name,string id)
        {
            var check_label = "SELECT COUNT(*) FROM Label WHERE LabelName=@LabelName and NoteId=@NoteId";
            var query = "delete from Label where LabelName=@LabelName and NoteId=@NoteId";
            var parameters = new DynamicParameters();
            parameters.Add("@NoteId", id, DbType.String);
            parameters.Add("@LabelName", @name, DbType.String);
            int rowsAffected = 0;
            using (var connection = _context.CreateConnection())
            {
                int labelCount = await connection.ExecuteScalarAsync<int>(check_label, new { LabelName = name, NoteId = id });

                if (labelCount == 0)
                {
                    throw new LabelNotFoundException($"Label is not present with this noteid");
                }
                try
                {
                    rowsAffected = await connection.ExecuteAsync(query, parameters);
                    if (rowsAffected > 0)
                    {
                        return rowsAffected;
                    }
                   
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while deleting the collaborator. Please try again later.", ex);
                }

                return rowsAffected;
            }
        }

    }
}
