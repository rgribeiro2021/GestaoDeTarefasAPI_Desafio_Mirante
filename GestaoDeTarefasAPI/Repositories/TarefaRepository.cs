using GestaoDeTarefasAPI.Data;
using GestaoDeTarefasAPI.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GestaoDeTarefasAPI.Repositories
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly ToDoContext _context;

        public TarefaRepository(ToDoContext context)
        {
            _context = context;
        }

        public async Task<Tarefa> RecuperaTarefaPorId(int id)
        {
            return await _context.Tarefas.FindAsync(id);
        }

        //public async Task<IEnumerable<Tarefa>> RecuperaTodasAsTarefas() => await _context.Tarefas.ToListAsync();
        public async Task<IEnumerable<Tarefa>> RecuperaTodasAsTarefas(StatusTarefa? status = null, DateTime? dataDeVencimento = null)
        {
            var query = _context.Tarefas.AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(t => t.Status == status.Value);
            }

            if (dataDeVencimento.HasValue)
            {
                query = query.Where(t => t.DataDeVencimento.Date == dataDeVencimento.Value.Date);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Tarefa>> RecuperaTarefasPorStatus(StatusTarefa status) => await _context.Tarefas
            .Where(t => t.Status == status).ToListAsync();

        public async Task<IEnumerable<Tarefa>> RecuperaTarefasPorDataDeVencimento(DateTime dataDeVencimento) => await _context.Tarefas
            .Where(t => t.DataDeVencimento.Date == dataDeVencimento.Date).ToListAsync();

        public async Task CriaTarefa(Tarefa task)
        {
            await _context.Tarefas.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizaTarefa(Tarefa task)
        {
            _context.Tarefas.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task ExcluiTarefa(int id)
        {
            var task = await RecuperaTarefaPorId(id);
            if (task != null)
            {
                _context.Tarefas.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

    }
}
