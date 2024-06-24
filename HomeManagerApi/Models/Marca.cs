using HomeManagerApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace HomeManagerApi.Models
{

    public class Marca
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da marca é obrigatório.")]
        [MaxLength(50, ErrorMessage = "O tamanho do nome não pode exceder 50 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O grupo da marca é obrigatório.")]
        public Grupo grupo { get; set; }
    }
}
