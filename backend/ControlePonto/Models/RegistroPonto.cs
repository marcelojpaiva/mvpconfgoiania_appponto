using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlePonto.Models
{
    /// <summary>
    /// Modelo de dados para representar um registro de ponto
    /// </summary>
    public class RegistroPonto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "FuncionarioId é obrigatório")]
        public int FuncionarioId { get; set; }

        [Required(ErrorMessage = "Data de entrada é obrigatória")]
        public DateTime DataHoraEntrada { get; set; }

        public DateTime? DataHoraSaida { get; set; }

        // Propriedade calculada para duração do trabalho
        [NotMapped]
        public TimeSpan? Duracao => DataHoraSaida.HasValue 
            ? DataHoraSaida.Value - DataHoraEntrada 
            : null;

        // Propriedade de navegação para o funcionário
        [ForeignKey("FuncionarioId")]
        public virtual Funcionario Funcionario { get; set; } = null!;
    }
}
