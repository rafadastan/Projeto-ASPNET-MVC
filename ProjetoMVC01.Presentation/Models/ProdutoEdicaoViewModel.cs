using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoMVC01.Presentation.Models
{
    public class ProdutoEdicaoViewModel
    {
        public Guid IdProduto { get; set; } //campo oculto..

        [MinLength(6, ErrorMessage = "Por favor, informe o mínimo {1} caractere.")]
        [MaxLength(150, ErrorMessage = "Por favor, informe no máximo {1} caracteres.")]
        [Required(ErrorMessage = "Por favor, informe o nome do produto.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Por favor, informe o preço do produto.")]
        public decimal? Preco { get; set; }

        [Required(ErrorMessage = "Por favor, informe a quantidade do produto.")]
        public int? Quantidade { get; set; }

        [Required(ErrorMessage = "Por favor, informe a descrição do produto.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Por favor, selecione o fornecedor.")]
        public Guid? IdFornecedor { get; set; }

        public List<SelectListItem> ListagemFornecedores { get; set; }
    }
}
