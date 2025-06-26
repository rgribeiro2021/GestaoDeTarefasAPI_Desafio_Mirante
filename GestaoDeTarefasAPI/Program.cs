using GestaoDeTarefasAPI.Data;
using GestaoDeTarefasAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.Data.SqlClient;
using System.Threading; 
using Microsoft.Extensions.Logging;
using GestaoDeTarefasAPI.Services; 

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ToDoContext>(options =>
    options.UseSqlServer("Server=sqlserver,1433;Database=GestaoDeTarefasDB;User Id=sa;Password=teste@123;TrustServerCertificate=True;Command Timeout=60;"));


builder.Services.AddScoped<ITarefaRepository, TarefaRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITarefaService, TarefaService>();

builder.Services.AddControllers();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ToDo API", Version = "v1" });
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ToDoContext>();
    var logger = services.GetRequiredService<ILogger<Program>>();

    int maxRetries = 5; 
    int delaySeconds = 10; 

    for (int i = 0; i < maxRetries; i++)
    {
        try
        {
            logger.LogInformation($"Tentando conectar ao banco de dados e aplicar migrações (tentativa {i + 1}/{maxRetries})...");
            context.Database.Migrate(); 
            logger.LogInformation("Banco de dados conectado e migrações aplicadas com sucesso!");
            break; 
        }
        catch (SqlException ex)
        {
            logger.LogError(ex, $"Erro ao conectar ao banco de dados ou aplicar migrações: {ex.Message}");
            if (i < maxRetries - 1)
            {
                logger.LogInformation($"Aguardando {delaySeconds} segundos antes de tentar novamente...");
                Thread.Sleep(TimeSpan.FromSeconds(delaySeconds));
            }
            else
            {
                logger.LogError("Número máximo de tentativas de conexão/migração atingido. A aplicação não pode iniciar.");
                throw; 
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ocorreu um erro inesperado durante a inicialização do banco de dados.");
            throw; 
        }
    }
}



if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}


app.UseRouting();
app.UseAuthorization();


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDo API v1");
});


app.MapControllers();


app.Run();