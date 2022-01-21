using System;
using System.ComponentModel.DataAnnotations;

namespace Investimentos.Models
{
    public class Historico
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Negociação")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} tem que estar entre {2} e {1} caracteres!")]
        public string Negociacao { get; set; }
        
        [Display(Name = "C/V")]
        public char C_V { get; set; }

        [Display(Name = "Tipo de Mercado")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} tem que estar entre {2} e {1} caracteres!")]
        public string TipoMercado { get; set; }
        
        public Papel Papel { get; set; }
        
        [Display(Name = "Papel")]
        public int PapelId { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [Range(1, 50000, ErrorMessage = "{0} tem que estar entre {1} e {2}!")]
        public int Quantidade { get; set; }

        [Display(Name = "Preço")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        [Range(0, 50000, ErrorMessage = "{0} tem que estar entre {1:F2} e {2:F2}!")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double Preco { get; set; }

        [Display(Name = "Valor da Operação")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        [Range(0, 50000, ErrorMessage = "{0} tem que estar entre {1:F2} e {2:F2}!")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double ValorOperacao { get; set; }

        [Display(Name = "Data da Operação")]
        [Required(ErrorMessage = "Preenchimento obrigatório!")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime DataOperacao { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "{0} tem que estar entre {2} e {1} caracteres!")]
        public string Obs { get; set; }

        public Historico() { }

        public Historico(int id, string negociacao, char c_V, string tipoMercado, int papelId, int quantidade, double preco, double valorOperacao, DateTime dataOperacao, string obs)
        {
            Id = id;
            Negociacao = negociacao;
            C_V = c_V;
            TipoMercado = tipoMercado;
            PapelId = papelId;
            Quantidade = quantidade;
            Preco = preco;
            ValorOperacao = valorOperacao;
            DataOperacao = dataOperacao;
            Obs = obs;
        }
    }
}
