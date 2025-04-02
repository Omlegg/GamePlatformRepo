using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamePlatformRepo.Core.Data;
using GamePlatformRepo.Models;
using GamePlatformRepo.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace GamePlatformRepo.InfraStructure.Repositories
{
    public class CommentEFRepository : ICommentRepository
    {

        private readonly GamePlatformDbContext dbContext;

        public CommentEFRepository(GamePlatformDbContext dbConext){
            this.dbContext = dbConext;
        }


        public async Task<int> Create(Comment comment)
        {
            await dbContext.AddAsync(comment);
            await dbContext.SaveChangesAsync();
            return comment.Id;
        }

        public async Task Delete(int id)
        {
            var entity = await dbContext.Comments.FindAsync(id);
            dbContext.Comments.Remove(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Comment?> Get(int id)
        {
            var entity = await dbContext.Comments.FindAsync(id);
            return entity;
        }

        public async Task<List<Comment>?> GetAll(int id)
        {
            return await dbContext.Comments.Where<Comment>(p=> p.GameId == id).ToListAsync();
        }

        public async Task Update(Comment comment)
        {
            var entity = await dbContext.Comments.FindAsync(comment.Id);
            dbContext.Comments.Update(entity);
            await dbContext.SaveChangesAsync();
        }
    }
}