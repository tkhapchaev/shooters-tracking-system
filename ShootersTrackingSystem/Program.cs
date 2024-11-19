using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShootersTrackingSystem;
using ShootersTrackingSystem.Database;

var builder = WebApplication.CreateBuilder(args);
var key = Encoding.ASCII.GetBytes(builder.Configuration["Keys:JwtSecretKey"] ?? throw new ArgumentNullException("JwtSecretKey"));

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var application = builder.Build();

using (var scope = application.Services.CreateScope())
{
    var repository = scope.ServiceProvider.GetRequiredService<ShootersDbContext>();
    repository.Database.Migrate();
}

startup.Configure(application, builder.Environment);

application.UseHttpsRedirection();
application.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

application.MapControllers();
application.Run();