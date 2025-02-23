using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using GamePlatformRepo.Models;
using GamePlatformRepo.Repository;

namespace GamePlatformRepo.Repositories;
public class GameJSONRepository : IGameRepository
{
    private readonly string filePath = "games.json";

    public GameJSONRepository(){
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public int CreateGame(Game game)
    {
        var games = GetAllGames();
        var newId = 0;
        if(games != null){
            newId = games.ToArray().Length-1;
        }else{
            games = new List<Game>();

        }
        games.Add(game);
        var SerializedProducts = JsonSerializer.Serialize(games);
        File.WriteAllText(filePath, SerializedProducts);
        return newId;
    }

    public void DeleteGame(int id)
    {
        var games = GetAllGames();
        if(games == null){
            games = new List<Game>();

        }
        var gameToDelete = games.FirstOrDefault(p => p.Id == id);
        if (gameToDelete != null)
        {
            games.Remove(gameToDelete);
        }
        var SerializedProducts = JsonSerializer.Serialize(games);
        File.WriteAllText(filePath, SerializedProducts);
    }

    private List<Game>? GetAllGames()
    {
        var GamesJSON = File.ReadAllText(filePath);
        var games = JsonSerializer.Deserialize<List<Game>>(GamesJSON);
        return games;
    }

    public Game? GetGame(int id)
    {
        var games = GetAllGames();
        if(games == null){
            return null;
        }

        var game = games.FirstOrDefault(p => p.Id == id);
        return game;
    }


    public void UpdateGame(Game game)
    {
        var games = GetAllGames();
        if(games == null){
            games = new List<Game>();
        }
        var gameToUpdate = games.FirstOrDefault(p => p.Id == game.Id);
        if (gameToUpdate != null)
        {
            gameToUpdate.Name = game.Name;
            gameToUpdate.Description = game.Description;
            gameToUpdate.Price = game.Price;
            gameToUpdate.Views = game.Views;
        }

        var SerializedProducts = JsonSerializer.Serialize(games);
        File.WriteAllText(filePath, SerializedProducts);
    }
}