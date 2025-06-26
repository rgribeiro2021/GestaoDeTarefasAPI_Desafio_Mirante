using GestaoDeTarefasAPI.Data;
using GestaoDeTarefasAPI.Models;
using GestaoDeTarefasAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoDeTarefasAPI.Tests
{
    public class TarefaRepositoryTests
    {

        private ToDoContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ToDoContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;
            var context = new ToDoContext(options);

            context.Database.EnsureCreated();
            return context;
        }

        [Fact] 
        public async Task AddAsync_AddsNewTarefaToDatabase()
        {
            
            using var context = GetInMemoryDbContext(); 
            var repository = new TarefaRepository(context); 

            var newTarefa = new Tarefa
            {
                Titulo = "Comprar Leite",
                Descricao = "Leite integral no supermercado",
                Status = StatusTarefa.Pendente,
                DataDeVencimento = DateTime.Now.AddDays(1)
            };

            // Act (Ação)
            await repository.CriaTarefa(newTarefa); 
            await context.SaveChangesAsync(); 

            // Assert (Verificação)
            
            Assert.NotEqual(0, newTarefa.Id); 
            var addedTarefa = await context.Tarefas.FindAsync(newTarefa.Id);
            Assert.NotNull(addedTarefa);
            Assert.Equal("Comprar Leite", addedTarefa.Titulo);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectTarefa()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new TarefaRepository(context);

            var tarefa1 = new Tarefa { Titulo = "Tarefa 1", Status = StatusTarefa.Pendente, DataDeVencimento = DateTime.Now.AddDays(1) };
            var tarefa2 = new Tarefa { Titulo = "Tarefa 2", Status = StatusTarefa.Concluido, DataDeVencimento = DateTime.Now.AddDays(2) };
            await context.Tarefas.AddRangeAsync(tarefa1, tarefa2);
            await context.SaveChangesAsync(); 

            // Act
            var result = await repository.RecuperaTarefaPorId(tarefa1.Id); 

            // Assert
            Assert.NotNull(result);
            Assert.Equal(tarefa1.Titulo, result.Titulo);
            Assert.Equal(tarefa1.Id, result.Id);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllTarefas()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new TarefaRepository(context);

            await context.Tarefas.AddRangeAsync(
                new Tarefa { Titulo = "Tarefa A", Status = StatusTarefa.Pendente, DataDeVencimento = DateTime.Now.AddDays(1) },
                new Tarefa { Titulo = "Tarefa B", Status = StatusTarefa.EmAndamento, DataDeVencimento = DateTime.Now.AddDays(2) }
            );
            await context.SaveChangesAsync();

            // Act
            var result = await repository.RecuperaTodasAsTarefas(); 

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Theory] 
        [InlineData(StatusTarefa.Pendente, 1)] 
        [InlineData(StatusTarefa.Concluido, 1)] 
        [InlineData(null, 3)]
        public async Task GetAllAsync_FiltersByStatusCorrectly(StatusTarefa? statusFilter, int expectedCount)
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new TarefaRepository(context);

            await context.Tarefas.AddRangeAsync(
                new Tarefa { Titulo = "T1", Status = StatusTarefa.Pendente, DataDeVencimento = DateTime.Now.AddDays(1) },
                new Tarefa { Titulo = "T2", Status = StatusTarefa.EmAndamento, DataDeVencimento = DateTime.Now.AddDays(2) },
                new Tarefa { Titulo = "T3", Status = StatusTarefa.Concluido, DataDeVencimento = DateTime.Now.AddDays(3) }
            );
            await context.SaveChangesAsync();

            // Act
            var result = await repository.RecuperaTodasAsTarefas(status: statusFilter);

            // Assert
            Assert.Equal(expectedCount, result.Count());
        }

        [Fact]
        public async Task GetAllAsync_FiltersByDataDeVencimentoCorrectly()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new TarefaRepository(context);

            var today = DateTime.Today;
            await context.Tarefas.AddRangeAsync(
                new Tarefa { Titulo = "Hoje", Status = StatusTarefa.Pendente, DataDeVencimento = today },
                new Tarefa { Titulo = "Amanhã", Status = StatusTarefa.EmAndamento, DataDeVencimento = today.AddDays(1) },
                new Tarefa { Titulo = "Ontem", Status = StatusTarefa.Concluido, DataDeVencimento = today.AddDays(-1) }
            );
            await context.SaveChangesAsync();

            // Act
            var result = await repository.RecuperaTodasAsTarefas(dataDeVencimento: today);

            // Assert
            Assert.Single(result); 
            Assert.Equal("Hoje", result.First().Titulo);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesExistingTarefa()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new TarefaRepository(context);

            var tarefa = new Tarefa { Titulo = "Tarefa Original", Status = StatusTarefa.Pendente, DataDeVencimento = DateTime.Now.AddDays(1) };
            await repository.AtualizaTarefa(tarefa);
            await context.SaveChangesAsync(); 

            // Modifica a tarefa
            tarefa.Titulo = "Tarefa Atualizada";
            tarefa.Status = StatusTarefa.Concluido;
            tarefa.Descricao = "Nova descrição";
            tarefa.DataDeVencimento = DateTime.Now.AddDays(5);

            // Act
            await repository.AtualizaTarefa(tarefa); 
            await context.SaveChangesAsync(); 

            // Assert
            var updatedTarefa = await context.Tarefas.FindAsync(tarefa.Id);
            Assert.NotNull(updatedTarefa);
            Assert.Equal("Tarefa Atualizada", updatedTarefa.Titulo);
            Assert.Equal(StatusTarefa.Concluido, updatedTarefa.Status);
            Assert.Equal("Nova descrição", updatedTarefa.Descricao);
            Assert.Equal(tarefa.DataDeVencimento, updatedTarefa.DataDeVencimento);
        }

        [Fact]
        public async Task DeleteAsync_RemovesTarefaFromDatabase()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new TarefaRepository(context);

            var tarefa = new Tarefa { Titulo = "Tarefa a ser excluída", Descricao = "Descrição", Status = StatusTarefa.Pendente, DataDeVencimento = DateTime.Now.AddDays(1) };
            await repository.CriaTarefa(tarefa);
            await context.SaveChangesAsync(); 

            // Act
            await repository.ExcluiTarefa(tarefa.Id); 
            await context.SaveChangesAsync();

            // Assert
            var deletedTarefa = await context.Tarefas.FindAsync(tarefa.Id);
            Assert.Null(deletedTarefa); 
        }

        [Fact]
        public async Task DeleteAsync_NonExistingTarefa_DoesNothing()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var repository = new TarefaRepository(context);

            // Act & Assert 
            var initialCount = await context.Tarefas.CountAsync(); 
            await repository.ExcluiTarefa(999);
            await context.SaveChangesAsync(); 
            var finalCount = await context.Tarefas.CountAsync();

            Assert.Equal(initialCount, finalCount);
        }
    }
}
