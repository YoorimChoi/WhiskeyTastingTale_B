using Microsoft.EntityFrameworkCore;
using Whiskey_TastingTale_Backend.Repository;
using Whiskey_TastingTale_Backend.Repository.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<WhiskeyRepository>();
builder.Services.AddTransient<UserRepository>();

builder.Services.AddDbContext<WhiskeyContext>(
        options => options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Whiskey_TT;Trusted_Connection=true;Encrypt=false;TrustServerCertificate=true;"));
builder.Services.AddDbContext<UserContext>(
        options => options.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Whiskey_TT;Trusted_Connection=true;Encrypt=false;TrustServerCertificate=true;"));


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
