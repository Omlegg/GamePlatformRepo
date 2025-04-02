using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamePlatformRepo.Core.Data;
using GamePlatformRepo.Models;
using GamePlatformRepo.Repository;
using Microsoft.EntityFrameworkCore;

namespace GamePlatformRepo.InfraStructure.Repositories
{
    public class GameEFRepository : IGameRepository
    {
        public readonly GamePlatformDbContext dbContext;

        public GameEFRepository(GamePlatformDbContext dbConext){
            this.dbContext = dbConext;
        }

        public async Task<int> Create(Game game)
        {
            await dbContext.AddAsync(game);
            await dbContext.SaveChangesAsync();
            return game.Id;
        }

        public async Task Delete(int id)
        {
            var entity = await dbContext.Games.FindAsync(id);
            if (entity != null)
            {
                dbContext.Games.Remove(entity);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<Game?> Get(int id)
        {
            var entity = await dbContext.Games.FindAsync(id);
            return entity;
        }

        public async Task<List<Game>?> GetAll()
        {
            return await dbContext.Games.ToListAsync();
        }

        public async Task Update(Game game)
        {
            var entity = await dbContext.Games.FindAsync(game.Id);
            dbContext.Games.Update(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}