using BusinessLayer.InterfaceBl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entities;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCollaboratorController : ControllerBase
    {
        private readonly IUserCollaboratorBl userdl2;
        private readonly IConfiguration configuration;
        public UserCollaboratorController(IUserCollaboratorBl userdl2, IConfiguration configuration)
        {
            this.userdl2 = userdl2;
            this.configuration = configuration;
        }

        //----------------------------------------------------------------------------------------------

        [HttpPost("Create collaborator")]
        public async Task<IActionResult> AddCollaborator(UserCollaborator updateDto2)
        {
            try
            {
                await userdl2.AddCollaborator(updateDto2.CollaboratorId, updateDto2.NoteId, updateDto2.CollaboratorEmail);
                return Ok(updateDto2);
            }
            catch (Exception ex)
            {
                // Log the exception
                //return StatusCode(500, "An error occurred while inserting values");
                return BadRequest(ex.Message);
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------
        [HttpGet("Display user collaborators  Details")]
        public async Task<IActionResult> GetAllCollaborators()
        {
            try
            {
                var values = await userdl2.GetAllCollaborators();
                return Ok(values);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------

        [HttpDelete("delete collaborator")]
        public async Task<IActionResult> DeleteCollaborator(string cid,string nid)
        {
            try
            {
                //await userdl.DeleteUserByEmail(email);
                return Ok(await userdl2.DeleteCollaborator(cid,nid));

            }
            catch (Exception ex)
            {
                // Log error
                return BadRequest(ex.Message);
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------
    }
}
