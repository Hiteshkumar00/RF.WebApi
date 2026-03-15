using Microsoft.EntityFrameworkCore;
using RF.WebApi.Api.Apis.Authentication;
using RF.WebApi.Api.Apis.Extensions;
using RF.WebApi.Api.Apis.Middleware;
using RF.WebApi.Infrastructure.Data.DataBase;

var builder = WebApplication.CreateBuilder(args);

// --- 1. ADD OPEN CORS SERVICE ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()   // Allows any website/localhost port
                  .AllowAnyMethod()   // Allows GET, POST, PUT, DELETE, etc.
                  .AllowAnyHeader();  // Allows any Headers (Content-Type, Authorization)
        });
});

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

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();