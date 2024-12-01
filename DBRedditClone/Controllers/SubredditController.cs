using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using DBRedditClone.Services;
using DBRedditClone.Models;

namespace DBRedditClone.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubredditController : ControllerBase
    {
        private readonly SubredditsService _subredditsService;

        public SubredditController(SubredditsService subredditsService)
        {
            _subredditsService = subredditsService;
        }

        [HttpGet("all-subreddits")]
        public async Task<IActionResult> GetAllSubreddits()
        {
            var result = await _subredditsService.GetAllSubreddits();
            if (result is SuccessDataResult<List<SubredditModel>> subs)
                return Ok(subs.Data);
            return BadRequest(result);
        }

        [HttpPost("new-subreddit")]
        public async Task<IActionResult> InsertSubreddit([FromBody] NewSubredditDto newDto)
        {
            if (!Guid.TryParse(newDto.CreatedById, out Guid userId))
                return BadRequest("Invalid GUID format.");

            var newSub = new SubredditEntity
            {
                SubredditId = Guid.NewGuid(),
                CreatedById = userId,
                SubName = newDto.SubName,
                SubDescription = newDto.SubDescription,
                UserCount = 0       
            };
            var result = await _subredditsService.InsertSubreddit(newSub);
            if (result.IsSuccess)
            {
                var memberResult = await _subredditsService.InsertUserToSubreddit(newSub.SubredditId, userId);  // add the founder as a member
                return memberResult is SuccessResult ? Ok(result) : BadRequest(memberResult); 
            }
            return BadRequest(result);
        }

        [HttpPost("add-member-to-sub")]
        public async Task<IActionResult> InsertMemberToSubreddit([FromBody] NewMemberDto dto)
        {
            if (!Guid.TryParse(dto.SubredditId, out Guid subId))
                return BadRequest("Invalid GUID format.");

            if (!Guid.TryParse(dto.UserId, out Guid userId))
                return BadRequest("Invalid GUID format.");

            var result = await _subredditsService.InsertUserToSubreddit(subId, userId);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result.Message);
        }


        [HttpDelete("remove-subreddit/{id}")]
        public async Task<IActionResult> DeleteSubreddit(string id)
        {
            if (!Guid.TryParse(id, out Guid subGuid))
                return BadRequest("Invalid GUID format.");

            var result = await _subredditsService.DeleteSubreddit(subGuid);
            if (result.IsSuccess)
                return Ok(result.Message);
            return BadRequest(result.Message);
        }
    }
}
