﻿using System.ComponentModel.DataAnnotations.Schema;
using GerenciadorDeProjetos.Domain.Enum;

namespace GerenciadorDeProjetos.Domain.DTOs
{
    public class TarefaDto

    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public String Prazo { get; set; }
        [Column("status_tarefa")]
        public string StatusTarefa { get; set; }
        [Column("usuario_id")]
        public int UsuarioId { get; set; }
        [Column("projeto_id")]
        public int ProjetoId { get; set; }

        public TarefaDto()
        {

        }

        public TarefaDto(Tarefa tarefa)
        {
            Id = tarefa.Id;
            Nome = tarefa.Nome;
            Descricao = tarefa.Descricao;
            Prazo = tarefa.Prazo.ToString("yyyy-MM-dd");
            StatusTarefa = tarefa.StatusTarefa.ToString(); // Retorna o valor textual do enum
            UsuarioId = tarefa.UsuarioId;
            ProjetoId = tarefa.ProjetoId;
        }

    }
}
