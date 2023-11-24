using Dapper;
using MISA.HCSN2.Common.Entities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.HCSN2.DL.PropertyDL
{
    public class PropertyDL : BaseDL<Property>, IPropertyDL
    {

        /// <summary>
        /// hàm lấy danh sách và tổng số bản ghi đã được lọc
        /// Author: TTDuc (06/09/2022)
        /// </summary>
        /// <returns>danh sách tài sản và tổng số bản ghi</returns>
        public PagingData<Property> GetPagingData(List<Guid>? listID, string? keyword, Guid? departmentId, Guid? propertyTypeId, int? status, int pageNumber, int pageSize)
        {
            // lấy tên procedure
            string getPagingDataProcedureName = "Proc_property_GetPaging";

            // chuẩn bị tham số dầu vào 

            string whereClause = "";
            var andConditions = new List<String>();
            var orConditions = new List<String>();
            if (keyword != null)
            {
                orConditions.Add($"propertyName LIKE '%{keyword}%'");
                orConditions.Add($"propertyCode LIKE '%{keyword}%'");
            }

            if (departmentId != null)
            {
                andConditions.Add($"DepartmentId = '{departmentId}'");
            }

            if (propertyTypeId != null)
            {
                andConditions.Add($"PropertyTypeId = '{propertyTypeId}'");
            }

            if (status != null)
            {
                andConditions.Add($"Status = {status}");
            }

            if (listID != null)
            {
                var listIdToString = $"PropertyID NOT IN ('{String.Join("','", listID)}')";
                andConditions.Add(listIdToString);
            }

            if (orConditions.Count() > 0)
            {
                whereClause = String.Join(" OR ", orConditions);

                if (andConditions.Count() > 0)
                {
                    whereClause = "(" + whereClause + ") AND " + String.Join(" AND ", andConditions);
                }
            }
            else
            {
                whereClause = String.Join(" AND ", andConditions);
            }




            var parameters = new DynamicParameters();

            parameters.Add("v_Sort", "ModifiedDate DESC");
            parameters.Add("v_Where", whereClause);
            parameters.Add("v_Limit", pageSize);
            parameters.Add("v_Offset", (pageNumber - 1) * pageSize);

            // thực hiện truy vấn 

            var pagingData = new PagingData<Property>();
            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var multipleResult = sqlConnection.QueryMultiple(getPagingDataProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                if (multipleResult != null)
                {
                    pagingData.Data = multipleResult.Read<Property>().ToList();
                    pagingData.TotalCount = multipleResult.Read<long>().Single();

                }
            }

            return pagingData;

        }

        /// <summary>
        /// hàm xóa nhiều tài sản 
        /// Author: TTDuc (06/09/2022)
        /// </summary>
        /// <returns>số tài sản bị xóa</returns>
        public int DeleteMultipleProperty(List<Guid> listId)
        {
            // lấy procedure name
            string procedureNameCommand = "Proc_property_DeleteMultiple";


            // tạo param
            var parameters = new DynamicParameters();

            var listIdToString = "";

            if (listId == null || listId.Count == 0)
            {
                return 0;
            }
            listIdToString = $"('{String.Join("','", listId)}')";

            parameters.Add("@v_ListIdToString", listIdToString);


            // chạy câu lệnh 

            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {

                var result = sqlConnection.Execute(procedureNameCommand, parameters, commandType: System.Data.CommandType.StoredProcedure);

                return result;

            }
        }

        /// <summary>
        /// ghi tăng tài sản 
        /// Author : TTDuc(10/10/2022)
        /// </summary>
        /// <param name="propertyID">ID tài sản </param>
        /// <param name="increamentPropertyID">ID chứng từ ghi tăng </param>
        /// <param name="budget">nguồn ngân sách </param>
        /// <returns>ID tài sản đã được ghi tăng</returns>
        public Guid IncreaseProperty(Guid propertyID, Guid increamentID, string budget)
        {
            // lấy tên procedureName 
            string procedureName = "Proc_property_Increase";

            // tạo param
            var parameters = new DynamicParameters();

            parameters.Add("v_IncrementID", increamentID);
            parameters.Add("v_PropertyID", propertyID);
            parameters.Add("v_Budget", budget);

            var numberOfAffectedRows = 0;
            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                numberOfAffectedRows = sqlConnection.Execute(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }

            if (numberOfAffectedRows > 0)
            {
                return propertyID;
            }

            return Guid.Empty;
        }

        /// <summary>
        /// xóa nhiều tài sản trong danh sách ghi tăng
        /// </summary>
        /// <param name="propertyID">ID tài sản</param>
        /// <param name="increamentPropertyID">ID ghi tăng</param>
        /// <returns>ID tài sản đã được xóa</returns>
        public Guid DecreaseProperty(Guid propertyID, Guid increamentID)
        {
            // tên Procedure 
            var procedureName = "Proc_property_Decrease";

            // tạo param
            var parameters = new DynamicParameters();

            parameters.Add("v_IncrementID", increamentID);
            parameters.Add("v_PropertyID", propertyID);

            var numberOfAffectedRows = 0;
            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                numberOfAffectedRows = sqlConnection.Execute(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);
            }

            if (numberOfAffectedRows > 0)
            {
                return propertyID;
            }

            return Guid.Empty;
        }
    }
}
