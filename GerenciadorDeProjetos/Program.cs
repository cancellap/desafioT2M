using Microsoft.EntityFrameworkCore;
using GerenciadorDeProjetos.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using GerenciadorDeProjetos.Presentation.Controllers;
using GerenciadorDeProjetos.Infrastructure.Data;
using GerenciadorDeProjetos.Domain.Services;
using GerenciadorDeProjetos.Infrastructure.Interfaces;
using GerenciadorDeProjetos.Application.Services;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
