using Microsoft.EntityFrameworkCore;
using Talabat.Core.IRepository;
using Talabat.Repository.Data;
using Talabat.Repository.Repositories;
using AutoMapper;
using Talabat.API.Mapping;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Errors;
using Talabat.Repository.Middlewares;
using Talabat.API.Config_s_Extensions;
using StackExchange.Redis;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Talabat.Repository.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSwaggerService();

builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TalabatConnection"));
});

builder.Services.AddDbContext<AppIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TalabatIdentityConnection"));
});

builder.Services.AddIdentityServices(builder.Configuration);

// Redis Connection Configurations
builder.Services.AddRedis(builder);

builder.Services.AddApplicationServices();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", options =>
    {
        options.AllowAnyHeader().AllowAnyMethod().WithOrigins(builder.Configuration["FrontURL"]);
    });
});

var app = builder.Build();

#region Migrate Async

await app.AddMigrateAsyncMiddleware();

#endregion

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.AddSwaggerMiddleware();
}

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors();

app.MapControllers();

app.Run();
