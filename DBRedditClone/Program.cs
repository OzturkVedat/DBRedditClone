using DBRedditClone.Services;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddSingleton<SqlScriptExecutor>(provider =>
new SqlScriptExecutor(connectionString));

builder.Services.AddSingleton<PostgresService>(sp =>
    new PostgresService(connectionString, sp.GetRequiredService<ILogger<PostgresService>>()));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "SqlScripts", "CreateSchema.sql");

using (var scope = app.Services.CreateScope())
{
    var sqlScriptExecutor = scope.ServiceProvider.GetRequiredService<SqlScriptExecutor>();
    try
    {
        await sqlScriptExecutor.ExecuteSqlScriptAsync(scriptPath);
        Console.WriteLine("Db schema created successfully.");

        var triggerFiles = Directory.GetFiles("SqlScripts/Triggers", "*.sql");
        foreach (var triggerFile in triggerFiles)
        {
            await sqlScriptExecutor.ExecuteSqlScriptAsync(triggerFile);
        }
        Console.WriteLine("Triggers are set successfully.");

        var procedureFiles = Directory.GetFiles("SqlScripts/UserScripts", "*.sql");
        foreach (var procFile in procedureFiles)
        {
            await sqlScriptExecutor.ExecuteSqlScriptAsync(procFile);
        }
        Console.WriteLine("Stored procedures are seeded and ready.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error executing SQL script: {ex.Message}");
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
