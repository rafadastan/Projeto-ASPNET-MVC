using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoMVC01.Repository.Interfaces
{
    public interface IBaseRepository<TEntity, TKey>
        where TEntity : class
        where TKey : struct
    {
        void Create(TEntity obj);
        void Update(TEntity obj);
        void Delete(TEntity obj);

        List<TEntity> GetAll();
        TEntity GetById(TKey id);
    }
}
