using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

using OfficeOpenXml;

using Spear.Inf.Core.Tool;

namespace Spear.Inf.DataFile.Excel
{
    public class EPPlusHelper : IExcelHelper
    {
        public EPPlusHelper() { ExcelPackage.LicenseContext = LicenseContext.NonCommercial; }

        #region 导入Excel

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public DataSet ImportToDataSet(FileInfo fileInfo)
        {
            var set = new DataSet();
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                foreach (var worksheet in package.Workbook.Worksheets)
                {
                    set.Tables.Add(WorksheetToTable(worksheet));
                }
            }
            return set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        public static DataTable WorksheetToTable(ExcelWorksheet worksheet)
        {
            //获取worksheet的行数
            int rows = worksheet.Dimension.End.Row;
            //获取worksheet的列数
            int cols = worksheet.Dimension.End.Column;

            DataTable dt = new DataTable(worksheet.Name);
            DataRow dr = null;
            for (int i = 1; i <= rows; i++)
            {
                if (i > 1)
                {
                    dr = dt.Rows.Add();
                }

                for (int j = 1; j <= cols; j++)
                {
                    //默认将第一行设置为datatable的标题
                    if (i == 1)
                    {
                        dt.Columns.Add(worksheet.Cells[i, j].Value?.ToString());
                    }
                    else
                    {
                        dr[j - 1] = worksheet.Cells[i, j].Value?.ToString();
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public List<T> ImportToList<T>(T obj, MemoryStream memoryStream, string sheet = null) where T : new()
        {
            var list = new List<T>();
            using (var package = new ExcelPackage(memoryStream))
            {
                var worksheet = sheet.IsEmptyString() ? package.Workbook.Worksheets[0] : package.Workbook.Worksheets[sheet];

                var rowCount = worksheet.Dimension?.Rows;
                var colCount = worksheet.Dimension?.Columns;

                if (rowCount.HasValue && colCount.HasValue)
                {
                    for (int row = 2; row <= rowCount.Value; row++)
                    {
                        var currently = new T();
                        for (int col = 1; col <= colCount.Value; col++)
                        {
                            if (worksheet.Cells[1, col].Value != null && worksheet.Cells[row, col].Value != null)
                            {
                                ReflectionsHelper.DynamicBestowValue(currently, worksheet.Cells[1, col].Value.ToString(), worksheet.Cells[row, col].Value.ToString());
                                ReflectionsHelper.DynamicBestowValue(currently, obj);
                            }
                        }
                        list.Add(currently);
                    }
                }
                return list;
            }
        }

        #endregion

        #region 导出Excel

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public byte[] ExportFromDataTable(DataTable table)
        {
            byte[] result = null;

            using (ExcelPackage pck = new ExcelPackage())
            {
                string sheetName = table.TableName.IsEmptyString() ? "Sheet1" : table.TableName;
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(sheetName);

                ws.Cells["A1"].LoadFromDataTable(table, true);

                result = pck.GetAsByteArray();
            }

            return result;
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public byte[] ExportFromList<T>(IEnumerable<T> list)
        {
            byte[] result = null;
            using (ExcelPackage pck = new ExcelPackage())
            {
                string sheetName = "Sheet1";
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(sheetName);

                ws.Cells["A1"].LoadFromCollection(list, true);

                result = pck.GetAsByteArray();
            }

            return result;
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="filePath">存放路径</param>
        /// <param name="fileName">文件名称。格式：*.xlsx</param>
        /// <param name="table">数据源</param>
        /// <param name="isDeleteSameNameSheet">是否删除重名的表</param>
        /// <returns></returns>
        public string ExportFromDataTable(string filePath, string fileName, DataTable table, bool isDeleteSameNameSheet)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                FileInfo file = new FileInfo(Path.Combine(filePath, fileName));
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    // 添加worksheet
                    var worksheet = AddSheet(package, table.TableName, isDeleteSameNameSheet);
                    worksheet.Cells["A1"].LoadFromDataTable(table, true);
                    package.Save();
                }
                return "完成";
            }
            catch (Exception ex)
            {
                return ex.InnerException?.Message ?? ex.Message;
            }
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="filePath">存放路径</param>
        /// <param name="fileName">文件名称。格式：*.xlsx</param>
        /// <param name="sourceTable">数据源</param>
        /// <param name="isDeleteSameNameSheet">是否删除重名的表</param>
        /// <returns></returns>
        public string ExportFromList<T>(string filePath, string fileName, List<T> list, bool isDeleteSameNameSheet)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                FileInfo file = new FileInfo(Path.Combine(filePath, fileName));
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    // 添加worksheet
                    var worksheet = AddSheet(package, fileName.Split('.')[0], isDeleteSameNameSheet);
                    worksheet.Cells["A1"].LoadFromCollection(list, true);
                    package.Save();
                }
                return "完成";
            }
            catch (Exception ex)
            {
                return ex.InnerException?.Message ?? ex.Message;
            }
        }

        /// <summary>
        /// 添加Sheet到ExcelPackage
        /// </summary>
        /// <param name="package">package</param>
        /// <param name="sheetName">sheet名称</param>
        /// <param name="isDeleteSameNameSheet">如果存在同名的sheet是否删除</param>
        /// <returns></returns>
        public ExcelWorksheet AddSheet(ExcelPackage package, string sheetName, bool isDeleteSameNameSheet)
        {
            if (isDeleteSameNameSheet)
            {
                DeleteSheet(package, sheetName);
            }
            else
            {
                var total = package.Workbook.Worksheets.Where(item => sheetName.Equals(item.Name)).ToList().Count;
                if (total > 0)
                {
                    sheetName = $"{sheetName}({total})";
                }
            }

            return package.Workbook.Worksheets.Add(sheetName);
        }

        /// <summary>
        /// 删除指定的sheet
        /// </summary>
        /// <param name="package"></param>
        /// <param name="sheetName"></param>
        public void DeleteSheet(ExcelPackage package, string sheetName)
        {
            var sheet = package.Workbook.Worksheets.FirstOrDefault(i => i.Name == sheetName);
            if (sheet != null)
            {
                package.Workbook.Worksheets.Delete(sheet);
            }
        }

        #endregion
    }
}
