using Labor.Model.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Labor.IRepository
{
    /// <summary>
    /// 基类接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBaseRepository<TEntity> where TEntity:BaseEntity
    {
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="saved"></param>
        /// <returns></returns>
        Task CreateAsync(TEntity entity, bool saved=true);

        /// <summary>
        /// 根据Id删除数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="saved"></param>
        /// <returns></returns>
        Task RemoveAsync(Guid id, bool saved = true);

        /// <summary>
        /// 根据model删除数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="saved"></param>
        /// <returns></returns>
        Task RemoveAsync(TEntity entity, bool saved = true);

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="saved"></param>
        /// <returns></returns>
        Task EditAsync(TEntity entity, bool saved = true);

        /// <summary>
        /// 根据Id查询数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="saved"></param>
        /// <returns></returns>
        Task<TEntity> GetOneByIdAsync(Guid id);

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// 获取所有数据并排序
        /// </summary>
        /// <param name="asc"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetAllByOrder(bool asc = false);

        /// <summary>
        /// 获取数据分页
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetAllByPage(int pageSize = 10, int pageNumber = 1);

        /// <summary>
        /// 获取所有数据排序后分页
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="asc"></param>
        /// <returns></returns>
        IQueryable<TEntity> GetAllByPageOrder(int pageSize = 10, int pageNumber = 1, bool asc = false);

        /// <summary>
        /// 判断对象是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(Guid id);

        /// <summary>
        /// 提交到数据库
        /// </summary>
        /// <returns></returns>
        Task SaveAsync();
    }
}
