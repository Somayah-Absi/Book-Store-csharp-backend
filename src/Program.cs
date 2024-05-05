using Backend.Models;
using Backend.Services;
using Microsoft.EntityFrameworkCore;

/**

Set up the infrastructure for a backend API, including service registration, 
database context configuration, middleware setup, and controller mapping and integrate 
Swagger for API documentation in the development environment.
*/
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Backend Teamwork API", Version = "v1" });
});

builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderProductService>();

builder.Services.AddControllers();
builder.Services.AddDbContext<EcommerceSdaContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend Teamwork API V1");
    });
}

// Middleware is configured to redirect HTTP requests to HTTPS in order to enforce secure communication.
app.UseHttpsRedirection();

app.MapControllers();
app.Run();
