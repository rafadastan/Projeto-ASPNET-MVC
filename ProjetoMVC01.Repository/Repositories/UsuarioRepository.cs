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
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Create(Usuario obj)
        {
            var query = @"
                    INSERT INTO USUARIO(
                        IDUSUARIO, 
                        NOME, 
                        EMAIL, 
                        SENHA, 
                        DATACADASTRO, 
                        HABILITADO) 
                    VALUES(
                        NEWID(), 
                        @Nome, 
                        @Email, 
                        CONVERT(VARCHAR(32), HASHBYTES('MD5', @Senha), 2), 
                        GETDATE(), 1)
                ";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(query, obj);
            }
        }

        public void Update(Usuario obj)
        {
            var query = @"
                    UPDATE USUARIO SET 
                        NOME = @Nome, 
                        EMAIL = @Email, 
                        SENHA = CONVERT(VARCHAR(32), HASHBYTES('MD5', @Senha), 2), 
                        HABILITADO = @Habilitado
                    WHERE 
                        IDUSUARIO = @IdUsuario
                ";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(query, obj);
            }
        }

        public void Delete(Usuario obj)
        {
            var query = @"
                    DELETE FROM USUARIO WHERE IDUSUARIO = @IDUSUARIO
                ";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(query, obj);
            }
        }

        public List<Usuario> GetAll()
        {
            var query = @"
                    SELECT * FROM USUARIO ORDER BY NOME
                ";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection
                    .Query<Usuario>(query)
                    .ToList();
            }
        }

        public Usuario GetById(Guid id)
        {
            var query = @"
                    SELECT * FROM USUARIO WHERE IDUSUARIO = @id
                ";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection
                        .Query<Usuario>(query, new { id })
                        .FirstOrDefault();
            }
        }

        public Usuario GetByEmail(string email)
        {
            var query = @"
                    SELECT * FROM USUARIO WHERE EMAIL = @email
                ";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection
                        .Query<Usuario>(query, new { email })
                        .FirstOrDefault();
            }
        }

        public Usuario GetByEmailAndSenha(string email, string senha)
        {
            var query = @"
                    SELECT * FROM USUARIO 
                    WHERE 
                        EMAIL = @email 
                        AND 
                        SENHA = CONVERT(VARCHAR(32), HASHBYTES('MD5', @Senha), 2)
                ";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection
                        .Query<Usuario>(query, new { email, senha })
                        .FirstOrDefault();
            }
        }
    }
}
