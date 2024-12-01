using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DBRedditClone.Models;
using Npgsql;

namespace DBRedditClone.Services
{
    public class SubredditsService
    {
        private readonly string _connectionString;
        private readonly ILogger<SubredditsService> _logger;

        public SubredditsService(string connectionString, ILogger<SubredditsService> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<ResultModel> GetAllSubreddits()
        {
            try
            {
                var subreddits = new List<SubredditModel>();

                await using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                const string query = "SELECT * FROM GetAllSubreddits();";

                await using var command = new NpgsqlCommand(query, connection);
                await using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    subreddits.Add(new SubredditModel
                    {
                        SubredditId = reader.GetGuid(0).ToString(),
                        CreatedById = reader.GetGuid(1).ToString(),
                        SubName = reader.GetString(2),
                        SubDescription = reader.GetString(3),
                        UserCount = reader.GetInt32(4)
                    });
                }
                return new SuccessDataResult<List<SubredditModel>>(subreddits);
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError($"Failed to fetch subreddits: {ex.Message}");
                return new FailureResult("Failed to fetch subreddits.");
            }

        }


        public async Task<ResultModel> InsertSubreddit(SubredditEntity newSub)
        {
            try
            {
                await using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                const string query = "SELECT InsertSubreddit(@SubredditId, @CreatedBy, @SubName, @SubDescription, @UserCount);";

                await using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@SubredditId", newSub.SubredditId);
                command.Parameters.AddWithValue("@CreatedBy", newSub.CreatedById);
                command.Parameters.AddWithValue("@SubName", newSub.SubName);
                command.Parameters.AddWithValue("@SubDescription", newSub.SubDescription ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@UserCount", newSub.UserCount);

                await command.ExecuteNonQueryAsync();
                return new SuccessResult("Subreddit successfully created.");
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError($"Failed to add subreddits: {ex.Message}");
                return new FailureResult("Failed to add subreddit.");
            }
        }

        public async Task<ResultModel> InsertUserToSubreddit(Guid subredditId, Guid userId)
        {
            try
            {
                await using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                const string query = "SELECT AddMember(@SubredditId, @UserId);";

                await using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@SubredditId", subredditId);
                command.Parameters.AddWithValue("@UserId", userId);

                await command.ExecuteNonQueryAsync();
                return new SuccessResult("User successfully added to the subreddit.");
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError($"Failed to add user to subreddit: {ex.Message}");
                return new FailureResult("Failed to add user to subreddit.");
            }
        }

        public async Task<ResultModel> DeleteSubreddit(Guid subredditId)
        {
            try
            {
                await using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                const string query = "SELECT DeleteSubreddit(@SubredditId);";

                await using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("@SubredditId", subredditId);

                await command.ExecuteNonQueryAsync();
                return new SuccessResult("Successfully deleted.");
            }

            catch (NpgsqlException ex)
            {
                _logger.LogError($"Failed to remove subreddits: {ex.Message}");
                return new FailureResult("Failed to remove subreddit.");
            }
        }
    }
}
