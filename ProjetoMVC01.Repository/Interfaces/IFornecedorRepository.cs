using ProjetoMVC01.Domain.DTOs;
using ProjetoMVC01.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoMVC01.Repository.Interfaces
{
    public interface IFornecedorRepository
        : IBaseRepository<Fornecedor, Guid>
    {
        List<Fornecedor> GetByNome(string nome);
        List<QtdProdutosFornecedorDTO> GroupByQtdProdutos();
    }
}
