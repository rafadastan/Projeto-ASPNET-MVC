using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; //validações

namespace ProjetoMVC01.Presentation.Models
{
    public class FornecedorCadastroViewModel
    {
        [MinLength(6, ErrorMessage = "Por favor, informe no mínimo {1} caracteres.")]
        [MaxLength(150, ErrorMessage = "Por favor, informe no máximo {1} caracteres.")]
        [Required(ErrorMessage = "Por favor, informe o nome do fornecedor.")]
        public string Nome { get; set; }

        [RegularExpression("^[0-9]{14}$", ErrorMessage = "Por favor, informe exatamente 14 digitos numéricos.")]
        [Required(ErrorMessage = "Por favor, informe o cnpj do fornecedor.")]
        public string Cnpj { get; set; }
    }
}
