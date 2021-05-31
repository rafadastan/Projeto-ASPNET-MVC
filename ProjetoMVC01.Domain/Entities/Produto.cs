using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoMVC01.Domain.Entities
{
    public class Produto
    {
        public Guid IdProduto { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Foto { get; set; }
        public decimal Preco { get; set; }
        public int Quantidade { get; set; }
        public Guid IdFornecedor { get; set; }

        #region Associações

        public Fornecedor Fornecedor { get; set; }

        #endregion
    }
}
