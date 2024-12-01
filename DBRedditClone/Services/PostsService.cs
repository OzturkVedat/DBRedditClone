using Npgsql;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using DBRedditClone.Models;

namespace DBRedditClone.Services
{
    public class PostsService
    {
        private readonly string _connectionString;
        private readonly ILogger<PostsService> _logger;

        public PostsService(string connectionString, ILogger<PostsService> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<ResultModel> GetAllPosts()
        {
            try
            {
                var posts = new List<PostModel>();
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new NpgsqlCommand("SELECT * FROM GetAllPosts();", connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var post = new PostModel
                                {
                                    PostId = reader.GetGuid(0).ToString(),
                                    SubredditId = reader.GetGuid(1).ToString(),
                                    UserId = reader.GetGuid(2).ToString(),
                                    Title = reader.GetString(3),
                                    Content = reader.GetString(4),
                                    VoteScore = reader.GetInt32(5)
                                };
                                posts.Add(post);
                            }
                        }
                    }
                }
                return new SuccessDataResult<List<PostModel>>(posts);
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError($"Failed to fetch posts: {ex.Message}");
                return new FailureResult("Failed to fetch posts.");
            }
        }

        public async Task<ResultModel> CreatePost(PostEntity post)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand("SELECT InsertPost(@PostId, @SubredditId, @UserId, @Title, @Content, @VoteScore);", connection))
                    {
                        command.Parameters.AddWithValue("@PostId", post.PostId);
                        command.Parameters.AddWithValue("@SubredditId", post.SubredditId);
                        command.Parameters.AddWithValue("@UserId", post.UserId);
                        command.Parameters.AddWithValue("@Title", post.Title);
                        command.Parameters.AddWithValue("@Content", post.Content);
                        command.Parameters.AddWithValue("@VoteScore", post.VoteScore);

                        await command.ExecuteNonQueryAsync();
                    }
                }
                return new SuccessResult("Post created successfully.");
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError($"Post creation failed: {ex.Message}");
                return new FailureResult("Post creation failed.");
            }
        }

        public async Task<ResultModel> DeletePost(Guid postId)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand("SELECT DeletePost(@PostId);", connection))
                    {
                        command.Parameters.AddWithValue("@PostId", postId);
                        await command.ExecuteNonQueryAsync();
                    }
                }
                return new SuccessResult("Post deleted successfully.");
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError($"Post deletion failed: {ex.Message}");
                return new FailureResult("Post deletion failed.");
            }
        }

        public async Task<ResultModel> InsertPostVote(Guid voteId, Guid userId, int voteValue, Guid postId)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    const string query = "SELECT InsertPostVote(@VoteId, @UserId, @VoteValue, @PostId);";
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@VoteId", voteId);
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@VoteValue", voteValue);
                        command.Parameters.AddWithValue("@PostId", postId);

                        await command.ExecuteNonQueryAsync();
                    }
                }
                return new SuccessResult("Vote inserted successfully.");
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError($"Failed to insert post vote: {ex.Message}");
                return new FailureResult("Failed to insert post vote.");
            }
        }
        public async Task<ResultModel> DeletePostVote(Guid voteId)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    const string query = "SELECT DeletePostVote(@VoteId);";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@VoteId", voteId);

                        await command.ExecuteNonQueryAsync();
                    }
                }
                return new SuccessResult("Vote deleted successfully.");
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError($"Failed to delete post vote: {ex.Message}");
                return new FailureResult("Failed to delete post vote.");
            }
        }


    }
}
