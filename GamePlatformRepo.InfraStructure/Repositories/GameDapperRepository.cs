using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
﻿using System.Data.SqlClient;
using Dapper;
using GamePlatformRepo.Repository;
using GamePlatformRepo.Models;


namespace GamePlatformRepo.Repository
{
    public class GameDapperRepository : IGameRepository
    {
        private SqlConnection connection = new SqlConnection("Server=localhost;Database=GamePlatformDb;Integrated Security=true;");

        public async Task<int> Create(Game game){
            connection.Open();
            
            string sql = "INSERT INTO Games (Name, Price, Description,Views, Creator, DateOfPublication) VALUES (@Name, @Price, @Description, @Views,  @Creator, @DateOfPublication)";
            await connection.ExecuteAsync(sql: sql,
                param: new
                {
                    Description = game.Description,
                    Views = game.Views,
                    Price = game.Price,
                    Name = game.Name,
                    Creator = game.Creator,
                    DateOfPublication = game.DateOfPublication,
                });
            var id = connection.ExecuteScalar<int>("SELECT SCOPE_IDENTITY()");
            connection.Close();
            return id;
        }

        public async Task Delete(int id)
        {
             using (var conn = new SqlConnection("Server=localhost;Database=GamePlatformDb;Integrated Security=true;")) 
            {
                await conn.OpenAsync();
                string sql = "DELETE FROM Comments WHERE GameId = @Id";
                await conn.ExecuteAsync(sql, new { Id = id });
                sql = "DELETE FROM Games WHERE Id = @Id";
                await conn.ExecuteAsync(sql, new { Id = id });
            }
        }

        public async Task<List<Game>?> GetAll()
        {
            connection.Open();

            var games = await connection.QueryAsync<Game>(
                sql: $@"select *
                    from Games"
                );

            connection.Close();
            return games.ToList();
        }

        public async Task<Game?> Get(int id)
        {
            connection.Open();

            var game = await connection.QueryFirstAsync<Game>(
                sql: $@"select *
                    from Games
                    where Id = @Id",
                param: new
                {
                    Id = id,
                }
                );

            connection.Close();
            return game;
        }

        public async Task Update(Game game)
        {
            connection.Open();

            await connection.ExecuteAsync(
                sql: @"update Games
                set Name = @Name, 
                Description = @Description,
                Price = @Price,
                Views = @Views
                where Id = @Id",
                param: new
                {
                    Description = game.Description,
                    Views = game.Views,
                    Id = game.Id,
                    Price = game.Price,
                    Name = game.Name,
                });
            connection.Close();
        }
    }
}