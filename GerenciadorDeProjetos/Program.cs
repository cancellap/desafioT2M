using Microsoft.EntityFrameworkCore;
using GerenciadorDeProjetos.Domain.Entities;
using GerenciadorDeProjetos.Application.Services;
using GerenciadorDeProjetos.Domain.Interface;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
