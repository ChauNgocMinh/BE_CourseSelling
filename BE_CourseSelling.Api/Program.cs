using BE_CourseSelling.Api.Configurations;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using BE_CourseSelling.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddServiceConfigs(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Apply any pending EF Core migrations automatically on startup.
// Migrations are located in the BE_CourseSelling.Infrastructure project (see DbContext configuration).
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
        app.Logger.LogInformation("Database migrations applied successfully.");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred while applying database migrations.");
        throw;
    }
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.Lifetime.ApplicationStarted.Register(() =>
    {
        _ = Task.Run(() => RunPostmanSync(app, builder.Environment.ContentRootPath));
    });
}

app.Run();

static async Task RunPostmanSync(WebApplication app, string projectRoot)
{
    var scriptPath = Path.Combine(projectRoot, "sync-postman.cmd");

    if (!File.Exists(scriptPath))
    {
        app.Logger.LogError("Postman sync script not found: {ScriptPath}", scriptPath);
        return;
    }

    app.Logger.LogInformation("Starting Postman sync script: {ScriptPath}", scriptPath);

    using var process = new Process
    {
        StartInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            Arguments = $"/c \"{scriptPath}\"",
            WorkingDirectory = projectRoot,
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        },
        EnableRaisingEvents = true
    };

    process.OutputDataReceived += (_, e) => LogPostmanOutput(app, e.Data);
    process.ErrorDataReceived += (_, e) => LogPostmanError(app, e.Data);

    try
    {
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        await process.WaitForExitAsync();

        if (process.ExitCode == 0)
        {
            app.Logger.LogInformation("Postman sync completed successfully.");
            return;
        }

        app.Logger.LogError("Postman sync failed with exit code: {ExitCode}", process.ExitCode);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Failed to run Postman sync script.");
    }
}

static void LogPostmanOutput(WebApplication app, string? data)
{
    if (!string.IsNullOrWhiteSpace(data))
    {
        app.Logger.LogInformation("[Postman Sync] {Message}", data.Trim());
    }
}

static void LogPostmanError(WebApplication app, string? data)
{
    if (string.IsNullOrWhiteSpace(data))
    {
        return;
    }

    var message = data.Trim();

    if (
        message.Contains("ExperimentalWarning: localStorage is not available") ||
        message.Contains("Use `node --trace-warnings")
    )
    {
        return;
    }

    app.Logger.LogError("[Postman Sync Error] {Message}", message);
}