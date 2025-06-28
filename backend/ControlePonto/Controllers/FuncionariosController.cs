using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ControlePonto.Data;
using ControlePonto.Models;
using ControlePonto.DTOs;

namespace ControlePonto.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de funcionários
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FuncionariosController : ControllerBase
    {
        private readonly ControlePontoContext _context;
        private readonly ILogger<FuncionariosController> _logger;

        public FuncionariosController(ControlePontoContext context, ILogger<FuncionariosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retorna todos os funcionários
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FuncionarioResponseDto>>> GetFuncionarios()
        {
            try
            {
                var funcionarios = await _context.Funcionarios
                    .Select(f => new FuncionarioResponseDto
                    {
                        Id = f.Id,
                        Nome = f.Nome,
                        Email = f.Email,
                        Cargo = f.Cargo
                    })
                    .ToListAsync();

                _logger.LogInformation("Listagem de funcionários realizada com sucesso. Total: {Count}", funcionarios.Count);
                return Ok(funcionarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar funcionários");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Retorna um funcionário específico por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<FuncionarioResponseDto>> GetFuncionario(int id)
        {
            try
            {
                var funcionario = await _context.Funcionarios
                    .Where(f => f.Id == id)
                    .Select(f => new FuncionarioResponseDto
                    {
                        Id = f.Id,
                        Nome = f.Nome,
                        Email = f.Email,
                        Cargo = f.Cargo
                    })
                    .FirstOrDefaultAsync();

                if (funcionario == null)
                {
                    _logger.LogWarning("Funcionário com ID {Id} não encontrado", id);
                    return NotFound($"Funcionário com ID {id} não encontrado");
                }

                _logger.LogInformation("Funcionário {Id} encontrado com sucesso", id);
                return Ok(funcionario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar funcionário com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Cria um novo funcionário
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<FuncionarioResponseDto>> PostFuncionario(FuncionarioCreateDto funcionarioDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verificar se já existe funcionário com este email
                var existeEmail = await _context.Funcionarios
                    .AnyAsync(f => f.Email.ToLower() == funcionarioDto.Email.ToLower());

                if (existeEmail)
                {
                    return BadRequest("Já existe um funcionário cadastrado com este e-mail");
                }

                var funcionario = new Funcionario
                {
                    Nome = funcionarioDto.Nome,
                    Email = funcionarioDto.Email,
                    Cargo = funcionarioDto.Cargo
                };

                _context.Funcionarios.Add(funcionario);
                await _context.SaveChangesAsync();

                var responseDto = new FuncionarioResponseDto
                {
                    Id = funcionario.Id,
                    Nome = funcionario.Nome,
                    Email = funcionario.Email,
                    Cargo = funcionario.Cargo
                };

                _logger.LogInformation("Funcionário criado com sucesso. ID: {Id}, Nome: {Nome}", funcionario.Id, funcionario.Nome);
                return CreatedAtAction(nameof(GetFuncionario), new { id = funcionario.Id }, responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar funcionário");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Atualiza um funcionário existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFuncionario(int id, FuncionarioCreateDto funcionarioDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var funcionario = await _context.Funcionarios.FindAsync(id);
                if (funcionario == null)
                {
                    _logger.LogWarning("Tentativa de atualizar funcionário inexistente. ID: {Id}", id);
                    return NotFound($"Funcionário com ID {id} não encontrado");
                }

                // Verificar se o email já está sendo usado por outro funcionário
                var existeEmail = await _context.Funcionarios
                    .AnyAsync(f => f.Email.ToLower() == funcionarioDto.Email.ToLower() && f.Id != id);

                if (existeEmail)
                {
                    return BadRequest("Já existe outro funcionário cadastrado com este e-mail");
                }

                funcionario.Nome = funcionarioDto.Nome;
                funcionario.Email = funcionarioDto.Email;
                funcionario.Cargo = funcionarioDto.Cargo;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Funcionário {Id} atualizado com sucesso", id);
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FuncionarioExists(id))
                {
                    return NotFound();
                }
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar funcionário com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Remove um funcionário
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFuncionario(int id)
        {
            try
            {
                var funcionario = await _context.Funcionarios.FindAsync(id);
                if (funcionario == null)
                {
                    _logger.LogWarning("Tentativa de deletar funcionário inexistente. ID: {Id}", id);
                    return NotFound($"Funcionário com ID {id} não encontrado");
                }

                _context.Funcionarios.Remove(funcionario);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Funcionário {Id} removido com sucesso", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover funcionário com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        private bool FuncionarioExists(int id)
        {
            return _context.Funcionarios.Any(e => e.Id == id);
        }
    }
}
