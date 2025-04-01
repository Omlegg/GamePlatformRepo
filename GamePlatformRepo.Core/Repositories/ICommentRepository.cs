using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamePlatformRepo.Models;

namespace GamePlatformRepo.Repository.Base
{
    public interface ICommentRepository
    {
        Task<int> Create(Comment coment);
        Task<Comment?> Get(int id);

        Task<List<Comment>?> GetAll(int id);

        Task Delete(int id);
        Task Update(Comment comment);
    }
}