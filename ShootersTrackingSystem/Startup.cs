using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ShootersTrackingSystem.Database;
using ShootersTrackingSystem.Model.Services;

namespace ShootersTrackingSystem;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ShootersDbContext>(options => options.UseNpgsql(_configuration.GetConnectionString("database")));
        services.AddControllers();
        
        services.AddScoped<AuthService>();
        services.AddScoped<ResultsService>();
        
        services.AddDistributedMemoryCache();
        
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
    }

    public void Configure(IApplicationBuilder application, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            application.UseDeveloperExceptionPage();
            application.UseSwagger();
            application.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "ShootersTrackingSystem API V1"));
        }

        application.UseRouting();
        application.UseAuthentication();
        application.UseAuthorization();
        application.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}