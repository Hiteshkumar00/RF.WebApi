using Microsoft.EntityFrameworkCore;
using RF.WebApi.Api.Apis.Authentication;
using RF.WebApi.Api.Apis.Extensions;
using RF.WebApi.Api.Apis.Middleware;
using RF.WebApi.Api.Domain.Interfaces;
using RF.WebApi.Api.Infrastructure.Services;
using RF.WebApi.Infrastructure.Data.DataBase;

var builder = WebApplication.CreateBuilder(args);

//  CONTROLLERS & CONFIG 
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// Custom Configurations
builder.Services.ConfigureAutoMapper();
builder.Services.ConfigureCustomValidationErrorResponse();
builder.Services.AddIdentityServices(builder.Configuration);

// SERVICE REGISTRATION 
builder.Services.AddApplicationServices();


// Database
// Add-Migration 'InitialCreate' -Context RFDBContext
// Update-Database
builder.Services.AddDbContext<RFDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

var httpContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();
Token.Configure(httpContextAccessor);

// MIDDLEWARE PIPELINE 
app.UseCustomGlobalExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();