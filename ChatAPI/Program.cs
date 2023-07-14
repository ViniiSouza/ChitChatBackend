using AutoMapper;
using Chat.Infra.Contexts;
using Chat.Security;
using ChatAPI.Hubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// database configuration
builder.Services.AddDbContext<ChatDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("UserConnection"));
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Chat API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

// dependency injection
builder.Services.AddDependencies();

string profilesPath = String.Empty;
string settingsName = String.Empty;
#if DEBUG
profilesPath = @"..\Chat\bin\Debug\net6.0\Chat.dll";
settingsName = "appsettings.Development.json";
#else
profilesPath = @"..\Chat\bin\Release\net6.0\Chat.dll";
settingsName = "appsettings.json";
#endif

// AutoMapper profiles injection
var assembly = Assembly.LoadFrom(profilesPath);
var profileTypes = assembly.GetTypes()
    .Where(type => type.Name.EndsWith("Profile") && typeof(Profile).IsAssignableFrom(type) && type != typeof(Profile))
    .ToArray();
if (profileTypes != null && profileTypes.Length > 0)
    builder.Services.AddAutoMapper(profileTypes);

// adds appsettings to configuration
builder.Configuration.AddJsonFile(settingsName, optional: false, reloadOnChange: true);
var config = builder.Configuration;
builder.Services.AddSingleton<IConfiguration>(config);

// inject configuration into token service
TokenService.Initialize(config);
// configure authentication
var key = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("Secret"));
builder.Services.AuthenticationConfigure(key);

builder.Services.AddSignalR();

var app = builder.Build();

app.UseCors(builder => builder
.AllowAnyOrigin()
.AllowAnyMethod()
.AllowAnyHeader());

app.MapHub<ChatHub>("/hubs/chat");

// Configure the HTTP request pipeline.
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
