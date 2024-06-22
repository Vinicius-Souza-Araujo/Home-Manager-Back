using HomeManagerApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace HomeManagerApi.Models
{
    public class Categoria
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
        [MaxLength(50, ErrorMessage = "O tamanho do nome não pode exceder 50 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O grupo da categoria é obrigatório.")]
        public Grupo Grupo { get; set; }

    }
}
