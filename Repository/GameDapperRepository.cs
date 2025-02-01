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

        public int CreateGame(Game game){
            connection.Open();
            string sql = "INSERT INTO Games (Name, Price, Description,Views) VALUES (@Name, @Price, @Description, @Views)";
            connection.Execute(sql: sql,
                param: new
                {
                    Description = game.Description,
                    Views = game.Views,
                    Price = game.Price,
                    Name = game.Name,
                });
            var id = connection.ExecuteScalar<int>("SELECT SCOPE_IDENTITY()");
            connection.Close();
            return id;
        }

        public void DeleteGame(int id)
        {
            connection.Open();
            string sql = "DELETE FROM Games WHERE Id = @Id";
            connection.Execute(sql, new { Id = id });
            connection.Close();
        }

        public Game? GetGame(int id)
        {
            connection.Open();

            var game = connection.QueryFirst<Game>(
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

        public void UpdateGame(Game game)
        {
            connection.Open();

            connection.Execute(
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