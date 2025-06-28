using System.ComponentModel.DataAnnotations;

namespace ControlePonto.DTOs
{
    /// <summary>
    /// DTO para criação de funcionário
    /// </summary>
    public class FuncionarioCreateDto
    {
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
    }

    /// <summary>
    /// DTO para retorno de funcionário
    /// </summary>
    public class FuncionarioResponseDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
    }
}
