using System.Collections.Generic;
using System.Data;
using System.IO;

using Spear.Inf.Core.Injection;

namespace Spear.Inf.DataFile.Excel
{
    public interface IExcelHelper : ITransient
    {
        #region 导入Excel

        /// <summary>
        /// 返回dataset
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        DataSet ImportToDataSet(FileInfo fileInfo);

        /// <summary>
        /// 返回List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="memoryStream"></param>
        /// <returns></returns>
        List<T> ImportToList<T>(T obj, MemoryStream memoryStream, string sheet = null) where T : new();

        #endregion

        #region 导出Excel

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="table">存放路径</param>
        /// <returns></returns>
        byte[] ExportFromDataTable(DataTable table);

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="filePath">存放路径</param>
        /// <param name="fileName">文件名称。格式：*.xlsx</param>
        /// <param name="table">数据源</param>
        /// <param name="isDeleteSameNameSheet">是否删除重名的表</param>
        /// <returns></returns>
        string ExportFromDataTable(string filePath, string fileName, DataTable table, bool isDeleteSameNameSheet);

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="filePath">存放路径</param>
        /// <param name="fileName">文件名称。格式：*.xlsx</param>
        /// <param name="sourceTable">数据源</param>
        /// <param name="isDeleteSameNameSheet">是否删除重名的表</param>
        /// <returns></returns>
        string ExportFromList<T>(string filePath, string fileName, List<T> list, bool isDeleteSameNameSheet);

        #endregion
    }
}
