using System.ComponentModel.DataAnnotations;

namespace GestaoDeTarefasAPI.Models
{
     public class Tarefa
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O título da tarefa é obrigatório.")] 
        [MaxLength(100, ErrorMessage = "O título não pode exceder 100 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "A descrição não pode exceder 500 caracteres.")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "O status da tarefa é obrigatório.")]
        [EnumDataType(typeof(StatusTarefa), ErrorMessage = "Status inválido. Use Pendente(0), EmAndamento(1) ou Concluido(2).")]
        public StatusTarefa Status { get; set; } = StatusTarefa.Pendente;

        [Required(ErrorMessage = "A data de vencimento é obrigatória.")]
        public DateTime DataDeVencimento { get; set; }
    }


    public enum StatusTarefa
    {
        Pendente = 0,
        EmAndamento = 1,
        Concluido = 2
    }
}
