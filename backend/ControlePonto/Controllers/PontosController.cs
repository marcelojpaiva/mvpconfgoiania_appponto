using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ControlePonto.Data;
using ControlePonto.Models;
using ControlePonto.DTOs;

namespace ControlePonto.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de registros de ponto
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PontosController : ControllerBase
    {
        private readonly ControlePontoContext _context;
        private readonly ILogger<PontosController> _logger;

        public PontosController(ControlePontoContext context, ILogger<PontosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retorna registros de ponto de funcionários filtrando por cargo
        /// </summary>
        [HttpGet("por-cargo")]
        public async Task<ActionResult<IEnumerable<RegistroPontoResponseDto>>> GetPontosPorCargo(
            [FromQuery] string cargo,
            [FromQuery] DateTime? data = null,
            [FromQuery] DateTime? dataInicio = null,
            [FromQuery] DateTime? dataFim = null)
        {
            if (string.IsNullOrWhiteSpace(cargo))
            {
                return BadRequest("Cargo é obrigatório");
            }

            try
            {
                var query = _context.RegistrosPonto
                    .Include(r => r.Funcionario)
                    .Where(r => r.Funcionario.Cargo.ToLower() == cargo.ToLower())
                    .AsQueryable();

                if (data.HasValue)
                {
                    var dataFiltro = data.Value.Date;
                    query = query.Where(r => r.DataHoraEntrada.Date == dataFiltro);
                }
                else if (dataInicio.HasValue || dataFim.HasValue)
                {
                    if (dataInicio.HasValue)
                    {
                        query = query.Where(r => r.DataHoraEntrada.Date >= dataInicio.Value.Date);
                    }
                    if (dataFim.HasValue)
                    {
                        query = query.Where(r => r.DataHoraEntrada.Date <= dataFim.Value.Date);
                    }
                }

                var pontos = await query
                    .OrderByDescending(r => r.DataHoraEntrada)
                    .Select(r => new RegistroPontoResponseDto
                    {
                        Id = r.Id,
                        FuncionarioId = r.FuncionarioId,
                        NomeFuncionario = r.Funcionario.Nome,
                        DataHoraEntrada = r.DataHoraEntrada,
                        DataHoraSaida = r.DataHoraSaida,
                        Duracao = r.DataHoraSaida.HasValue
                            ? FormatDuration(r.DataHoraSaida.Value - r.DataHoraEntrada)
                            : null
                    })
                    .ToListAsync();

                _logger.LogInformation("Listagem de pontos por cargo realizada com sucesso. Cargo: {Cargo}, Total: {Count}", cargo, pontos.Count);
                return Ok(pontos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar registros de ponto por cargo {Cargo}", cargo);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Retorna todos os registros de ponto com filtros opcionais
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegistroPontoResponseDto>>> GetPontos(
            [FromQuery] int? funcionarioId = null,
            [FromQuery] DateTime? data = null,
            [FromQuery] DateTime? dataInicio = null,
            [FromQuery] DateTime? dataFim = null)
        {
            try
            {
                var query = _context.RegistrosPonto
                    .Include(r => r.Funcionario)
                    .AsQueryable();

                // Filtro por funcionário
                if (funcionarioId.HasValue)
                {
                    query = query.Where(r => r.FuncionarioId == funcionarioId.Value);
                }

                // Filtro por data específica
                if (data.HasValue)
                {
                    var dataFiltro = data.Value.Date;
                    query = query.Where(r => r.DataHoraEntrada.Date == dataFiltro);
                }
                // Filtro por período
                else if (dataInicio.HasValue || dataFim.HasValue)
                {
                    if (dataInicio.HasValue)
                    {
                        query = query.Where(r => r.DataHoraEntrada.Date >= dataInicio.Value.Date);
                    }
                    if (dataFim.HasValue)
                    {
                        query = query.Where(r => r.DataHoraEntrada.Date <= dataFim.Value.Date);
                    }
                }

                var pontos = await query
                    .OrderByDescending(r => r.DataHoraEntrada)
                    .Select(r => new RegistroPontoResponseDto
                    {
                        Id = r.Id,
                        FuncionarioId = r.FuncionarioId,
                        NomeFuncionario = r.Funcionario.Nome,
                        DataHoraEntrada = r.DataHoraEntrada,
                        DataHoraSaida = r.DataHoraSaida,
                        Duracao = r.DataHoraSaida.HasValue 
                            ? FormatDuration(r.DataHoraSaida.Value - r.DataHoraEntrada)
                            : null
                    })
                    .ToListAsync();

                _logger.LogInformation("Listagem de pontos realizada com sucesso. Total: {Count}", pontos.Count);
                return Ok(pontos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar registros de ponto");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Retorna um registro de ponto específico por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<RegistroPontoResponseDto>> GetPonto(int id)
        {
            try
            {
                var ponto = await _context.RegistrosPonto
                    .Include(r => r.Funcionario)
                    .Where(r => r.Id == id)
                    .Select(r => new RegistroPontoResponseDto
                    {
                        Id = r.Id,
                        FuncionarioId = r.FuncionarioId,
                        NomeFuncionario = r.Funcionario.Nome,
                        DataHoraEntrada = r.DataHoraEntrada,
                        DataHoraSaida = r.DataHoraSaida,
                        Duracao = r.DataHoraSaida.HasValue 
                            ? FormatDuration(r.DataHoraSaida.Value - r.DataHoraEntrada)
                            : null
                    })
                    .FirstOrDefaultAsync();

                if (ponto == null)
                {
                    _logger.LogWarning("Registro de ponto com ID {Id} não encontrado", id);
                    return NotFound($"Registro de ponto com ID {id} não encontrado");
                }

                _logger.LogInformation("Registro de ponto {Id} encontrado com sucesso", id);
                return Ok(ponto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar registro de ponto com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Cria um novo registro de ponto (entrada)
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<RegistroPontoResponseDto>> PostPonto(RegistroPontoCreateDto pontoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Verificar se o funcionário existe
                var funcionarioExiste = await _context.Funcionarios
                    .AnyAsync(f => f.Id == pontoDto.FuncionarioId);

                if (!funcionarioExiste)
                {
                    return BadRequest($"Funcionário com ID {pontoDto.FuncionarioId} não encontrado");
                }

                // Verificar se já existe um ponto em aberto para o funcionário na data
                var pontoEmAberto = await _context.RegistrosPonto
                    .Where(r => r.FuncionarioId == pontoDto.FuncionarioId 
                               && r.DataHoraEntrada.Date == pontoDto.DataHoraEntrada.Date
                               && r.DataHoraSaida == null)
                    .FirstOrDefaultAsync();

                if (pontoEmAberto != null)
                {
                    return BadRequest("Já existe um registro de ponto em aberto para este funcionário na data informada");
                }

                var registroPonto = new RegistroPonto
                {
                    FuncionarioId = pontoDto.FuncionarioId,
                    DataHoraEntrada = pontoDto.DataHoraEntrada,
                    DataHoraSaida = pontoDto.DataHoraSaida
                };

                _context.RegistrosPonto.Add(registroPonto);
                await _context.SaveChangesAsync();

                // Buscar o registro criado com o funcionário
                var pontoCompleto = await _context.RegistrosPonto
                    .Include(r => r.Funcionario)
                    .FirstAsync(r => r.Id == registroPonto.Id);

                var responseDto = new RegistroPontoResponseDto
                {
                    Id = pontoCompleto.Id,
                    FuncionarioId = pontoCompleto.FuncionarioId,
                    NomeFuncionario = pontoCompleto.Funcionario.Nome,
                    DataHoraEntrada = pontoCompleto.DataHoraEntrada,
                    DataHoraSaida = pontoCompleto.DataHoraSaida,
                    Duracao = pontoCompleto.DataHoraSaida.HasValue 
                        ? FormatDuration(pontoCompleto.DataHoraSaida.Value - pontoCompleto.DataHoraEntrada)
                        : null
                };

                _logger.LogInformation("Registro de ponto criado com sucesso. ID: {Id}, Funcionário: {FuncionarioId}", 
                    registroPonto.Id, registroPonto.FuncionarioId);
                
                return CreatedAtAction(nameof(GetPonto), new { id = registroPonto.Id }, responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar registro de ponto");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Registra a saída de um ponto
        /// </summary>
        [HttpPatch("{id}/saida")]
        public async Task<IActionResult> RegistrarSaida(int id, RegistroPontoSaidaDto saidaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var registroPonto = await _context.RegistrosPonto.FindAsync(id);
                if (registroPonto == null)
                {
                    _logger.LogWarning("Tentativa de registrar saída em ponto inexistente. ID: {Id}", id);
                    return NotFound($"Registro de ponto com ID {id} não encontrado");
                }

                if (registroPonto.DataHoraSaida.HasValue)
                {
                    return BadRequest("Este registro de ponto já possui horário de saída");
                }

                if (saidaDto.DataHoraSaida <= registroPonto.DataHoraEntrada)
                {
                    return BadRequest("O horário de saída deve ser posterior ao horário de entrada");
                }

                registroPonto.DataHoraSaida = saidaDto.DataHoraSaida;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Saída registrada com sucesso para o ponto {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao registrar saída do ponto com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Remove um registro de ponto
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePonto(int id)
        {
            try
            {
                var registroPonto = await _context.RegistrosPonto.FindAsync(id);
                if (registroPonto == null)
                {
                    _logger.LogWarning("Tentativa de deletar registro de ponto inexistente. ID: {Id}", id);
                    return NotFound($"Registro de ponto com ID {id} não encontrado");
                }

                _context.RegistrosPonto.Remove(registroPonto);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Registro de ponto {Id} removido com sucesso", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover registro de ponto com ID {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Registra entrada rápida (apenas data/hora atual)
        /// </summary>
        [HttpPost("entrada-rapida")]
        public async Task<ActionResult<RegistroPontoResponseDto>> RegistrarEntradaRapida([FromBody] int funcionarioId)
        {
            try
            {
                var pontoDto = new RegistroPontoCreateDto
                {
                    FuncionarioId = funcionarioId,
                    DataHoraEntrada = DateTime.Now
                };

                return await PostPonto(pontoDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao registrar entrada rápida para funcionário {FuncionarioId}", funcionarioId);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Registra saída rápida para o último ponto em aberto do funcionário
        /// </summary>
        [HttpPost("saida-rapida")]
        public async Task<IActionResult> RegistrarSaidaRapida([FromBody] int funcionarioId)
        {
            try
            {
                var pontoEmAberto = await _context.RegistrosPonto
                    .Where(r => r.FuncionarioId == funcionarioId && r.DataHoraSaida == null)
                    .OrderByDescending(r => r.DataHoraEntrada)
                    .FirstOrDefaultAsync();

                if (pontoEmAberto == null)
                {
                    return BadRequest("Não há registros de ponto em aberto para este funcionário");
                }

                var saidaDto = new RegistroPontoSaidaDto
                {
                    DataHoraSaida = DateTime.Now
                };

                return await RegistrarSaida(pontoEmAberto.Id, saidaDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao registrar saída rápida para funcionário {FuncionarioId}", funcionarioId);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        private static string FormatDuration(TimeSpan duration)
        {
            if (duration.TotalHours >= 1)
            {
                return $"{(int)duration.TotalHours}h {duration.Minutes}min";
            }
            return $"{duration.Minutes}min";
        }
    }
}
