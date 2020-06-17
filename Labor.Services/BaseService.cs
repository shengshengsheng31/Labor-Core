using Labor.IRepository;
using Labor.IServices;
using Labor.Model.Models;
using Labor.Model.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Labor.Services
{
    public class BaseService<TEntity>:IBaseService<TEntity> where TEntity:BaseEntity
    {
        public IBaseRepository<TEntity> BaseRepository;

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="saved"></param>
        /// <returns></returns>
        public async Task CreateAsync(TEntity entity, bool saved=true)
        {
            await BaseRepository.CreateAsync(entity, saved);
        }

        public async Task EditAsync(TEntity entity, bool saved=true)
        {
            await BaseRepository.EditAsync(entity, saved);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await BaseRepository.ExistsAsync(id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return BaseRepository.GetAll();
        }

        public IQueryable<TEntity> GetAllByOrder(bool asc = false )
        {
            return BaseRepository.GetAllByOrder(asc);
        }

        public IQueryable<TEntity> GetAllByPage(int pageSize = 10, int pageNumber = 1)
        {
            return BaseRepository.GetAllByPage(pageSize, pageNumber);
        }

        public IQueryable<TEntity> GetAllByPageOrder(PageViewModel model, bool asc = false )
        {
            return BaseRepository.GetAllByPageOrder(model.PageSize, model.PageNumber, asc);
        }

        public Task<TEntity> GetOneByIdAsync(Guid id)
        {
            return BaseRepository.GetOneByIdAsync(id);
        }

        public Task RemoveAsync(Guid id, bool saved=true)
        {
            return BaseRepository.RemoveAsync(id, saved);
        }

        public Task RmoveAsync(TEntity entity, bool saved=true)
        {
            return BaseRepository.RemoveAsync(entity, saved);
        }

        public Task SaveAsync()
        {
            return BaseRepository.SaveAsync();
        }
    }
}
