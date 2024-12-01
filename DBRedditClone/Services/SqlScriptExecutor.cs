using Npgsql;

namespace DBRedditClone.Services
{
    public class SqlScriptExecutor
    {
        private readonly string _connectionString;

        public SqlScriptExecutor(string connectionString)
        {
            _connectionString = connectionString;   
        }


        public async Task ExecuteSqlScriptAsync(string scriptPath)
        {
            var script= await File.ReadAllTextAsync(scriptPath);

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new NpgsqlCommand(script, connection))
                {
                    await command.ExecuteNonQueryAsync();   
                } 
            }
        }
    }
}
