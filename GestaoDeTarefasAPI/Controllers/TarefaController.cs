using GestaoDeTarefasAPI.DTOs;
using GestaoDeTarefasAPI.Models;
using GestaoDeTarefasAPI.Repositories;
using GestaoDeTarefasAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeTarefasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefaController : ControllerBase
    {
        private readonly ITarefaService _tarefaService;

        public TarefaController(ITarefaService tarefaService)
        {
            _tarefaService = tarefaService;
        }

        [HttpGet("recupera-todas-as-tarefas")]
        public async Task<ActionResult<IEnumerable<Tarefa>>> RecuperaTodasAsTarefas([FromQuery] StatusTarefa? status = null, [FromQuery] DateTime? dataDeVencimento = null)
        {
            var tarefas = await _tarefaService.RecuperaTodasAsTarefasAsync(status, dataDeVencimento);
            return Ok(tarefas);
        }


        [HttpGet("recupera-tarefa-por-id/{id}")]
        public async Task<ActionResult<Tarefa>> RecuperaTarefa(int id)
        {
            var tarefa = await _tarefaService.RecuperaTarefaPorIdAsync(id);
            if (tarefa == null)
            {
                return NotFound();
            }
            return Ok(tarefa);
        }

        [HttpPost]
        public async Task<ActionResult<Tarefa>> CriaTarefa([FromBody] TarefaInputDTO tarefaInput)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }
            var novaTarefa = await _tarefaService.CriaTarefaAsync(tarefaInput);
            return CreatedAtAction(nameof(RecuperaTarefa), new { id = novaTarefa.Id }, novaTarefa);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizaTarefa(int id, [FromBody] TarefaInputDTO tarefaInput)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); 
            }

            
            var tarefaAtualizada = await _tarefaService.AtualizaTarefaAsync(id, tarefaInput);

            if (tarefaAtualizada == null)
            {
                return NotFound(); 
            }

            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluiTarefa(int id)
        {
            var deleted = await _tarefaService.ExcluiTarefaAsync(id);
            if (!deleted)
            {
                return NotFound(); 
            }
            return NoContent();
        }

    }
}
