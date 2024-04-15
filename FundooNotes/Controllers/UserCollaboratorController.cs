using BusinessLayer.InterfaceBl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Entities;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCollaboratorController : ControllerBase
    {
        private readonly IUserCollaboratorBl collaboratorbl;
        private readonly IConfiguration configuration;
        public UserCollaboratorController(IUserCollaboratorBl collaboratorbl, IConfiguration configuration)
        {
            this.collaboratorbl = collaboratorbl;
            this.configuration = configuration;
        }

        //----------------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<IActionResult> AddCollaborator(UserCollaborator updateDto2)
        {
            try
            {
                await collaboratorbl.AddCollaborator(updateDto2.CollaboratorId, updateDto2.NoteId, updateDto2.CollaboratorEmail);
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
        [HttpGet]
        public async Task<IActionResult> GetAllCollaborators()
        {
            try
            {
                var values = await collaboratorbl.GetAllCollaborators();
                return Ok(values);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------

        [HttpDelete]
        public async Task<IActionResult> DeleteCollaborator(string cid,string nid)
        {
            try
            {
                //await userdl.DeleteUserByEmail(email);
                return Ok(await collaboratorbl.DeleteCollaborator(cid,nid));

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
