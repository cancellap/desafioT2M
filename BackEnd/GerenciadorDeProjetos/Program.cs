using Microsoft.EntityFrameworkCore;
using GerenciadorDeProjetos.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using GerenciadorDeProjetos.Presentation.Controllers;
using GerenciadorDeProjetos.Infrastructure.Data;
using GerenciadorDeProjetos.Domain.Services;
using GerenciadorDeProjetos.Infrastructure.Interfaces;
using GerenciadorDeProjetos.Application.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader() 
                  .AllowAnyMethod();
        });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ProjetoService>();
builder.Services.AddScoped<ProjetoRepository>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<TarefaService>();
builder.Services.AddScoped<TarefaRepository>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<AuthenticationController>();

builder.Services.AddSingleton<RabbitMqService>();

var consumer = new RabbitMqConsumer();

var app = builder.Build();
consumer.StartConsuming();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");


app.UseAuthorization();

app.MapControllers();

app.Run();
