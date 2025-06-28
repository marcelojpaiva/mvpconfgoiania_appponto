using System.ComponentModel.DataAnnotations;

namespace ControlePonto.Models
{
    /// <summary>
    /// Modelo de dados para representar um funcionário
    /// </summary>
    public class Funcionario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail deve ter um formato válido")]
        [StringLength(150, ErrorMessage = "E-mail deve ter no máximo 150 caracteres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cargo é obrigatório")]
        [StringLength(80, ErrorMessage = "Cargo deve ter no máximo 80 caracteres")]
        public string Cargo { get; set; } = string.Empty;

        // Propriedade de navegação para os pontos registrados
        public virtual ICollection<RegistroPonto> RegistrosPonto { get; set; } = new List<RegistroPonto>();
    }
}
