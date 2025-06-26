using GestaoDeTarefasAPI.DTOs;
using GestaoDeTarefasAPI.Models;

namespace GestaoDeTarefasAPI.Services
{
    public interface ITarefaService
    {
        
        Task<TarefaOutputDTO?> RecuperaTarefaPorIdAsync(int id);

        
        Task<IEnumerable<TarefaOutputDTO>> RecuperaTodasAsTarefasAsync(StatusTarefa? status = null, DateTime? dataVencimento = null);

        
        Task<TarefaOutputDTO> CriaTarefaAsync(TarefaInputDTO tarefaInput);

        
        Task<TarefaOutputDTO?> AtualizaTarefaAsync(int id, TarefaInputDTO tarefaInput);

        
        Task<bool> ExcluiTarefaAsync(int id);
    }
}
