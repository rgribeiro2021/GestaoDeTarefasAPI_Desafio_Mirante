using GestaoDeTarefasAPI.Repositories;

public interface IUnitOfWork : IDisposable
{
    ITarefaRepository TarefaRepository { get; }
    Task CommitAsync();
}