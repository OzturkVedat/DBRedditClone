using DBRedditClone.Models;
using Npgsql;
using System.Text;
using System.Security.Cryptography;

namespace DBRedditClone.Services
{
    public class PostgresService
    {
        private readonly string _connectionString;
        private readonly ILogger<PostgresService> _logger;
        public PostgresService(string connectionString, ILogger<PostgresService> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        private async Task<bool> ExecuteDbCommand(Func<NpgsqlConnection, Task> dbOperation)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    await dbOperation(connection);
                }
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Database operation failed: {ex.Message}");
                return false;
            }
        }

        public async Task<List<UserModel>> GetAllUsers()
        {
            var users= new List<UserModel>();
            using(var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand("SELECT * FROM GetAllUsers();", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var user = new UserModel
                            {
                                UserID = reader.GetGuid(0),
                                UserName= reader.GetString(1),
                                Email=reader.GetString(2),
                                Karma= reader.GetInt32(3)
                            };
                            users.Add(user);
                        }
                    }
                }
            }
            return users;
        }

        public async Task<bool> CreateUser(UserRegisterDto newUser)
        {
            if (newUser == null)
            {
                _logger.LogError("User DTO cannot be null.");
                return false;
            }
            var hashedPw= ComputeHash(newUser.Password);
            var userId= Guid.NewGuid();

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
    }
}
