using DBRedditClone.Models;
using DBRedditClone.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DBRedditClone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly CommentsService _commentService;
        private readonly ILogger<CommentsController> _logger;

        public CommentsController(CommentsService commentService, ILogger<CommentsController> logger)
        {
            _commentService = commentService;
            _logger = logger;
        }

        [HttpGet("comment-details/{id}")]
        public async Task<IActionResult> GetCommentById(string id)
        {
            if (!Guid.TryParse(id, out Guid commentId))
                return BadRequest("Invalid GUID format.");

            var result = await _commentService.GetCommentById(commentId);
            return result is SuccessDataResult<CommentModel> ?
                Ok(result) : BadRequest(result);
        }

        [HttpGet("all-comments")]
        public async Task<IActionResult> GetAllComments()
        {
            var result = await _commentService.GetAllComments();
            return result is SuccessDataResult<List<CommentModel>> ?
                Ok(result) : BadRequest(result);
        }

        [HttpPost("post-comment")]
        public async Task<IActionResult> PostComment(AddCommmentDto dto)
        {
            var comment = new CommentModel
            {
                PostId = dto.PostId,
                UserId = dto.UserId,
                Content = dto.Content,
                VoteScore = 0
            };
            var result = await _commentService.CreateComment(comment);
            return result is SuccessResult ?
                 Ok(result) : BadRequest(result);
        }

        [HttpDelete("remove-comment/{id}")]
        public async Task<IActionResult> DeleteCommentById(string id)
        {
            if (!Guid.TryParse(id, out Guid commentId))
                return BadRequest("Invalid GUID format.");

            var result = await _commentService.DeleteComment(commentId);
            return result is SuccessResult ?
                Ok(result) : BadRequest(result);
        }

    }
}
