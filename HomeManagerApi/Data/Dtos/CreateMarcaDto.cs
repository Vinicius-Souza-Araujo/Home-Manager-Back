using HomeManagerApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace HomeManagerApi.Data.Dtos
{
    public class CreateMarcaDto
    {
        [Required(ErrorMessage = "O nome da marca é obrigatório.")]
        [StringLength(50, ErrorMessage = "O tamanho do nome não pode exceder 50 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O grupo da marca é obrigatório.")]
        public Grupo Grupo { get; set; }
    }
}
