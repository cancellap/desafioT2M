﻿using System.ComponentModel.DataAnnotations.Schema;

public class Projeto
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string? Descricao { get; set; }

    [Column("data_inicio")]
    public DateTime DataInicio { get; set; }

    [Column("data_termino")]
    public DateTime DataTermino { get; set; }

    [Column("usuario_id")]
    public int UsuarioId { get; set; }
    public List<Tarefa> Tarefas { get; set; } = new List<Tarefa>();


    public Projeto() { }

    public Projeto(string nome, string? descricao, DateTime dataInicio, DateTime dataTermino, List<Tarefa> tarefas, int usuarioId)
    {
        Nome = nome;
        Descricao = descricao;
        DataInicio = dataInicio.Date;
        DataTermino = dataTermino.Date;
        Tarefas = tarefas ?? new List<Tarefa>();
        UsuarioId = usuarioId;
    }

    public Projeto(ProjetoDto projetoDto)
    {
        Nome = projetoDto.Nome;
        Descricao = projetoDto.Descricao;

        DataInicio = DateTime.TryParse(projetoDto.DataInicio, out var dataInicio) ? dataInicio.Date : DateTime.MinValue;
        DataTermino = DateTime.TryParse(projetoDto.DataTermino, out var dataTermino) ? dataTermino.Date : DateTime.MinValue;
        UsuarioId = projetoDto.UsuarioId;
        Tarefas = projetoDto.Tarefas?.Select(tarefaDto => new Tarefa(tarefaDto)).ToList() ?? new List<Tarefa>();
    }


}
