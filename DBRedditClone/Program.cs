using DBRedditClone.Services;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddSingleton<SqlScriptExecutor>(provider =>
new SqlScriptExecutor(connectionString));

builder.Services.AddSingleton<UsersService>(sp =>
    new UsersService(connectionString, sp.GetRequiredService<ILogger<UsersService>>()));

builder.Services.AddSingleton<SubredditsService>(sp =>
    new SubredditsService(connectionString, sp.GetRequiredService<ILogger<SubredditsService>>()));

builder.Services.AddSingleton<PostsService>(sp =>
    new PostsService(connectionString, sp.GetRequiredService<ILogger<PostsService>>()));

builder.Services.AddSingleton<CommentsService>(sp =>
    new CommentsService(connectionString, sp.GetRequiredService<ILogger<CommentsService>>()));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var baseScriptPath = Path.Combine(Directory.GetCurrentDirectory(), "SqlScripts");

using (var scope = app.Services.CreateScope())
{
    var sqlScriptExecutor = scope.ServiceProvider.GetRequiredService<SqlScriptExecutor>();

    try
    {
        var schemaScriptPath = Path.Combine(baseScriptPath, "CreateSchema.sql");
        await ExecuteScriptWithLogging(sqlScriptExecutor, schemaScriptPath);
        await ExecuteScriptsInDirectory(sqlScriptExecutor, Path.Combine(baseScriptPath, "Triggers"));

        var procedureDirectories = new[] { "UserScripts", "SubredditScripts", "CommentScripts", "PostScripts" };
        foreach (var dir in procedureDirectories)
        {
            await ExecuteScriptsInDirectory(sqlScriptExecutor, Path.Combine(baseScriptPath, dir));
        }
        Console.WriteLine("All scripts executed successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error executing SQL scripts: {ex.Message}");
    }
}

async Task ExecuteScriptsInDirectory(SqlScriptExecutor sqlScriptExecutor, string directoryPath)
{
    if (Directory.Exists(directoryPath))
    {
        var scriptFiles = Directory.GetFiles(directoryPath, "*.sql");
        foreach (var scriptFile in scriptFiles)
        {
            await ExecuteScriptWithLogging(sqlScriptExecutor, scriptFile);
        }
    }
    else 
        Console.WriteLine($"Directory not found: {directoryPath}");   
}
async Task ExecuteScriptWithLogging(SqlScriptExecutor sqlScriptExecutor, string scriptFilePath)
{
    try
    {
        Console.WriteLine($"Executing script: {scriptFilePath}");
        await sqlScriptExecutor.ExecuteSqlScriptAsync(scriptFilePath);
        Console.WriteLine($"Successfully executed script: {scriptFilePath}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error executing script: {scriptFilePath}");
        Console.WriteLine($"Detailed error: {ex.Message}");
        throw;
    }
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
