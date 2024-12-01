using DBRedditClone.Models;
using Npgsql;

namespace DBRedditClone.Services
{
    public class CommentsService
    {
        private readonly string _connectionString;
        private readonly ILogger<CommentsService> _logger;

        public CommentsService(string connectionString, ILogger<CommentsService> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<ResultModel> GetCommentById(Guid postId)
        {
            try
            {
                var comment = new CommentModel();
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new NpgsqlCommand("SELECT * FROM GetCommentById(@PostId);", connection))
                    {
                        command.Parameters.AddWithValue("@PostId", postId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                comment = new CommentModel
                                {
                                    CommentId = reader.GetGuid(0).ToString(),
                                    PostId = reader.GetGuid(1).ToString(),
                                    UserId = reader.GetGuid(2).ToString(),
                                    Content = reader.GetString(3),
                                    VoteScore = reader.GetInt32(4)
                                };
                            }
                        }
                    }
                }
                return new SuccessDataResult<CommentModel>(comment);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to fetch comment: {ex.Message}");
                return new FailureResult("Failed to fetch comment.");
            }
        }
        public async Task<ResultModel> GetAllComments()
        {
            try
            {
                var comments = new List<CommentModel>();
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new NpgsqlCommand("SELECT * FROM GetAllComments();", connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var comment = new CommentModel
                                {
                                    CommentId = reader.GetGuid(0).ToString(),
                                    PostId = reader.GetGuid(1).ToString(),
                                    UserId = reader.GetGuid(2).ToString(),
                                    Content = reader.GetString(3),
                                    VoteScore = reader.GetInt32(4)
                                };
                                comments.Add(comment);
                            }
                        }
                    }
                }
                return new SuccessDataResult<List<CommentModel>>(comments);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to fetch comments: {ex.Message}");
                return new FailureResult("Failed to fetch comments.");
            }
        }


        public async Task<ResultModel> CreateComment(CommentModel comment)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new NpgsqlCommand(
                        "SELECT InsertComment(@CommentId, @PostId, @UserId, @Content, @VoteScore);", connection))
                    {
                        command.Parameters.AddWithValue("@CommentId", Guid.NewGuid());
                        command.Parameters.AddWithValue("@PostId", comment.PostId);
                        command.Parameters.AddWithValue("@UserId", comment.UserId);
                        command.Parameters.AddWithValue("@Content", comment.Content);
                        command.Parameters.AddWithValue("@VoteScore", comment.VoteScore);

                        await command.ExecuteNonQueryAsync();
                    }
                }
                return new SuccessResult("Comment created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Comment creation failed: {ex.Message}");
                return new FailureResult("Comment creation failed.");
            }
        }
        public async Task<ResultModel> DeleteComment(Guid commentId)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new NpgsqlCommand("SELECT DeleteComment(@CommentId);", connection))
                    {
                        command.Parameters.AddWithValue("@CommentId", commentId);

                        await command.ExecuteNonQueryAsync();
                    }
                }
                return new SuccessResult("Comment deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Comment deletion failed: {ex.Message}");
                return new FailureResult("Comment deletion failed.");
            }
        }
        public async Task<ResultModel> AddVoteToComment(CommentVote vote)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand("SELECT InsertCommentVote(@VoteId, @UserId, @CommentId, @VoteValue);", connection))
                    {
                        command.Parameters.AddWithValue("@VoteId", vote.VoteId);
                        command.Parameters.AddWithValue("@UserId", vote.UserId);
                        command.Parameters.AddWithValue("@CommentId", vote.CommentId);
                        command.Parameters.AddWithValue("@VoteValue", vote.VoteValue);

                        await command.ExecuteNonQueryAsync();
                    }
                }

                return new SuccessResult("Vote added successfully, trigger will update the VoteScore.");
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError($"Vote insertion failed: {ex.Message}");
                return new FailureResult("Failed to add vote.");
            }
        }

        public async Task<ResultModel> RemoveVoteFromComment(Guid voteId)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new NpgsqlCommand(
                        "SELECT DeleteCommentVote(@VoteId);", connection))
                    {
                        command.Parameters.AddWithValue("@VoteId", voteId);

                        await command.ExecuteNonQueryAsync();
                    }
                }
                return new SuccessResult("Comment vote deleted successfully.");
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError($"Error deleting comment vote: {ex.Message}");
                return new FailureResult("Failed to delete comment vote.");
            }
        }

    }
}
