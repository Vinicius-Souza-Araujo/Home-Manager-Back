using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeManagerApi.Models
{
    public class Patrimonio
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do patrimônio é obrigatório.")]
        [MaxLength(50, ErrorMessage = "O tamanho do nome não pode exceder 50 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O preço do do patrimônio é obrigatório.")]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal  Preco { get; set; }

        [Required(ErrorMessage = "A quantidade do patrimônio é obrigatória.")]
        public int Quantidade { get; set; }

        [Required(ErrorMessage = "A categoria é obrigatória.")]
        [ForeignKey("Categoria")]
        public int FK_Categoria { get; set; }
        public Categoria Categoria { get; set; }

        [Required(ErrorMessage = "A marca é obrigatória.")]
        [ForeignKey("Marca")]
        public int FK_Marca { get; set; }
        public Marca Marca { get; set; }

    }
}
