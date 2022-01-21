using System.ComponentModel.DataAnnotations;

namespace Investimentos.Models
{
    public class Papel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "{0} tem que estar entre {2} e {1} caracteres!")]
        public string Nome { get; set; }

        [Display(Name = "Tipo de Papel")]
        public TipoPapel TipoPapel { get; set; }

        [Display(Name = "Tipo de Papel")]
        public int TipoPapelId { get; set; }

        public Papel() { }

        public Papel(int id, string nome, int tipoPapelId)
        {
            Id = id;
            Nome = nome;
            TipoPapelId = tipoPapelId;
        }
    }
}
