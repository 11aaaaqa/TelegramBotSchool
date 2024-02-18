using Hangfire;
using Microsoft.EntityFrameworkCore;
using TelegramBotSchool.Commands;
using TelegramBotSchool.Database;
using TelegramBotSchool.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHangfire(x => x.UseSqlServerStorage(
    "Server=(LocalDB)\\MSSQLLocalDB;Database=TelegramBotSchool;Integrated Security=true"));
builder.Services.AddHangfireServer();

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(
    "Server=(LocalDB)\\MSSQLLocalDB;Database=TelegramBotSchool;Integrated Security=true"));
builder.Services.AddSingleton<Bot>();
builder.Services.AddTransient<ICommandExecutor, CommandExecutor>();
builder.Services.AddTransient<IBaseCommand, StartCommand>();
builder.Services.AddTransient<IBaseCommand, WriteDifferenceCommand>();
builder.Services.AddTransient<IBaseCommand, ChangeDifferenceCommand>();
builder.Services.AddTransient<IBaseCommand, ChangeDifferenceExecutor>();
builder.Services.AddTransient<IBaseCommand, ShowAllCommand>();
builder.Services.AddTransient<IBaseCommand, AddReminderCommand>();
builder.Services.AddTransient<IBaseCommand, AddTextToReminderCommand>();
builder.Services.AddTransient<IBaseCommand, AddTimeToReminderCommand>();
builder.Services.AddTransient<IBaseCommand, GetToBackCommand>();
builder.Services.AddTransient<IBaseCommand, DeleteReminderCommand>();
builder.Services.AddTransient<IBaseCommand, DeleteReminderExecutor>();
builder.Services.AddTransient<IGetInlineKeyboardMarkup, GetInlineKeyBoardMarkup>();

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
