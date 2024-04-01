using Microsoft.AspNetCore.Http;
using BusinessLayer.InterfaceBl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using RepositoryLayer.Service;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace FundooNotes.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBl userdl;
        private readonly IConfiguration configuration;
        private readonly IDistributedCache _cache;
        public readonly ILogger<UserService> logger;
        private readonly ProducerConfig _config;
        public UserController(IUserBl userdl, IConfiguration configuration, IDistributedCache _cache,ILogger<UserService> logger, ProducerConfig config)
        {
            this.userdl = userdl;
            this.configuration = configuration;
            this._cache = _cache;
            this.logger = logger;
            _config = config;
        }

        //----------------------------------------------------------------------------------------------
        /*[HttpPost("Sign Up")]
        public async Task<IActionResult> Insert(User updateDto)
        {
            try
            {
                logger.LogInformation("Insertion done successfully");
                await userdl.Insertion(updateDto.FirstName, updateDto.LastName, updateDto.EmailId, updateDto.Password);
                return Ok(updateDto);
            }
            catch (Exception ex)
            {
                // Log the exception
               
                logger.LogError($"{ex.Message} exception occured");
                return BadRequest("An error occurred while processing your request.");
            }
        }*/

        [HttpPost("Sign Up")]
        public async Task<IActionResult> Insert(string topic,[FromBody] User updateDto)
        {
            try
            {
                string serilizedUser = JsonConvert.SerializeObject(updateDto);
                using (var producer = new ProducerBuilder<Null, string>(_config).Build())
                {
                    await producer.ProduceAsync(topic, new Message<Null, string> { Value = serilizedUser });
                    producer.Flush(TimeSpan.FromSeconds(10));
                    //return Ok(true);
                }
                
                await userdl.Insertion(updateDto.FirstName, updateDto.LastName, updateDto.EmailId, updateDto.Password);
                logger.LogInformation("Registration done successfully");
                return Ok("User registered successfully");
            }
            catch (Exception ex)
            {
                // Log the exception

                logger.LogError($"{ex.Message} exception occured");
                return BadRequest("An error occurred while processing your request.");
            }
        }
        //--------------------------------------------------------------------------------

        [HttpGet("GetUsersList")]
        [Authorize] 
        public async Task<IActionResult> GetUsersList()
        {
            try
            {
                logger.LogInformation("details of users");
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
                
                return BadRequest(ex.Message);
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------

        //Display user details based on email
        [HttpGet("getByEmailUsingRedis")]
        [Authorize]
        public async Task<IActionResult> GetUsersByEmail(string email)
        {
            try
            {
                var cachedLabel = await _cache.GetStringAsync(email);
                if (!string.IsNullOrEmpty(cachedLabel))
                {

                    return Ok(System.Text.Json.JsonSerializer.Deserialize<List<User>>(cachedLabel));

                }
                else
                {
                    var values = await userdl.GetUsersByEmail(email);
                    if (values != null)
                    {
                        //
                        var cacheOptions = new DistributedCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20)

                        };

                        //add to the redis calche memory
                        await _cache.SetStringAsync(email, System.Text.Json.JsonSerializer.Serialize(values), cacheOptions);
                        return Ok(values);
                    }
                    return NotFound("No id found");
                }
            }
            catch (Exception ex)
            {
                
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
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddHours(1);
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Email, email),
        // Add additional claims if needed
    };
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: cred
            );
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
        //

        [HttpGet("GetByToken")]

        public async Task<IActionResult> GetUsersByToken(string token)
        {
            try
            {
                var values = await userdl.GetUsersByToken(token);
                return Ok(values);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
