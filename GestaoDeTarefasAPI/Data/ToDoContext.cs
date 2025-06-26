using GestaoDeTarefasAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GestaoDeTarefasAPI.Data
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options)
        {
        }

        public DbSet<Tarefa> Tarefas { get; set; }
    }
}
