using DBRedditClone.Models;
using DBRedditClone.Services;
using Microsoft.AspNetCore.Mvc;

namespace DBRedditClone.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly PostgresService _postgresService;      // a simple service with user table CRUDs

        public UsersController(PostgresService postgresService)
        {
            _postgresService = postgresService;
        }

        [HttpGet("user-details/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            if (!Guid.TryParse(id, out Guid userId))
                return BadRequest("Invalid GUID format.");
            
            var result = await _postgresService.GetUserById(userId);
            if (result is SuccessDataResult<UserModel> successResult)         
                return Ok(successResult.Data);        
            return BadRequest(result.Message);
        }

        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _postgresService.GetAllUsers();
            if (result is SuccessDataResult<List<UserPublicDto>> successResult)
                return Ok(successResult.Data);      
            return BadRequest(result.Message);
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> CreateUser([FromBody] UserRegisterDto newUser)
        {
            var result = await _postgresService.CreateUser(newUser);
            if (result is SuccessResult)
                return Ok(result.Message);
            
            return BadRequest(result.Message);
        }

        [HttpPatch("update-user")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto dto)
        {
            var result = await _postgresService.UpdateUser(dto);
            if (result is SuccessResult)          
                return Ok(result.Message);     
            return BadRequest(result.Message);
        }

        [HttpDelete("delete-user/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (!Guid.TryParse(id, out Guid userId))
                return BadRequest("Invalid GUID format.");

            var result = await _postgresService.DeleteUser(userId);
            if (result is SuccessResult)
                return Ok(result.Message);
            
            return BadRequest(result.Message);
        }
    }
}
