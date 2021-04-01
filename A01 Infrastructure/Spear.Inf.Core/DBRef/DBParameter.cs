using System.Data;

namespace Spear.Inf.Core.DBRef
{
    public class DBParameter
    {
        public string Name { get; set; }

        public object Value { get; set; }

        public bool IsNullable { get; set; }

        public ParameterDirection Direction { get; set; }

        public DbType DBType { get; set; }

        public int Size { get; set; }

        //public DBParameter(string name, object value, bool isNullable, ParameterDirection direction, DbType dbType, int size)
        //{
        //    Name = name;
        //    Value = value;
        //    IsNullable = isNullable;
        //    Direction = direction;
        //    DBType = dbType;
        //    Size = size;
        //}
    }
}
