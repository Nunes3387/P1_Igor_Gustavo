using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using P1_Igor_Gustavo.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<P1_Igor_GustavoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("P1_Igor_GustavoContext") ?? throw new InvalidOperationException("Connection string 'P1_Igor_GustavoContext' not found.")));

// Add services to the container.

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
