using ProjetoMVC01.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoMVC01.Repository.Interfaces
{
    public interface IProdutoRepository
        : IBaseRepository<Produto, Guid>
    {
        List<Produto> GetByNome(string nome);
    }
}
