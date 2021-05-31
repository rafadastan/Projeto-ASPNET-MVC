using ProjetoMVC01.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoMVC01.Presentation.Models
{
    public class FornecedorConsultaViewModel
    {
        [MinLength(3, ErrorMessage = "Por favor, informe no mínimo {1} caracteres.")]
        [Required(ErrorMessage = "Por favor, informe todo ou parte do nome do fornecedor que deseja consultar.")]
        public string Nome { get; set; }

        //propriedade para exibir na página a listagem de fornecedores
        //obtida a partir da consulta feita no banco de dados
        public List<Fornecedor> Fornecedores { get; set; }
    }
}
