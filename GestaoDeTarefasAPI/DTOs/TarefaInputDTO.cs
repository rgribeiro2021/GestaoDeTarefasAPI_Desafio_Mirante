using GestaoDeTarefasAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace GestaoDeTarefasAPI.DTOs
{

    public class TarefaInputDTO
    {
        [Required(ErrorMessage = "O título da tarefa é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O título não pode exceder 100 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "A descrição não pode exceder 500 caracteres.")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O status da tarefa é obrigatório.")]
        [EnumDataType(typeof(StatusTarefa), ErrorMessage = "Status inválido. Use Pendente(0), EmAndamento(1) ou Concluido(2).")]
        public StatusTarefa Status { get; set; }

        [Required(ErrorMessage = "A data de vencimento é obrigatória.")]
        public DateTime DataDeVencimento { get; set; }
    }
}
