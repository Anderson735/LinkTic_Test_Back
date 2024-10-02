using LinkTic_Test_Back.Application.Services;
using LinkTic_Test_Back.Domain.Interfaces;
using LinkTic_Test_Back.Domain.Services;
using LinkTic_Test_Back.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Agrega las dependencias
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();


// Agrega el contexto de la base de datos
builder.Services.AddDbContext<ECommerceContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agrega los controladores
builder.Services.AddControllers();


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
