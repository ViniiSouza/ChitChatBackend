using AutoMapper;
using Chat.Helpers;
using Chat.Hubs;
using Chat.Infra.Contexts;
using Chat.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
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

var assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(a => a.GetName().Name == "Chat");
var profileTypes = assembly.GetTypes()
    .Where(type => type.Name.EndsWith("Profile") && typeof(Profile).IsAssignableFrom(type) && type != typeof(Profile))
    .ToArray();
if (profileTypes != null && profileTypes.Length > 0)
    builder.Services.AddAutoMapper(profileTypes);

// adds appsettings to configuration
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
}
else
{
    builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
}
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

app.UseSwagger();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();
