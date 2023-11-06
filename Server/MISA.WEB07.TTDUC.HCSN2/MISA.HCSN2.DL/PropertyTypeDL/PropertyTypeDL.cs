using Dapper;
using MISA.HCSN2.Common.Entities;
using MISA.HCSN2.Common.Utilities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.HCSN2.DL.PropertyTypeDL
{
    public class PropertyTypeDL : BaseDL<PropertyType> , IPropertyTypeDL
    {
        public IEnumerable<PropertyType> GetPropertyTypeWithFilter(string keyword)
        {
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                //tạo câu lệnh truy vấn 
                string tableName = EntityUtilities.GetTableName<PropertyType>();
                String getAllRecordsCommand = $"SELECT * FROM propertytype WHERE PropertyTypeCode LIKE '%{keyword}%' OR PropertyTypeName LIKE '%{keyword}%' order by ModifiedDate Desc, CreatedDate Desc;";

                // thực hiện chạy câu lệnh 
                var records = mySqlConnection.Query<PropertyType>(getAllRecordsCommand);

                return records;
            }
        }

        /// <summary>
        /// hàm validate trước khi xóa loại tài sản
        /// </summary>
        /// <param name="propertyTypeId"></param>
        /// <returns></returns>
        public bool ValidateDeletePropertyType(Guid propertyTypeId)
        {
            bool isValid = true;

            string queryValidate = $"SELECT COUNT(1) FROM property p WHERE p.PropertyTypeID = '{propertyTypeId}';";

            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var numberOfProperty = sqlConnection.Query<int>(queryValidate, commandType: System.Data.CommandType.Text).FirstOrDefault();
                if (numberOfProperty > 0)
                {
                    isValid = false;
                }
            }
            
            return isValid;
        }
    }
}
