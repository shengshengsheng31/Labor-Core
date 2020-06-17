using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labor.Model.Helpers
{
    public class PageInfoHelper<T>:List<T>
    {
        //当前页码
        public int CurrentPage { get; }
        //每页数量
        public int PageSize { get; }
        //结果数量
        public int TotalCount { get; }


        //初始化翻页信息
        public PageInfoHelper(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            AddRange(items);
        }

        //创建分页信息
        public static async Task<PageInfoHelper<T>> CreatePageMsgAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            int count = await source.CountAsync();//总数据量
            List<T> list = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PageInfoHelper<T>(list, count, pageNumber, pageSize);
        }
    }
}
