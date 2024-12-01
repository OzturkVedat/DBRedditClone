using DBRedditClone.Models;
using DBRedditClone.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DBRedditClone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {

        private readonly PostsService _postService;
        private readonly ILogger<PostsController> _logger;

        public PostsController(PostsService postService, ILogger<PostsController> logger)
        {
            _postService = postService;
            _logger = logger;
        }

        [HttpGet("all-posts")]
        public async Task<IActionResult> GetAllPosts()
        {
            var result = await _postService.GetAllPosts();
            return result is SuccessDataResult<List<PostModel>> ?
                Ok(result) : BadRequest(result);
        }

        [HttpPost("new-post")]
        public async Task<IActionResult> AddNewPost(NewPostDto dto)
        {
            if (!Guid.TryParse(dto.SubredditId, out Guid subGuid))
                return BadRequest("Invalid GUID format.");
            
            if (!Guid.TryParse(dto.UserId, out Guid userGuid))
                return BadRequest("Invalid GUID format.");

            var post = new PostEntity
            {
                PostId=Guid.NewGuid(),
                SubredditId=subGuid,
                UserId=userGuid,
                Title=dto.Title,
                Content=dto.Content,
                VoteScore=0,
            };
            var result = await _postService.CreatePost(post);
            return result is SuccessResult ?
                 Ok(result) : BadRequest(result);
        }


        [HttpPost("post-upvote")]
        public async Task<IActionResult> AddNewPostUpvote(PostVoteDto dto)
        {
            if (!Guid.TryParse(dto.PostId, out Guid postId))
                return BadRequest("Invalid GUID format.");

            if (!Guid.TryParse(dto.UserId, out Guid userId))
                return BadRequest("Invalid GUID format.");

            var result = await _postService.InsertPostVote(Guid.NewGuid(), userId, 1, postId);
            return result is SuccessResult ? Ok(result) : BadRequest(result);
        }

        [HttpPost("post-downvote")]
        public async Task<IActionResult> AddNewPostDownvote(PostVoteDto dto)
        {
            if (!Guid.TryParse(dto.PostId, out Guid postId))
                return BadRequest("Invalid GUID format.");

            if (!Guid.TryParse(dto.UserId, out Guid userId))
                return BadRequest("Invalid GUID format.");

            var result = await _postService.InsertPostVote(Guid.NewGuid(), userId, -1, postId);
            return result is SuccessResult ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("remove-post-vote/{voteId}")]
        public async Task<IActionResult> DeletePostVoteById(string voteId)
        {
            if (!Guid.TryParse(voteId, out Guid voteGuid))
                return BadRequest("Invalid GUID format.");

            var result = await _postService.DeletePostVote(voteGuid);
            return result is SuccessResult ?
                Ok(result) : BadRequest(result);
        }

        [HttpDelete("remove-post/{id}")]
        public async Task<IActionResult> DeletePostById(string id)
        {
            if (!Guid.TryParse(id, out Guid postId))
                return BadRequest("Invalid GUID format.");

            var result = await _postService.DeletePost(postId);
            return result is SuccessResult ?
                Ok(result) : BadRequest(result);
        }


    }
}
