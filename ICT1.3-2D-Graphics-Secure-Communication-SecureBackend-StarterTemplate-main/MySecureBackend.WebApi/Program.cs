using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using MySecureBackend.WebApi.Repositories;
using MySecureBackend.WebApi.Services;
using System;
using System.Reflection;
using APIData;

var builder = WebApplication.CreateBuilder(args);

// Register MVC controllers for handling HTTP requests.
builder.Services.AddControllers();

var sqlConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Snippet 04
builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 8;
})
.AddRoles<IdentityRole>()
.AddDapperStores(options =>
{
    options.ConnectionString = sqlConnectionString;
});
// Snippet 04

// Register OpenAPI/Swagger for API documentation and testing.
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MySecureBackend API",
        Version = "v1",
    });
});

builder.Services.Configure<RouteOptions>(o => o.LowercaseUrls = true);

// Register IHttpContextAccessor for accessing HTTP context in services (e.g., to get current user info).
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IAuthenticationService, AspNetIdentityAuthenticationService>();

// Register application repositories.
// By default, use an in-memory repository for example objects.
builder.Services.AddTransient<IExampleObjectRepository, MemoryExampleObjectRepository>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// To use a SQL-backed repository instead, uncomment the following line:
//builder.Services.AddTransient<IExampleObjectRepository, SqlExampleObjectRepository>(o => new SqlExampleObjectRepository(sqlConnectionString!));

var app = builder.Build();

// Register OpenAPI/Swagger endpoints.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MySecureBackend API v1");
        options.RoutePrefix = "swagger"; // Access at /swagger
        options.CacheLifetime = TimeSpan.Zero; // Disable caching for development
    });
}
else
{
    // Show the health message directly in non-development environments
    var buildTimeStamp = File.GetCreationTime(Assembly.GetExecutingAssembly().Location);
    string currentHealthMessage = $"The API is up 🚀 | Connection string found: {(sqlConnectionString != null ? "✅" : "❌")} | Build timestamp: {buildTimeStamp}";

    app.MapGet("/", () => currentHealthMessage);
}

// Enforce HTTPS for all requests.
app.UseHttpsRedirection();

// Snippet 04

// Enable authorization middleware.
app.UseAuthorization();

// Register Identity endpoints for account management (register, login, etc.) under /account.
// 👇 uncomment the following line to enable Identity API endpoints to use authentication/authorization
//app.MapGroup("/account").MapIdentityApi<IdentityUser>().WithTags("Account");
app.MapGroup("/account").MapIdentityApi<IdentityUser>();

// Register all controller endpoints for the application.
app.MapControllers().RequireAuthorization();

app.Run();
// Snippet 04

/*
 * Object2D.ChangePos(); 200, 404, 401
 * Object2D.Scale(); 200, 404, 401
 * Object2D.Rotate(); 200, 404, 401
 * Object2D.Sort(); 200, 404, 401
 * 
 * Environment2D.Rename(); 200, 404, 401
 * Environment2D.Resize(); 200, 404, 401
 */