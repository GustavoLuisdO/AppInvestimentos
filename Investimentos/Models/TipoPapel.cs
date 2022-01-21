using System.ComponentModel.DataAnnotations;

namespace Investimentos.Models
{
    public class TipoPapel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} tem que estar entre {2} e {1} caracteres!")]
        public string Tipo { get; set; }

        public TipoPapel() { }

        public TipoPapel(int id, string tipo)
        {
            Id = id;
            Tipo = tipo;
        }
    }
}
