using BusinessLayer.InterfaceBl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entities;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserNoteController : ControllerBase
    {
        private readonly IUserNoteBl userdl1;
        private readonly IConfiguration configuration;
        public UserNoteController(IUserNoteBl userdl1, IConfiguration configuration)
        {
            this.userdl1 = userdl1;
            this.configuration = configuration;
        }

        //----------------------------------------------------------------------------------------------

        [HttpPost("Create Note")]
        public async Task<IActionResult> CreateNote(UserNote updateDto1)
        {
            try
            {
                await userdl1.CreateNote(updateDto1.NoteId, updateDto1.Title, updateDto1.Description, updateDto1.reminder, updateDto1.isArchive, updateDto1.isPinned, updateDto1.isTrash, updateDto1.EmailId);
                return Ok(updateDto1);
            }
            catch (Exception ex)
            {
                // Log the exception
                //return StatusCode(500, "An error occurred while inserting values");
                return BadRequest(ex.Message);
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------

        //Display user note details details based on id
        [HttpGet("getById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetAllNotes(string id)
        {
            try
            {
                var values = await userdl1.GetAllNotes(id);
                return Ok(values);
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex.Message);
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------
        [HttpPut("updateNote")]
        public async Task<IActionResult> Update(string id,string emailid, [FromBody] UserNote updateDto1)
        {
            try
            {
                return Ok(await userdl1.Update(id,emailid, updateDto1.Title,updateDto1.Description));
                //return Ok("User password updated successfully");
            }
            catch (Exception ex)
            {
                // Log the exception
                return BadRequest(ex.Message);
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------
       
        [HttpDelete("delete userNote/{id}/{email}")]
        public async Task<IActionResult> DeleteNote(string id,string email)
        {
            try
            {
                //await userdl.DeleteUserByEmail(email);
                return Ok(await userdl1.DeleteNote(id,email));

            }
            catch (Exception ex)
            {
                // Log error
                return BadRequest(ex.Message);
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------

       /* [HttpPut("MoveToTrash")]
        public async Task<IActionResult> MoveToTrash(string id, string emailid)
        {
            try
            {
                return Ok(await userdl1.MoveToTrash(id, emailid));
                
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex.Message);
            }
        }*/
    }
}
