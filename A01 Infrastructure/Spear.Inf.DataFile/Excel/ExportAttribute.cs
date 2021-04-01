using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

using Spear.Inf.Core.Tool;

namespace Spear.Inf.DataFile.Excel
{
    public enum Enum_AggregateType
    {
        /// <summary>
        /// 不参与总计
        /// </summary>
        None,

        /// <summary>
        /// 默认、即值直接输出
        /// </summary>
        Normal,

        /// <summary>
        /// 列值总合
        /// </summary>
        Sum,
    }

    public enum Enum_ExportDataType
    {
        /// <summary>
        /// 任意
        /// </summary>
        Whatever,

        /// <summary>
        /// 时间
        /// </summary>
        DateTime,

        /// <summary>
        /// 整数
        /// </summary>
        Integer,

        /// <summary>
        /// 数值
        /// </summary>
        Decimals,
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ExportSheetAttribute : Attribute
    {
        public ExportSheetAttribute(string sheetName)
        {
            SheetName = sheetName;
        }

        public string SheetName { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ExportColAttribute : Attribute
    {
        public ExportColAttribute(int index, string title, Type dataType)
        {
            Index = index;
            Title = title;
            DataType = dataType;
            DataFormat = "";
            Prefix = "";
            Suffix = "";
            Enlargement = 1;
        }

        public ExportColAttribute(int index, string title, Type dataType, string dataFormat = "", string prefix = "", string suffix = "", int enlargement = 1)
        {
            Index = index;
            Title = title;
            DataType = dataType;
            DataFormat = dataFormat;
            Prefix = prefix;
            Suffix = suffix;
            Enlargement = enlargement;
        }

        /// <summary>
        /// 列索引
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public Type DataType { get; set; }

        /// <summary>
        /// 格式化
        /// </summary>
        public string DataFormat { get; set; }

        /// <summary>
        /// 开始字符
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// 结束字符
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// 放大倍数
        /// </summary>
        public int Enlargement { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public Enum_ExportDataType EDataType
        {
            get
            {
                if (DataType.Equals(typeof(DateTime)))
                    return Enum_ExportDataType.DateTime;
                else if (DataType.Equals(typeof(int)))
                    return Enum_ExportDataType.Integer;
                else if (
                    DataType.Equals(typeof(decimal))
                    || DataType.Equals(typeof(float))
                    || DataType.Equals(typeof(double))
                )
                    return Enum_ExportDataType.Decimals;
                else
                    return Enum_ExportDataType.Whatever;
            }
        }

        /// <summary>
        /// 获取值
        /// 填充到表格之前调用
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string GetDataValue(object obj)
        {
            if (obj == null)
                return "";

            string result = "";

            switch (EDataType)
            {
                case Enum_ExportDataType.DateTime:
                    result = DataFormat.IsEmptyString() ?
                        ((DateTime)obj).ToString("yyyy-MM-dd HH:mm:ss") : ((DateTime)obj).ToString(DataFormat);
                    break;
                case Enum_ExportDataType.Integer:
                    var val_int = (int)obj * Enlargement;
                    result = DataFormat.IsEmptyString() ? val_int.ToString() : val_int.ToString(DataFormat);
                    break;
                case Enum_ExportDataType.Decimals:
                    var val_dcm = (decimal)obj * Enlargement;
                    result = DataFormat.IsEmptyString() ? val_dcm.ToString() : val_dcm.ToString(DataFormat);
                    break;
                default:
                    result = obj.ToString();
                    break;
            }

            return Prefix + result + Suffix;
        }

        /// <summary>
        /// 计算值
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public object Calculate(object obj1, object obj2)
        {
            if (obj1 == null && obj2 == null)
                return null;
            else if (obj1 == null)
                return obj2;
            else if (obj2 == null)
                return obj1;

            return CalculateValue(obj1, obj2);
        }

        /// <summary>
        /// 计算值
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        protected virtual object CalculateValue(object obj1, object obj2)
        {
            return obj1;
        }
    }

    public class ExportContentAttribute : ExportColAttribute
    {
        public ExportContentAttribute(int index, string title, Type dataType)
            : base(index, title, dataType)
        {
            //
        }

        public ExportContentAttribute(int index, string title, Type dataType, string dataFormat = "", string prefix = "", string suffix = "", int enlargement = 1)
            : base(index, title, dataType, dataFormat, prefix, suffix, enlargement)
        {
            //
        }
    }

    public class ExportAggregateAttribute : ExportColAttribute
    {
        public ExportAggregateAttribute(int index, string title, Type dataType)
            : base(index, title, dataType)
        {
            EAggregateType = Enum_AggregateType.None;
        }

        public ExportAggregateAttribute(int index, string title, Type dataType, string dataFormat = "", string prefix = "", string suffix = "", int enlargement = 1)
            : base(index, title, dataType, dataFormat, prefix, suffix, enlargement)
        {
            EAggregateType = Enum_AggregateType.None;
        }

        public ExportAggregateAttribute(int index, string title, Type dataType, Enum_AggregateType eAggregateType)
            : base(index, title, dataType)
        {
            EAggregateType = eAggregateType;
        }

        public ExportAggregateAttribute(int index, string title, Type dataType, Enum_AggregateType eAggregateType, string dataFormat = "", string prefix = "", string suffix = "", int enlargement = 1)
            : base(index, title, dataType, dataFormat, prefix, suffix, enlargement)
        {
            EAggregateType = eAggregateType;
        }

        /// <summary>
        /// 底部总计使用计算
        /// </summary>
        public Enum_AggregateType EAggregateType { get; set; }

        protected override object CalculateValue(object obj1, object obj2)
        {
            switch (EAggregateType)
            {
                case Enum_AggregateType.Normal:
                    return obj1;
                case Enum_AggregateType.Sum:
                    switch (EDataType)
                    {
                        case Enum_ExportDataType.Integer:
                            return (int)obj1 + (int)obj2;
                        case Enum_ExportDataType.Decimals:
                            return (decimal)obj1 + (decimal)obj2;
                        default:
                            return obj1;
                    }
                default:
                    return obj1;
            }
        }
    }

    public static class ExportExtend
    {
        public static DataTable ToDataTable<T>(this List<T> objList) where T : new()
        {
            objList = objList == null ? new List<T>() : objList;

            DataTable result = null;
            T data_aggregate = default;

            ExportSheetAttribute sheetAttr = null;
            List<ExportColAttribute> cols = new List<ExportColAttribute>();
            List<PropertyInfo> pi_content = new List<PropertyInfo>();
            List<PropertyInfo> pi_aggregate = new List<PropertyInfo>();

            ExportColAttribute col = null;
            DataRow row_content = null;
            DataRow row_aggregate = null;

            #region 初始化

            Type type = typeof(T);

            sheetAttr = type.GetCustomAttribute<ExportSheetAttribute>();
            if (sheetAttr == null)
                sheetAttr = new ExportSheetAttribute("");

            PropertyInfo[] piArray = type.GetProperties();
            if (piArray == null)
                return result;

            result = new DataTable(sheetAttr.SheetName);
            data_aggregate = new T();

            foreach (var pi1 in piArray)
            {
                col = pi1.GetCustomAttribute<ExportContentAttribute>(false);
                if (col != null)
                {
                    cols.Add(col);
                    pi_content.Add(pi1);
                }

                col = pi1.GetCustomAttribute<ExportAggregateAttribute>(false);
                if (col != null)
                {
                    cols.Add(col);
                    pi_aggregate.Add(pi1);

                    foreach (var obj in objList)
                    {
                        Type type2 = obj.GetType();

                        foreach (var pi2 in piArray)
                        {
                            if (pi1.Name != pi2.Name)
                                continue;

                            var obj1 = pi1.GetValue(data_aggregate, null);
                            var obj2 = pi2.GetValue(obj, null);
                            var dataValue = col.Calculate(obj1, obj2);
                            pi1.SetValue(data_aggregate, dataValue);
                        }
                    }
                }
            }

            cols = cols.GroupBy(o => o.Index).Select(o => o.First()).ToList();

            if (cols.Count() == 0)
                return result;

            #endregion

            #region 数据填充

            foreach (var titleCol in cols)
                result.Columns.Add(titleCol.Title, typeof(object));

            foreach (var obj in objList)
            {
                row_content = result.NewRow();

                foreach (var pi in pi_content)
                {
                    col = pi.GetCustomAttribute<ExportContentAttribute>(false);
                    if (col != null)
                    {
                        var colData = pi.GetValue(obj, null);
                        row_content[col.Title] = col.GetDataValue(colData);
                    }
                }

                result.Rows.Add(row_content);
            }

            row_aggregate = result.NewRow();

            foreach (var pi in pi_aggregate)
            {
                col = pi.GetCustomAttribute<ExportAggregateAttribute>(false);
                if (col != null)
                {
                    var colData = pi.GetValue(data_aggregate, null);
                    row_aggregate[col.Title] = col.GetDataValue(colData);
                }
            }

            result.Rows.Add(row_aggregate);

            #endregion

            return result;
        }
    }
}
