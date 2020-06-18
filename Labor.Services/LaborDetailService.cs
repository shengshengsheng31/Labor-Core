using Labor.IRepository;
using Labor.IServices;
using Labor.Model;
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
            if (await _laborDetailRepository.GetAll().AnyAsync(m => m.UserId == model.UserId && m.LaborId == model.LaborId))
            {
                return false;
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
                return true;
            }
        }

        /// <summary>
        /// 获取劳保通过LaborId
        /// </summary>
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

            List<LaborDetailListViewModel> list = GetAllByHead(model).ToList();
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("sheet");
            IRow Title = null;
            IRow rows = null;
            Type entityType = list[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();

            for (int i = 0; i <= list.Count; i++)
            {
                if (i == 0)
                {
                    Title = sheet.CreateRow(0);
                    Title.CreateCell(0).SetCellValue("序号");
                }
                else
                {
                    rows = sheet.CreateRow(i);
                    object entity = list[i - 1];
                    for (int j = 1; j <= entityProperties.Length; j++)
                    {
                        object[] entityValues = new object[entityProperties.Length];
                        entityValues[j - 1] = entityProperties[j - 1].GetValue(entity);
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
    } 
    
}
