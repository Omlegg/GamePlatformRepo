using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using GamePlatformRepo.Models;
using GamePlatformRepo.Repository.Base;

namespace GamePlatformRepo.Repository
{
    public class CommentDapperRepository : ICommentRepository
    {
        private SqlConnection connection = new SqlConnection("Server=localhost;Database=GamePlatformDb;Integrated Security=true;");

        public async Task<int> Create(Comment comment){
            connection.Open();
            string sql = "INSERT INTO COMMENTS (Author, Title, Description,GameId) VALUES (@Author, @Title, @Description, @GameId)";
            await connection.ExecuteAsync(sql: sql,
                param: new
                {
                    Description = comment.Description,
                    GameId = comment.GameId,
                    Title = comment.Title,
                    Author = comment.Author,
                });
            var id = connection.ExecuteScalar<int>("SELECT SCOPE_IDENTITY()");
            connection.Close();
            return id;
        }

        public async Task Delete(int id)
        {
             using (var conn = new SqlConnection("Server=localhost;Database=GamePlatformDb;Integrated Security=true;")) // Ensure a fresh connection
            {
                await conn.OpenAsync(); // Use OpenAsync() for async operations
                string sql = "DELETE FROM Comments WHERE Id = @Id";
                await conn.ExecuteAsync(sql, new { Id = id });
            } 
        }

        public async Task<List<Comment>?> GetAll(int id)
        {
            connection.Open();

            var comments = await connection.QueryAsync<Comment>(
                sql: $@"select *
                    from Comments
                    where GameId = @Id
                    "
                ,param: new
                {
                    Id = id
                });

            connection.Close();
            return comments.ToList();
        }

        public async Task<Comment?> Get(int id)
        {
            connection.Open();

            var comment = await connection.QueryFirstAsync<Comment>(
                sql: $@"select *
                    from Comments
                    where Id = @Id",
                param: new
                {
                    Id = id,
                }
                );

            connection.Close();
            return comment;
        }

        public async Task Update(Comment comment)
        {
            connection.Open();

            await connection.ExecuteAsync(
                sql: @"update Comments
                set Author = @Author, 
                Description = @Description,
                Title = @Title,
                GameId = @GameId
                where Id = @Id",
                param: new
                {
                    Description = comment.Description,
                    Title = comment.Title,
                    Id = comment.Id,
                    GameId = comment.GameId,
                    Author = comment.Author,
                });
            connection.Close();
        }
    }
}