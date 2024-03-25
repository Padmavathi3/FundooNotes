using Dapper;
using RepositoryLayer.Context;
using RepositoryLayer.CustomExceptions;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class UserCollaboratorService: IUserCollaborator
    {
        private readonly DapperContext _context;
        public UserCollaboratorService(DapperContext context)
        {
            _context = context;
        }

        //Add collaborator
        public  async Task AddCollaborator(string cid, string nid, string email)
        {
            var query = "insert into Collaborators(CollaboratorId,NoteId,CollaboratorEmail) values(@CollaboratorId,@NoteId,@CollaboratorEmail)";


            var parameters = new DynamicParameters();
            parameters.Add("@CollaboratorId", cid, DbType.String);
            parameters.Add("@NoteId", nid, DbType.String);
            parameters.Add("@CollaboratorEmail", email, DbType.String);

            //check if email exist in user table or not
            var check_email = "select count(*) from Person where EmailId=@EmailId";
            using (var connection = _context.CreateConnection())
            {
                int emailCount = await connection.ExecuteScalarAsync<int>(check_email, new {EmailId = email });

                if (emailCount == 0)
                {
                    throw new EmailNotFoundException($"Collaborator with email '{email}' Is Not A Registerd User please Register First and try Again.");
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

        //logic for Display the all collaborators

        public async Task<IEnumerable<UserCollaborator>> GetAllCollaborators()
        {
            var query = "SELECT * FROM Collaborators";


            using (var connection = _context.CreateConnection())
            {
                var person = await connection.QueryAsync<UserCollaborator>(query);
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


        
        public async Task<int> DeleteCollaborator(string cid, string nid)
        {
            // Check if the collaborator exists in the Collaborators table
            var check_id = "SELECT COUNT(*) FROM Collaborators WHERE CollaboratorId = @CollaboratorId AND NoteId = @NoteId";
            var deleteQuery = "DELETE FROM Collaborators WHERE CollaboratorId = @CollaboratorId AND NoteId = @NoteId";
            int rowsAffected = 0;

            using (var connection = _context.CreateConnection())
            {
                int idCount = await connection.ExecuteScalarAsync<int>(check_id, new { CollaboratorId = cid, NoteId = nid });

                if (idCount == 0)
                {
                    throw new EmailNotFoundException($"Collaborator with ID {cid} is not a registered user. Please register first and try again.");
                }

                try
                {
                    // Execute the delete query
                    rowsAffected = await connection.ExecuteAsync(deleteQuery, new { CollaboratorId = cid, NoteId = nid });

                    if (rowsAffected > 0)
                    {
                        return rowsAffected;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while deleting the collaborator. Please try again later.", ex);
                }
            }

            return rowsAffected;
        }

    }
}
