using BusinessLayer.InterfaceBl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entities;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserNoteLabelController : ControllerBase
    {
        private readonly IUserNoteLabelBl userdl3;
        private readonly IConfiguration configuration;
        public UserNoteLabelController(IUserNoteLabelBl userdl3, IConfiguration configuration)
        {
            this.userdl3 = userdl3;
            this.configuration = configuration;
        }

        //----------------------------------------------------------------------------------------------
        [HttpPost("Create Label")]
        public async Task<IActionResult> CreateLabel(UserNoteLabel updateDto3)
        {
            try
            {
                await userdl3.CreateLabel(updateDto3.NoteId, updateDto3.LabelName,updateDto3.Email);
                return Ok(updateDto3);
            }
            catch (Exception ex)
            {
                // Log the exception
                //return StatusCode(500, "An error occurred while inserting values");
                return BadRequest(ex.Message);
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------

        [HttpGet("label details")]
        public async Task<IActionResult> GetUserNoteLabels()
        {
            try
            {
                var values = await userdl3.GetUserNoteLabels();
                return Ok(values);
            }
            catch (Exception ex)
            {
                //log error
                return BadRequest(ex.Message);
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------------

        [HttpPut("update Label")]
        public async Task<IActionResult> UpdateName(string name, string id)
        {
            try
            {
                return Ok(await userdl3.UpdateName(name,id));
                //return Ok("User password updated successfully");
            }
            catch (Exception ex)
            {
                // Log the exception
                return BadRequest(ex.Message);
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------

        [HttpDelete("delete label")]
        public async Task<IActionResult> DeleteLabel(string name, string id)
        {
            try
            {
                //await userdl.DeleteUserByEmail(email);
                return Ok(await userdl3.DeleteLabel(name,id));

            }
            catch (Exception ex)
            {
                // Log error
                return BadRequest(ex.Message);
            }
        }
    }
}
