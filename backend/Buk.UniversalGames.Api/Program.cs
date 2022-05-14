using Buk.UniversalGames.Api;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>();

var app = builder.Build();

// Automatically migrate database
if (app.Environment.IsDevelopment())
{
    await Task.Delay(5000); //wait for DB to start up
}
var db = app.Services.CreateScope().ServiceProvider.GetService<DataContext>()!.Database;
await db.MigrateAsync();

app.UseSwagger();
app.UseSwaggerUI();

app.Map("/", () => Results.Redirect("/swagger"));


app.UseAuthorization();

app.MapControllers();

app.Run();
