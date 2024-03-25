using BusinessLayer.InterfaceBl;
using BusinessLayer.ServiceBl;
using RepositoryLayer.Context;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;

var builder = WebApplication.CreateBuilder(args);

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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
