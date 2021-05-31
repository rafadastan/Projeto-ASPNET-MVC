using Dapper;
using ProjetoMVC01.Domain.DTOs;
using ProjetoMVC01.Domain.Entities;
using ProjetoMVC01.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ProjetoMVC01.Repository.Repositories
{
    public class FornecedorRepository : IFornecedorRepository
    {
        private readonly string _connectionString;

        public FornecedorRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Create(Fornecedor obj)
        {
            var query = @"
                    INSERT INTO FORNECEDOR(IDFORNECEDOR, NOME, CNPJ)
                    VALUES(NEWID(), @Nome, @Cnpj)
                ";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(query, obj);
            }
        }

        public void Update(Fornecedor obj)
        {
            var query = @"
                    UPDATE FORNECEDOR 
                    SET 
                        NOME = @Nome, 
                        CNPJ = @Cnpj
                    WHERE 
                        IDFORNECEDOR = @IdFornecedor
                ";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(query, obj);
            }
        }

        public void Delete(Fornecedor obj)
        {
            var query = @"
                    DELETE FROM FORNECEDOR
                    WHERE IDFORNECEDOR = @IdFornecedor
                ";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(query, obj);
            }
        }

        public List<Fornecedor> GetAll()
        {
            var query = @"
                    SELECT * FROM FORNECEDOR
                    ORDER BY NOME
                ";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection
                    .Query<Fornecedor>(query)
                    .ToList();
            }
        }

        public Fornecedor GetById(Guid id)
        {
            var query = @"
                    SELECT * FROM FORNECEDOR
                    WHERE IDFORNECEDOR = @id   
                ";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection
                    .Query<Fornecedor>(query, new { id })
                    .FirstOrDefault();
            }
        }

        public List<Fornecedor> GetByNome(string nome)
        {
            var query = @"
                    SELECT * FROM FORNECEDOR
                    WHERE NOME LIKE @nome
                    ORDER BY NOME
                ";

            //adicionando '%' ao inicio e final do nome..
            //Exemplo: '%Loja%' -> contendo o nome informado..
            nome = $"%{nome}%";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection
                    .Query<Fornecedor>(query, new { nome })
                    .ToList();
            }
        }

        public List<QtdProdutosFornecedorDTO> GroupByQtdProdutos()
        {
            var query = @"
                    SELECT
	                    F.NOME AS FORNECEDOR,
	                    SUM(P.QUANTIDADE) AS QUANTIDADEPRODUTOS
                    FROM FORNECEDOR F
                    INNER JOIN PRODUTO P
                    ON F.IDFORNECEDOR = P.IDFORNECEDOR
                    GROUP BY F.NOME
                ";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<QtdProdutosFornecedorDTO>(query).ToList();
            }
        }
    }
}
