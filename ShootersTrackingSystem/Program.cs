using Microsoft.EntityFrameworkCore;
using ShootersTrackingSystem;
using ShootersTrackingSystem.Database;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseRepository>();
    dbContext.Database.Migrate();
}

startup.Configure(app, builder.Environment);
app.Run();