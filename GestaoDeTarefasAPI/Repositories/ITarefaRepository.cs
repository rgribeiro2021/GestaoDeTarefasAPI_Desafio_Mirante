using GestaoDeTarefasAPI.Models;

namespace GestaoDeTarefasAPI.Repositories
{
    public interface ITarefaRepository
    {
        Task<Tarefa> RecuperaTarefaPorId(int id);
        Task<IEnumerable<Tarefa>> RecuperaTodasAsTarefas(StatusTarefa? status = null, DateTime? dataDeVencimento = null);
        Task<IEnumerable<Tarefa>> RecuperaTarefasPorStatus(StatusTarefa status);
        Task<IEnumerable<Tarefa>> RecuperaTarefasPorDataDeVencimento(DateTime dataDeVencimento);
        Task CriaTarefa(Tarefa tarefa);
        Task AtualizaTarefa(Tarefa tarefa);
        Task ExcluiTarefa(int id);
    }
}
