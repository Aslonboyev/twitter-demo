//-> Services
using BlogApp.WebApi.AuthManagement;
using BlogApp.WebApi.DbContexts;
using BlogApp.WebApi.Extensions;
using BlogApp.WebApi.Helpers;
using BlogApp.WebApi.Interfaces.Repositories;
using BlogApp.WebApi.Interfaces.Services;
using BlogApp.WebApi.Middlewares;
using BlogApp.WebApi.Repositories;
using BlogApp.WebApi.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

//-> Database
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgressDb"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

//-> Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();

//-> Services
builder.Services.AddServices();
builder.Services.AddSwaggerAuthorization();
builder.Services.AddJwtService(builder.Configuration);
builder.Services.AddHttpContextAccessor();

//Serilog
builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

//->Middleware 
builder.Host.UseSerilog((hostingContext, loggerConfiduration) =>
                         loggerConfiduration.ReadFrom.Configuration(hostingContext.Configuration));

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Services.GetService<IHttpContextAccessor>() != null)
    HttpContextHelper.Accessor = app.Services.GetRequiredService<IHttpContextAccessor>();

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHadlerMiddleware>();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();