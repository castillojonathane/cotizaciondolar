using CotizacionDolarBNA.Models;
using CotizacionDolarBNA.Repository.Interface;
using CotizacionDolarBNA.Repository.Repository;
using CotizacionDolarBNA.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEntityFrameworkNpgsql()
.AddDbContext<ErreparContext>(option => option.UseNpgsql(builder.Configuration.GetConnectionString("conexionDB")));

// Agrego CORS
builder.Services.AddCors();

builder.Services.AddScoped<ICotizacionDolar, CotizacionDolar>();

builder.Services.AddTransient<CotizacionDolarService>();

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapSwagger();
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(c =>
{
    c.AllowAnyHeader();
    c.AllowAnyMethod();
    c.AllowAnyOrigin();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
