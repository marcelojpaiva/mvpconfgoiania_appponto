using System.ComponentModel.DataAnnotations;

namespace ControlePonto.DTOs
{
    /// <summary>
    /// DTO para criação de registro de ponto
    /// </summary>
    public class RegistroPontoCreateDto
    {
        [Required(ErrorMessage = "FuncionarioId é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "FuncionarioId deve ser maior que 0")]
        public int FuncionarioId { get; set; }

        [Required(ErrorMessage = "Data e hora de entrada é obrigatória")]
        public DateTime DataHoraEntrada { get; set; }

        public DateTime? DataHoraSaida { get; set; }
    }

    /// <summary>
    /// DTO para retorno de registro de ponto
    /// </summary>
    public class RegistroPontoResponseDto
    {
        public int Id { get; set; }
        public int FuncionarioId { get; set; }
        public string NomeFuncionario { get; set; } = string.Empty;
        public DateTime DataHoraEntrada { get; set; }
        public DateTime? DataHoraSaida { get; set; }
        public string? Duracao { get; set; }
    }

    /// <summary>
    /// DTO para atualização de saída do ponto
    /// </summary>
    public class RegistroPontoSaidaDto
    {
        [Required(ErrorMessage = "Data e hora de saída é obrigatória")]
        public DateTime DataHoraSaida { get; set; }
    }
}
