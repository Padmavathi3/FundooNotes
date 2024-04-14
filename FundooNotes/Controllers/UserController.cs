using Microsoft.AspNetCore.Http;
using BusinessLayer.InterfaceBl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using RepositoryLayer.Service;
using Confluent.Kafka;
using Newtonsoft.Json;
using ModelLayer.Entities;
using Microsoft.AspNetCore.Cors;
using ModelLayer;
using NuGet.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json.Linq;
using RepositoryLayer.CustomExceptions;
using RepositoryLayer.Interface;

namespace FundooNotes.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class UserController : ControllerBase
    {
        private readonly IUserBl userdl;
        private readonly IConfiguration configuration;
        private readonly IDistributedCache _cache;
        public readonly ILogger<UserService> logger;
        private readonly ProducerConfig _config;
       
        public UserController(IUserBl userdl, IConfiguration configuration, IDistributedCache _cache, ILogger<UserService> logger, ProducerConfig config)
        {
            this.userdl = userdl;
            this.configuration = configuration;
            this._cache = _cache;
            this.logger = logger;
            _config = config;
           
        }

        //---------------------------------------------------------------------------------------------------------

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] User updateDto)
        {
            try
            {
                // Inserting user details into the database
                int rowsAffected = await userdl.Insertion(updateDto.FirstName, updateDto.LastName, updateDto.EmailId, updateDto.Password);

                if (rowsAffected > 0)
                {
                    return Ok(new ResponseModel<object>
                    {
                        Success = true,
                        Message = "User registered successfully",
                        Data = updateDto
                    });
                }
                else
                {
                    return BadRequest(new ResponseModel<object>
                    {
                        Success = false,
                        Message = "Failed to register user",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------
        [HttpGet("GetUsersList")]
        public async Task<IActionResult> GetUsersList()
        {
            try
            {
                logger.LogInformation("Fetching details of users");
                var users = await userdl.GetUsers();

                return Ok(new ResponseModel<IEnumerable<User>>
                {
                    Success = true,
                    Message = "User Details",
                    Data = users
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = "User must be authorized",
                    Data = null
                });
            }
        }

        //-------------------------------------------------------------------------------------------------------------------------------------
        
        [HttpGet("getByEmailUsingRedis")]
        [Authorize]
        public async Task<IActionResult> GetUsersByEmail(string email)
        {
            try
            {
                var cachedLabel = await _cache.GetStringAsync(email);  
                if (!string.IsNullOrEmpty(cachedLabel))
                {

                    var result=System.Text.Json.JsonSerializer.Deserialize<List<User>>(cachedLabel);
                    return Ok(new ResponseModel<List<User>>
                    {
                        Success = true,
                        Message = "Users Detials from cache",
                        Data = result
                    });

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
                        return Ok(new ResponseModel<List<User>>
                        {
                            Success = true,
                            Message = "User details retrieved.",
                            Data = (List<User>)values
                        });
                    }
                    return NotFound("No email found");
                }
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModel<object>
                {

                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------

        [HttpDelete("DeleteUserByEmail")]
        public async Task<IActionResult> DeleteUserByEmail(string email)
        {
            try
            {
                await userdl.DeleteUserByEmail(email);
                return Ok(new ResponseModel<object>
                {
                    Success = true,
                    Message = "Users deleted successfully",
                    Data = null
                });


            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------

        [HttpGet("Login")]
       
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                var values = await userdl.Login(email, password);

                string token = TokenGeneration(email);
                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "login successfully",
                    Data = token
                });
            }
            catch (Exception ex) 
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = "login not donesuccessfully"
                });
            }

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

        [HttpPut("ForgotPassword")]
       
        public async Task<IActionResult> ChangePasswordRequest(string Email)
        {
            try
            {
                await userdl.ChangePasswordRequest(Email);
                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "otp sent to mail successfully"
                });
            }
            catch (Exception ex) 
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }



        [HttpPut("ChangePassword")]
        
        public async Task<IActionResult> ChangePassword(string otp, string password)
        {
            try
            {
                await userdl.ChangePassword(otp, password);
                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "password changed successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel<object>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
        
    }

}