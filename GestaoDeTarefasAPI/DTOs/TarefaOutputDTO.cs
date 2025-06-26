using GestaoDeTarefasAPI.Models;

namespace GestaoDeTarefasAPI.DTOs
{
    public class TarefaOutputDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public StatusTarefa Status { get; set; }
        public DateTime DataDeVencimento { get; set; }

        public static TarefaOutputDTO FromModel(Tarefa model)
        {
            return new TarefaOutputDTO
            {
                Id = model.Id,
                Titulo = model.Titulo,
                Descricao = model.Descricao,
                Status = model.Status,
                DataDeVencimento = model.DataDeVencimento
            };
        }
    }
}
