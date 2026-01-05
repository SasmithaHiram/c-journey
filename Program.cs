using WebApplication1.Data;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers.Models;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Add Postgres DbContext
builder.Services.AddDbContext<QuoteDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed sample data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<QuoteDbContext>();
    // Ensure database and tables exist
    db.Database.EnsureCreated();

    if (!db.Quotes.Any())
    {
        db.Quotes.AddRange(
            new Quote { Text = "The only limit to our realization of tomorrow is our doubts of today.", Author = "Franklin D. Roosevelt", Category = "Inspirational", Emoji = "??" },
            new Quote { Text = "Life is what happens when you're busy making other plans.", Author = "John Lennon", Category = "Life", Emoji = "???" },
            new Quote { Text = "Do not take life too seriously. You will never get out of it alive.", Author = "Elbert Hubbard", Category = "Humor", Emoji = "??" }
        );
        db.SaveChanges();
    }
}

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
