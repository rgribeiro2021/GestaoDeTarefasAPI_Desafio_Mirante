using GestaoDeTarefasAPI.DTOs;
using GestaoDeTarefasAPI.Models;

namespace GestaoDeTarefasAPI.Services
{
    public class TarefaService : ITarefaService
    {
        private readonly IUnitOfWork _unitOfWork;

        
        public TarefaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        
        public async Task<TarefaOutputDTO?> RecuperaTarefaPorIdAsync(int id)
        {
            var tarefa = await _unitOfWork.TarefaRepository.RecuperaTarefaPorId(id);
            return tarefa == null ? null : TarefaOutputDTO.FromModel(tarefa);
        }

        
        public async Task<IEnumerable<TarefaOutputDTO>> RecuperaTodasAsTarefasAsync(StatusTarefa? status = null, DateTime? dataVencimento = null)
        {
            var tarefas = await _unitOfWork.TarefaRepository.RecuperaTodasAsTarefas(status, dataVencimento);
            
            return tarefas.Select(TarefaOutputDTO.FromModel);
        }

        
        public async Task<TarefaOutputDTO> CriaTarefaAsync(TarefaInputDTO tarefaInput)
        {
            
            var tarefa = new Tarefa
            {
                Titulo = tarefaInput.Titulo,
                Descricao = tarefaInput.Descricao,
                Status = tarefaInput.Status,
                DataDeVencimento = tarefaInput.DataDeVencimento
            };

            await _unitOfWork.TarefaRepository.CriaTarefa(tarefa);
            await _unitOfWork.CommitAsync(); 

            
            return TarefaOutputDTO.FromModel(tarefa);
        }

        
        public async Task<TarefaOutputDTO?> AtualizaTarefaAsync(int id, TarefaInputDTO tarefaInput)
        {
            var existingTarefa = await _unitOfWork.TarefaRepository.RecuperaTarefaPorId(id);

            if (existingTarefa == null)
            {
                return null; 
            }

            
            existingTarefa.Titulo = tarefaInput.Titulo;
            existingTarefa.Descricao = tarefaInput.Descricao;
            existingTarefa.Status = tarefaInput.Status;
            existingTarefa.DataDeVencimento = tarefaInput.DataDeVencimento;

            await _unitOfWork.TarefaRepository.AtualizaTarefa(existingTarefa);
            await _unitOfWork.CommitAsync(); 

            
            return TarefaOutputDTO.FromModel(existingTarefa);
        }

        
        public async Task<bool> ExcluiTarefaAsync(int id)
        {
            var existingTarefa = await _unitOfWork.TarefaRepository.RecuperaTarefaPorId(id);
            if (existingTarefa == null)
            {
                return false; 
            }

            await _unitOfWork.TarefaRepository.ExcluiTarefa(id);
            await _unitOfWork.CommitAsync(); 
            return true;
        }
    }
}
