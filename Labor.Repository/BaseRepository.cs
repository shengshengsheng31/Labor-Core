using Labor.IRepository;
using Labor.Model;
using Labor.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Labor.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly LaborContext _context;
        public BaseRepository(LaborContext context)
        {
            _context = context;
        } 

        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="saved"></param>
        /// <returns></returns>
        public async Task CreateAsync(TEntity entity, bool saved=true)
        {
            _context.Set<TEntity>().Add(entity);
            if (saved) await SaveAsync();
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="saved"></param>
        /// <returns></returns>
        public async Task EditAsync(TEntity entity, bool saved=true)
        {
            _context.Entry(entity).State = EntityState.Modified;
            if (!_context.ChangeTracker.HasChanges()) return;
            if (saved) await SaveAsync();
        }

        /// <summary>
        /// 判断对象是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await GetAll().AnyAsync(m => m.Id==id);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().AsNoTracking();
        }

        /// <summary>
        /// 获取所有数据并排序
        /// </summary>
        /// <param name="asc"></param>
        /// <returns></returns>
        public IQueryable<TEntity> GetAllByOrder(bool asc = true)
        {
            var datas = GetAll();
            return datas = asc ?
                datas.OrderBy(m => m.UpdateTime) : datas.OrderByDescending(m => m.UpdateTime);
        }

        /// <summary>
        /// 获取数据分页
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public IQueryable<TEntity> GetAllByPage(int pageSize = 10, int pageNumber = 1)
        {
            return GetAll().Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        }

        /// <summary>
        /// 获取所有数据排序后分页
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        public IQueryable<TEntity> GetAllByPageOrder(int pageSize = 10, int pageNumber = 0, bool asc = true)
        {
            return GetAllByOrder(asc).Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        }

        /// <summary>
        /// 根据Id查询数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="saved"></param>
        /// <returns></returns>
        public async Task<TEntity> GetOneByIdAsync(Guid id)
        {
            return await GetAll().FirstAsync(m => m.Id == id);
        }

        /// <summary>
        /// 根据Id删除数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="saved"></param>
        /// <returns></returns>
        public async Task RemoveAsync(Guid id, bool saved=true)
        {
            var t = new TEntity() { Id = id };
            _context.Entry(t).State = EntityState.Deleted;
            if (saved) await SaveAsync();
        }

        /// <summary>
        /// 根据model删除数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="saved"></param>
        /// <returns></returns>
        public async Task RemoveAsync(TEntity entity, bool saved=true)
        {
            await RemoveAsync(entity.Id, saved);
        }

        /// <summary>
        /// 提交到数据库
        /// </summary>
        /// <returns></returns>
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
