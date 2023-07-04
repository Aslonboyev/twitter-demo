using BlogApp.WebApi.DbContexts;
using BlogApp.WebApi.Extensions;
using BlogApp.WebApi.Helpers;
using BlogApp.WebApi.Hubs;
using BlogApp.WebApi.Middlewares;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//-> Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddServices();
builder.Services.AddSwaggerAuthorization();
builder.Services.AddJwtService(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();

//-> Database
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgressDb"));
});

//Serilog
//builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
//    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration));

var app = builder.Build();

//Middleware
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

app.MapHub<ServerHub>("/serverhub");

app.Run();