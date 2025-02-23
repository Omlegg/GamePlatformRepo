using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamePlatformRepo.Models;

namespace GamePlatformRepo.Repository
{
    public interface IGameRepository
    {
        int CreateGame(Game game);
        Game? GetGame(int id);
        void DeleteGame(int id);
        void UpdateGame(Game game);
    }
}