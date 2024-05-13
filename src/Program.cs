using auth.Data;
using auth.Helpers;
using Backend.Helpers;
using Backend.Middlewares;
using Backend.Models;
using Backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

/**

Set up the infrastructure for a backend API, including service registration, 
database context configuration, middleware setup, and controller mapping and integrate 
Swagger for API documentation in the development environment, with CORS configuration 
and error handling middleware.
*/

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Backend Teamwork API", Version = "v1" });
    c.AddSwaggerExamples();
});

builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<JwtService>();

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder.WithOrigins("http://localhost:3000", "http://localhost:8080", "http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.AddControllers();

// Database context configuration
builder.Services.AddDbContext<EcommerceSdaContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Development specific configurations
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend Teamwork API V1");
    });
}

// Middleware setup
app.UseHttpsRedirection();

// Authentication and Authorization setup
app.UseAuthentication();
app.UseAuthorization();

// CORS setup
app.UseCors("CorsPolicy");

// Error handling middleware
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsync("An unexpected error occurred.");
    });
});
app.UseMiddleware<ExceptionHandling>();

// Finalize setup and start listening for incoming requests
app.MapControllers();
app.Run();

