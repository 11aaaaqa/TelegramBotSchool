using Microsoft.EntityFrameworkCore;
using TelegramBotSchool.Commands;
using TelegramBotSchool.Database;
using TelegramBotSchool.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(
    "Server=(LocalDB)\\MSSQLLocalDB;Database=TelegramBotSchool;Integrated Security=true"));
builder.Services.AddSingleton<Bot>();
builder.Services.AddTransient<ICommandExecutor, CommandExecutor>();
builder.Services.AddTransient<BaseCommand, StartCommand>();
builder.Services.AddTransient<BaseCommand, WriteDifferenceCommand>();
builder.Services.AddTransient<BaseCommand, ChangeDifferenceCommand>();
builder.Services.AddTransient<BaseCommand,ChangeDifferenceExecutor>();
builder.Services.AddTransient<BaseCommand,ShowAllCommand>();
builder.Services.AddTransient<BaseCommand, AddReminderCommand>();
builder.Services.AddTransient<BaseCommand,AddTextToReminderCommand>();
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
