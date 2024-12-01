using DBRedditClone.Models;
using Npgsql;
using System.Text;
using System.Security.Cryptography;

namespace DBRedditClone.Services
{
    public class UsersService
    {
        private readonly string _connectionString;
        private readonly ILogger<UsersService> _logger;
        public UsersService(string connectionString, ILogger<UsersService> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<ResultModel> GetUserById(Guid userId)
        {
            try
            {
                var user = new UserModel();
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand("SELECT * FROM GetUserById(@UserId);", connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                user = new UserModel
                                {
                                    UserId = reader.GetGuid(0).ToString(),
                                    UserName = reader.GetString(1),
                                    Email = reader.GetString(2),
                                    PasswordHash = reader.GetString(3),
                                    Karma = reader.GetInt32(4)
                                };
                            }
                        }
                    }
                }
                return new SuccessDataResult<UserModel>(user);
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError($"User fetch failed: {ex.Message}");
                return new FailureResult("Fetch request failed.");
            }
        }

        public async Task<ResultModel> GetAllUsers()
        {
            try
            {
                var users = new List<UserPublicDto>();
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var command = new NpgsqlCommand("SELECT * FROM GetAllUsers();", connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var user = new UserPublicDto
                                {
                                    UserId = reader.GetGuid(0).ToString(),
                                    UserName = reader.GetString(1),
                                    Karma = reader.GetInt32(2)
                                };
                                users.Add(user);
                            }
                        }
                    }
                }
                return new SuccessDataResult<List<UserPublicDto>>(users);
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError($"User fetch failed: {ex.Message}");
                return new FailureResult("Fetch request failed.");
            }
        }
        private async Task<ResultModel> ExecuteDbCommand(Func<NpgsqlConnection, Task> dbOperation)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    await dbOperation(connection);
                }
                return new SuccessResult("Database operation successful.");
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError($"Database operation failed: {ex.Message}");
                return new FailureResult("Database operation failed.");
            }
        }

        public async Task<ResultModel> CreateUser(UserRegisterDto newUser)
        {
            if (newUser == null)
                return new FailureResult("User DTO cannot be null.");

            var hashedPw = ComputeHash(newUser.Password);
            var userId = Guid.NewGuid();

            return await ExecuteDbCommand(async (connection) =>
            {
                using (var command = new NpgsqlCommand(
                    "SELECT InsertUser(@UserId, @UserName, @Email, @PasswordHash, @Karma);", connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@UserName", newUser.UserName);
                    command.Parameters.AddWithValue("@Email", newUser.Email);
                    command.Parameters.AddWithValue("@PasswordHash", hashedPw);
                    command.Parameters.AddWithValue("@Karma", 0);

                    await command.ExecuteNonQueryAsync();
                }
            });
        }
        private string ComputeHash(string content)          // SHA256 hashing
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(content));
                StringBuilder builder = new StringBuilder();

                foreach (byte b in bytes)       // convert byte to hexadecimal
                {
                    builder.Append(b.ToString("x2"));       // accumulate
                }
                return builder.ToString();
            }
        }

        public async Task<ResultModel> UpdateUser(UserUpdateDto dto)
        {
            var newHashedPw = ComputeHash(dto.NewPassword);

            if (!Guid.TryParse(dto.UserId, out Guid userGuid))
                return new FailureResult("Invalid GUID format.");

            return await ExecuteDbCommand(async (connection) =>
            {
                using (var cmd = new NpgsqlCommand(
                    "SELECT UpdateUser(@UserId, @UserName, @PasswordHash);", connection))
                {
                    cmd.Parameters.AddWithValue("@UserId", userGuid);
                    cmd.Parameters.AddWithValue("@UserName", dto.UserName);
                    cmd.Parameters.AddWithValue("@PasswordHash", newHashedPw);

                    await cmd.ExecuteNonQueryAsync();
                }
            });
        }

        public async Task<ResultModel> DeleteUser(Guid userId)
        {
            return await ExecuteDbCommand(async (connection) =>
            {
                using (var cmd = new NpgsqlCommand("SELECT DeleteUser(@UserId)",connection))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    await cmd.ExecuteNonQueryAsync();
                }
            });
        }

    }
}
