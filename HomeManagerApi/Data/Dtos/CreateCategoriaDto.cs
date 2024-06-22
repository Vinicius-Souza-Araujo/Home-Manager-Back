using HomeManagerApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace HomeManagerApi.Data.Dtos
{
    public class CreateCategoriaDto
    {
        [Required(ErrorMessage = "O nome da categoria é obrigatório.")]
        [StringLength(50, ErrorMessage = "O tamanho do nome não pode exceder 50 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O grupo da categoria é obrigatório.")]
        public Grupo Grupo { get; set; }
    }
}
