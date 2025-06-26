using GestaoDeTarefasAPI.Data;
using GestaoDeTarefasAPI.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ToDoContext _context;

    public UnitOfWork(ToDoContext context)
    {
        _context = context;
        TarefaRepository = new TarefaRepository(_context);
    }

    public ITarefaRepository TarefaRepository { get; private set; }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}