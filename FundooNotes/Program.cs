
using BusinessLayer.InterfaceBl;
using BusinessLayer.ServiceBl;
using Confluent.Kafka;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using NLog.Web;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
//----------------------------------------------------------------------------------------------------------------------------------

/*//Logging
builder.Services.AddLogging(config =>
{
    config.ClearProviders();
    config.AddConsole();
    config.AddDebug();

});*/


//-----------------------------------------------------------------------------------------------------------------------------------------------
var logpath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
NLog.GlobalDiagnosticsContext.Set("LogDirectory", logpath);
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
object value = builder.Host.UseNLog();

//---------------------------------------------------------------------------------------------------------------------------------------

//redis

builder.Services.AddStackExchangeRedisCache(options => { options.Configuration = builder.Configuration["RedisCacheUrl"]; });

//---------------------------------------------------------------------------------------------------------------------------------------
// Kafka Producer Configuration
var producerConfig = new ProducerConfig();
builder.Configuration.GetSection("producer").Bind(producerConfig);
builder.Services.AddSingleton(producerConfig);
//---------------------------------------------------------------------------------------------------------------------------------------------
// Add services to the container.
builder.Services.AddSingleton<DapperContext>();
//User
builder.Services.AddScoped<IUser, UserService>();
builder.Services.AddScoped<IUserBl, UserServiceBl>();
//UserNote
builder.Services.AddScoped<IUserNote, UserNoteService>();
builder.Services.AddScoped<IUserNoteBl, UserNoteServiceBl>();
//userCollaboration
builder.Services.AddScoped<IUserCollaborator, UserCollaboratorService>();
builder.Services.AddScoped<IUserCollaboratorBl, UserCollaboratorServiceBl>();
//UserNoteLabel
builder.Services.AddScoped<IUserNoteLabel, UserNoteLabelService>();
builder.Services.AddScoped<IUserNoteLabelBl, UserNoteLabelServiceBl>();


builder.Services.AddControllers();


builder.Services.AddCors((setup) =>
{
    setup.AddPolicy("default", (op) =>
    {
        op.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
    });
});
//---------------------------------------------------------------------------------------------------------------------------------------

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Authorization

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Get USerNotes based on ID", Version = "v1" });

    // Define the JWT bearer scheme
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = JwtBearerDefaults.AuthenticationScheme
        }
    };

    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);

    // Require JWT tokens to be passed on requests
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            securityScheme,
            Array.Empty<string>()
        }
    });
});
builder.Services.AddDistributedMemoryCache();

//jwt

// Add JWT authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]));
//var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true; // Set to true in production
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,



        ClockSkew = TimeSpan.Zero,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key
    };
});


//Ending...

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:7186")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithHeaders(HeaderNames.ContentType);
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthorization();
app.MapControllers();

app.Run();

//----------------------------------------------------------------------------------------------------------------------
















