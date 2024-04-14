using BusinessLayer.InterfaceBl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Entities;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserNoteLabelController : ControllerBase
    {
        private readonly IUserNoteLabelBl labelbl;
        private readonly IConfiguration configuration;
        public UserNoteLabelController(IUserNoteLabelBl labelbl, IConfiguration configuration)
        {
            this.labelbl = labelbl;
            this.configuration = configuration;
        }

        //----------------------------------------------------------------------------------------------
        [HttpPost("AddLabel")]
        public async Task<IActionResult> CreateLabel(UserNoteLabel updateDto3)
        {
            try
            {
                await labelbl.CreateLabel(updateDto3.NoteId, updateDto3.LabelName,updateDto3.Email);
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

        [HttpGet("GetAllLabels")]
        public async Task<IActionResult> GetUserNoteLabels()
        {
            try
            {
                var values = await labelbl.GetUserNoteLabels();
                return Ok(values);
            }
            catch (Exception ex)
            {
                //log error
                return BadRequest(ex.Message);
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------------

        [HttpPut("UpdateLabelNameById")]
        public async Task<IActionResult> UpdateName(string name, string id)
        {
            try
            {
                return Ok(await labelbl.UpdateName(name,id));
                //return Ok("User password updated successfully");
            }
            catch (Exception ex)
            {
                // Log the exception
                return BadRequest(ex.Message);
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------

        [HttpDelete("DeleteLabelNameById")]
        public async Task<IActionResult> DeleteLabel(string name, string id)
        {
            try
            {
                //await userdl.DeleteUserByEmail(email);
                return Ok(await labelbl.DeleteLabel(name,id));

            }
            catch (Exception ex)
            {
                // Log error
                return BadRequest(ex.Message);
            }
        }
    }
}
