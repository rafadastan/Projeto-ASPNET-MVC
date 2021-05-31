using Dapper;
using ProjetoMVC01.Domain.Entities;
using ProjetoMVC01.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ProjetoMVC01.Repository.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly string _connectionString;

        public ProdutoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Create(Produto obj)
        {
            var query = @"
                    INSERT INTO PRODUTO(IDPRODUTO, NOME, DESCRICAO, FOTO, PRECO, QUANTIDADE, IDFORNECEDOR)
                    VALUES(NEWID(), @Nome, @Descricao, @Foto, @Preco, @Quantidade, @IdFornecedor)
                ";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(query, obj);
            }
        }

        public void Update(Produto obj)
        {
            var query = @"
                    UPDATE PRODUTO 
                    SET
                        NOME = @Nome, 
                        DESCRICAO = @Descricao, 
                        FOTO = @Foto, 
                        PRECO = @Preco, 
                        QUANTIDADE = @Quantidade,
                        IDFORNECEDOR = @IdFornecedor
                    WHERE
                        IDPRODUTO = @IdProduto
                ";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(query, obj);
            }
        }

        public void Delete(Produto obj)
        {
            var query = @"
                    DELETE FROM PRODUTO 
                    WHERE IDPRODUTO = @IdProduto
                ";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(query, obj);
            }
        }

        public List<Produto> GetAll()
        {
            var query = @"
                    SELECT * FROM PRODUTO P
                    INNER JOIN FORNECEDOR F
                    ON F.IDFORNECEDOR = P.IDFORNECEDOR
                    ORDER BY P.NOME
                ";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection
                        .Query(query, (Produto p, Fornecedor f) => 
                        {
                            p.Fornecedor = f;
                            return p;
                        }, 
                        splitOn: "IdFornecedor")
                        .ToList();
            }
        }

        public Produto GetById(Guid id)
        {
            var query = @"
                    SELECT * FROM PRODUTO P
                    INNER JOIN FORNECEDOR F
                    ON F.IDFORNECEDOR = P.IDFORNECEDOR
                    WHERE P.IDPRODUTO = @id
                ";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection
                        .Query(query, (Produto p, Fornecedor f) =>
                        {
                            p.Fornecedor = f;
                            return p;
                        },
                        new { id }, 
                        splitOn: "IdFornecedor")
                        .FirstOrDefault();
            }
        }

        public List<Produto> GetByNome(string nome)
        {
            var query = @"
                    SELECT * FROM PRODUTO P
                    INNER JOIN FORNECEDOR F
                    ON F.IDFORNECEDOR = P.IDFORNECEDOR
                    WHERE P.NOME LIKE @nome
                    ORDER BY P.NOME
                ";

            nome = $"%{nome}%";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection
                    .Query(query, (Produto p, Fornecedor f) =>
                    {
                        p.Fornecedor = f;
                        return p;
                    },
                    new { nome },
                    splitOn: "IdFornecedor")
                    .ToList();
            }
        }
    }
}
