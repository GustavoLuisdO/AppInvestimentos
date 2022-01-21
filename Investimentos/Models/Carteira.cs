using System.ComponentModel.DataAnnotations;

namespace Investimentos.Models
{
    public class Carteira
    {
        [Key]
        public int Id { get; set; }
        public Papel Papel { get; set; }

        [Display(Name = "Papel")]
        public int PapelId { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [Range(1, 50000, ErrorMessage = "{0} tem que estar entre {1} e {2}!")]
        public int Quantidade { get; set; }

        [Display(Name = "Preço Médio")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        [Range(0, 50000, ErrorMessage = "{0} tem que estar entre {1:F2} e {2:F2}!")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double PrecoMedio { get; set; }

        public Carteira() { }

        public Carteira(int id, int papelId, int quantidade, double precoMedio)
        {
            Id = id;
            PapelId = papelId;
            Quantidade = quantidade;
            PrecoMedio = precoMedio;
        }
    }
}