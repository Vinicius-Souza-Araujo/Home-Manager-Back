using HomeManagerApi.Enums;

namespace HomeManagerApi.Data.Dtos
{
    public class ReadMarcaDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public Grupo Grupo { get; set; }
    }
}
