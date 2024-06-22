using HomeManagerApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace HomeManagerApi.Data.Dtos
{
    public class ReadCategoriaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public Grupo Grupo { get; set; }
    }
}
