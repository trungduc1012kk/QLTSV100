using MISA.HCSN2.Common.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MISA.HCSN2.Common.Utilities
{
    /// <summary>
    /// những hàm dùng chung xử lý của entity
    /// </summary>
    public static class EntityUtilities
    {

        /// <summary>
        /// hàm lấy tên bảng(table name) trong class
        /// Author: TTDUC 06/09/2022
        /// </summary>
        /// <typeparam name="T">đối tượng truyền vào</typeparam>
        /// <returns>tên bảng</returns>
        public static string GetTableName<T>()
        {
            string nameTable = typeof(T).Name;

            var tableAttributes = typeof(T).GetTypeInfo().GetCustomAttributes<TableAttribute>();
            if(tableAttributes.Count() > 0)
            {
                nameTable = tableAttributes.First().Name;
            }

            return nameTable;
        }

        public static string GetPreFix<T>()
        {
            string prefix = "";

            var tableAttributes = typeof(T).GetTypeInfo().GetCustomAttributes<PrefixAttribute>();
            if (tableAttributes.Count() > 0)
            {
                prefix = tableAttributes.First().Name;
            }

            return prefix;
        }
    }
}
