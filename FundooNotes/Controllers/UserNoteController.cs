using BusinessLayer.InterfaceBl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Entities;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserNoteController : ControllerBase
    {
        private readonly IUserNoteBl notebl;
        private readonly IConfiguration configuration;
        public UserNoteController(IUserNoteBl notebl, IConfiguration configuration)
        {
            this.notebl = notebl;
            this.configuration = configuration;
        }

        //----------------------------------------------------------------------------------------------

        [HttpPost]
        public async Task<IActionResult> CreateNote(UserNote updateDto1)
        {
            try
            {
                await notebl.CreateNote(updateDto1.NoteId, updateDto1.Title, updateDto1.Description, updateDto1.reminder, updateDto1.isArchive, updateDto1.isPinned, updateDto1.isTrash, updateDto1.EmailId);
                return Ok(updateDto1);
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex.Message);
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------

        //Display user note details details based on id
        [HttpGet]
        public async Task<IActionResult> GetAllNotes(string id)
        {
            try
            {
                var values = await notebl.GetAllNotes(id);
                return Ok(values);
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex.Message);
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------
        [HttpPut]
        public async Task<IActionResult> Update(string id,string emailid,UserNote updateDto1)
        {
            try
            {
                return Ok(await notebl.Update(id,emailid, updateDto1.Title,updateDto1.Description));
                //return Ok("User password updated successfully");
            }
            catch (Exception ex)
            {
                // Log the exception
                return BadRequest(ex.Message);
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------
       
        [HttpDelete]
        public async Task<IActionResult> DeleteNote(string id,string email)
        {
            try
            {
                //await userdl.DeleteUserByEmail(email);
                return Ok(await notebl.DeleteNote(id,email));

            }
            catch (Exception ex)
            {
                // Log error
                return BadRequest(ex.Message);
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------
    }
}
