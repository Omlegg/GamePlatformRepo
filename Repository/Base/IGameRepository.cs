using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamePlatformRepo.Models;

namespace GamePlatformRepo.Repository
{
    public interface IGameRepository
    {
        Task<int> Create(Game game);
        Task<Game?> Get(int id);

        Task<List<Game>?> GetAll();

        Task Delete(int id);
        Task Update(Game game);
    }
}