using BusinessLayer.InterfaceBl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBl userdl;
        private readonly IConfiguration configuration;
        public UserController(IUserBl userdl, IConfiguration configuration)
        {
            this.userdl = userdl;
            this.configuration = configuration;
        }

        //----------------------------------------------------------------------------------------------
        [HttpPost("Sign Up")]
        public async Task<IActionResult> Insert(User updateDto)
        {
            try
            {
                await userdl.Insertion(updateDto.FirstName, updateDto.LastName, updateDto.EmailId, updateDto.Password);
                return Ok(updateDto);
            }
            catch (Exception ex)
            {
                // Log the exception
                //return StatusCode(500, "An error occurred while inserting values");
                return BadRequest(ex.Message);
            }
        }
        //--------------------------------------------------------------------------------

        [HttpGet("Display user Details")]
        public async Task<IActionResult> GetUsersList()
        {
            try
            {
                var values = await userdl.GetUsers();
                return Ok(values);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        //---------------------------------------------------------------------------------------------

        [HttpPut("ResetPassWord/{personEmailUpdate}")]
        public async Task<IActionResult> ResetPasswordByEmail(string personEmailUpdate, [FromBody] User updateDto)
        {
            try
            {
                return Ok(await userdl.ResetPasswordByEmail(personEmailUpdate, updateDto.Password));
                //return Ok("User password updated successfully");
            }
            catch (Exception ex)
            {
                // Log the exception
                return BadRequest(ex.Message);
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------

        //Display user details based on email
        [HttpGet("getByEmail")]
        public async Task<IActionResult> GetUsersByEmail(string email)
        {
            try
            {
                var values = await userdl.GetUsersByEmail(email);
                return Ok(values);
            }
            catch (Exception ex)
            {
                //log error
                return BadRequest(ex.Message);  
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------

        [HttpDelete("delete user/{email}")]
        public async Task<IActionResult> DeleteUserByEmail(string email)
        {
            try
            {
                //await userdl.DeleteUserByEmail(email);
                return Ok(await userdl.DeleteUserByEmail(email));
                
            }
            catch (Exception ex)
            {
                // Log error
                return BadRequest(ex.Message);
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------
        [HttpGet("Login/{email}/{password}")]
        //[UserExceptionHandlerFilter]
        public async Task<IActionResult> Login(string email, string password)
        {

            var values = await userdl.Login(email, password);

            String token = TokenGeneration(email);
            return Ok(token);


        }
        //----------------------------------------------------------------------------------------

        private string TokenGeneration(string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:key"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claim = new[]
            {
                new Claim(ClaimTypes.Email,email),
                //here we can add additional clims like permissiones
            };
            var token = new JwtSecurityToken(issuer: configuration["jwt:issuer"],
                                                audience: configuration["jwt:audience"],
                                                claims: claim,
                                                expires: DateTime.UtcNow.AddMinutes(double.Parse(configuration["jwt:minutes"])),
                                                signingCredentials: cred);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        //-----------------------------------------------------------------------------------------------------------

        [HttpPut("forgot password/{Email}")]
        //[UserExceptionHandlerFilter]
        public async Task<IActionResult> ChangePasswordRequest(String Email)
        {
            return Ok(await userdl.ChangePasswordRequest(Email));
        }



        [HttpPut("otp/{otp}/{password}")]
        //[UserExceptionHandlerFilter]
        public async Task<IActionResult> ChangePassword(String otp, String password)
        {
            return Ok(await userdl.ChangePassword(otp, password));
        }
    }

}
