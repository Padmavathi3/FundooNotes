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
       
        public UserNoteLabelController(IUserNoteLabelBl labelbl)
        {
            this.labelbl = labelbl;
        }

        //----------------------------------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> CreateLabel(UserNoteLabel updateDto3)
        {
            try
            {
                await labelbl.CreateLabel(updateDto3.NoteId, updateDto3.LabelName,updateDto3.Email);
                return Ok(updateDto3);
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex.Message);
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------

        [HttpGet]
        public async Task<IActionResult> GetUserNoteLabels()
        {
            try
            {
                var values = await labelbl.GetUserNoteLabels();
                return Ok(values);
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex.Message);
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------------

        [HttpPut]
        public async Task<IActionResult> UpdateName(string name, string id)
        {
            try
            {
                return Ok(await labelbl.UpdateName(name,id));
                
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex.Message);
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------

        [HttpDelete]
        public async Task<IActionResult> DeleteLabel(string name, string id)
        {
            try
            {
                //await userdl.DeleteUserByEmail(email);
                return Ok(await labelbl.DeleteLabel(name,id));

            }
            catch (Exception ex)
            {
                
                return BadRequest(ex.Message);
            }
        }
    }
}
