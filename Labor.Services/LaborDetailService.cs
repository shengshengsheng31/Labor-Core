using Labor.IRepository;
using Labor.IServices;
using Labor.Model;
using Labor.Model.Helpers;
using Labor.Model.Models;
using Labor.Model.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Labor.Services
{
    public class LaborDetailService : BaseService<LaborDetail>, ILaborDetailService
    {
        private readonly ILaborDetailRepository _laborDetailRepository;
        public LaborDetailService(ILaborDetailRepository laborDetailRepository)
        {
            _laborDetailRepository = laborDetailRepository;
            BaseRepository = laborDetailRepository;
        }

        /// <summary>
        /// 创建一个人的劳保
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> CreateLaborDetailAsync(CreateLaborDetailViewModel model)
        {
            LaborDetail existLaborDetail = await _laborDetailRepository.GetAll().FirstOrDefaultAsync(m => m.UserId == model.UserId && m.LaborId == model.LaborId);
            if (existLaborDetail != null)
            {
                existLaborDetail.Option = model.Option;
                existLaborDetail.Goods = model.Goods;
                await _laborDetailRepository.EditAsync(existLaborDetail);
            }
            else
            {
                await _laborDetailRepository.CreateAsync(new LaborDetail
                {
                    UserId = model.UserId,
                    LaborId = model.LaborId,
                    Option = model.Option,
                    Goods = model.Goods
                });
            }
            return true;

        }

        /// <summary>
        /// 获取劳保通过LaborId分页
        /// </summary>
        /// <returns></returns>
        public async Task<PageInfoHelper<LaborDetailListViewModel>> GetAllByHeadPage(GetLaborDetailViewModel model)
        {
            IQueryable<LaborDetailListViewModel> result = _laborDetailRepository.GetAllByHead(model);
            var a = await PageInfoHelper<LaborDetailListViewModel>.CreatePageMsgAsync(result, model.PageNumber, model.PageSize);
            return a;
        }

        /// <summary>
        /// 获取劳保通过Id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IQueryable<LaborDetailListViewModel> GetAllByHead(GetLaborDetailViewModel model)
        {
            return _laborDetailRepository.GetAllByHead(model);
        }

        /// <summary>
        /// 通过用户Id和LaborHead Id 来获取一条已选择的劳保
        /// </summary>
        /// <returns></returns>
        public async Task<LaborDetail> GetLaborDetailByUserAndLaborAsync(OneLaborDetailViewModel model)
        {
            return await _laborDetailRepository.GetAll().Where(m => m.UserId == model.UserId && m.LaborId == model.LaborId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 通过劳保导出这期所有人的劳保excel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public byte[] ExlExport(GetLaborDetailViewModel model)
        {

            var list = GetAllByHead(model).Select(m => new
            {
                m.Account,
                m.Department,
                m.Option,
                m.Goods
            }).ToList();
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("sheet");
            IRow titleRow = sheet.CreateRow(0);
            IRow rows = null;
            //获取访问属性
            Type entityType = list[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();

            for (int i = 0; i <= list.Count; i++)
            {
                if (i == 0)
                {
                    //标题行
                    titleRow.CreateCell(0).SetCellValue("序号");
                    titleRow.CreateCell(1).SetCellValue("用户");
                    titleRow.CreateCell(2).SetCellValue("部门");
                    titleRow.CreateCell(3).SetCellValue("选项");
                    titleRow.CreateCell(4).SetCellValue("劳保");
                }
                else
                {
                    //正文行
                    rows = sheet.CreateRow(i);
                    object entity = list[i - 1];
                    //遍历列
                    for (int j = 1; j <= entityProperties.Length; j++)
                    {
                        //使用对象，不需要指定类型
                        object[] entityValues = new object[entityProperties.Length];
                        entityValues[j - 1] = entityProperties[j - 1].GetValue(entity) is null?"":entityProperties[j - 1].GetValue(entity);
                        rows.CreateCell(0).SetCellValue(i);
                        rows.CreateCell(j).SetCellValue(entityValues[j - 1].ToString());
                    }
                }
            }

            byte[] buffer = new byte[1024 * 2];
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                buffer = ms.ToArray();
                ms.Close();
            }
            return buffer;
        }

        /// <summary>
        /// 设置默认劳保
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task SetDefaultLabor(DefaultLaborViewModel model)
        {
            await _laborDetailRepository.SetDefaultLabor(model);
        }
    } 
    
}
