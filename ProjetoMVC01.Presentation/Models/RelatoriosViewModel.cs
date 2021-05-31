using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoMVC01.Presentation.Models
{
    public class RelatoriosViewModel
    {
        [Required(ErrorMessage = "Por favor, informe o tipo do relatório.")]
        public int? TipoRelatorio { get; set; }

        [Required(ErrorMessage = "Por favor, informe o formato do relatório.")]
        public int? FormatoRelatorio { get; set; }
    }
}
