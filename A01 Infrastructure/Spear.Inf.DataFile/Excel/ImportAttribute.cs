using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using OfficeOpenXml;

namespace Spear.Inf.DataFile.Excel
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ImportSheetAttribute : Attribute
    {
        public ImportSheetAttribute(string sheetName)
        {
            SheetName = sheetName;
        }

        public string SheetName { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ImportColAttribute : Attribute
    {
        public ImportColAttribute(Type colType, string colName)
        {
            ColType = colType;
            ColName = colName;
        }

        public Type ColType { get; set; }
        public string ColName { get; set; }
    }

    public static class ImportExtend
    {
        public static List<T> ToObject<T>(this Stream stream) where T : class, new()
        {
            List<T> result = new List<T>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ImportSheetAttribute sheetAttr = null;
            Dictionary<ImportColAttribute, PropertyInfo> cols = new Dictionary<ImportColAttribute, PropertyInfo>();
            List<Dictionary<ImportColAttribute, object>> datas = new List<Dictionary<ImportColAttribute, object>>();

            #region 初始化 列标识 从泛型对象载入

            Type type = typeof(T);

            sheetAttr = type.GetCustomAttribute<ImportSheetAttribute>();
            if (sheetAttr == null)
                return result;

            PropertyInfo[] piArray = type.GetProperties();
            if (piArray == null)
                return result;

            foreach (var pi in piArray)
            {
                var attr = pi.GetCustomAttribute<ImportColAttribute>(false);
                if (attr == null)
                    continue;

                cols.Add(attr, pi);
            }

            #endregion

            #region 初始化 数据内容 从文件载入

            using (var package = new ExcelPackage(stream))
            {
                if (package == null) return result;
                if (package.Workbook == null) return result;
                if (package.Workbook.Worksheets == null) return result;

                var sheet = package.Workbook.Worksheets[sheetAttr.SheetName];
                if (sheet == null) return result;

                var dimension = sheet.Dimension;
                if (dimension == null) return result;

                int rowQty = dimension.Rows;
                int colQty = dimension.Columns;

                if (colQty != cols.Count()) return result;

                try
                {
                    //跳过第一行的列名
                    for (int rowIndex = 1; rowIndex < rowQty; rowIndex++)
                    {
                        Dictionary<ImportColAttribute, object> data = new Dictionary<ImportColAttribute, object>();

                        for (int colIndex = 0; colIndex < colQty; colIndex++)
                        {
                            var title = cols.Keys.ToList()[colIndex];

                            //sheet.Cells的索引是从1开始的，所以要+1
                            object value = sheet.Cells[rowIndex + 1, colIndex + 1].Value;

                            data.Add(title, value);
                        }

                        datas.Add(data);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("初始化数据内容发生错误", ex);
                }
            }

            #endregion

            foreach (var data in datas)
            {
                T resultItem = new T();

                foreach (var col in cols)
                {
                    object value = Convert.ChangeType(data[col.Key], col.Key.ColType);
                    col.Value.SetValue(resultItem, value);
                }

                result.Add(resultItem);
            }

            return result;
        }
    }
}
