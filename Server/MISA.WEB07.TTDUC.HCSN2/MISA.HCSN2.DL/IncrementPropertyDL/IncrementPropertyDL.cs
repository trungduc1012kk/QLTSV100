using Dapper;
using MISA.HCSN2.Common.Entities;
using MISA.HCSN2.Common.Utilities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.HCSN2.DL.IncrementPropertyDL
{
    public class IncrementPropertyDL : BaseDL<IncrementProperty>, IIncrementPropertyDL
    {

        /// <summary>
        /// phân trang và tìm kiếm chứng từ
        /// Author: TTDuc(17/10/2022)
        /// </summary>
        /// <param name="keyword">từ khóa lọc theo số chứng từ </param>
        /// <param name="offset">vị trí lấy</param>
        /// <param name="limit">số lượng trong 1 trang</param>
        /// <returns>trả về tổng số bản ghi và danh sách</returns>
        public PagingData<IncrementProperty> GetPagingIncrementProperty(string? keyword, int pageNumber, int pageSize)
        {
            // thực hiện chuẩn hóa dữ liệu
            string whereClause = "";
            var orConditions = new List<String>();
            if (keyword != null)
            {
                orConditions.Add($"IncrementPropertyCode LIKE '%{keyword}%'");
                orConditions.Add($"Description LIKE '%{keyword}%'");
            }
            whereClause = String.Join(" OR ", orConditions);
            int offset = (pageNumber - 1) * pageSize;
            int limit = pageSize;

            // tên procedure
            string procedureName = "Proc_increment_property_GetPaging";

            // tạo ra các param
            var parameters = new DynamicParameters();
            parameters.Add("v_Where", whereClause);
            parameters.Add("v_Offset", offset);
            parameters.Add("v_Limit", limit);
            parameters.Add("v_Sort", "ModifiedDate DESC");

            //thực hiện truy vấn vào DB
            var pagingData = new PagingData<IncrementProperty>();
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var result = mySqlConnection.QueryMultiple(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                pagingData.Data = result.Read<IncrementProperty>().ToList();
                pagingData.TotalCount = result.Read<long>().Single();

            }
            return pagingData;
        }

        /// <summary>
        /// thêm mới trong bảng ghi tăng chi tiết
        /// Author: TTDuc(17/10/2022)
        /// </summary>
        /// <param name="incrementPropertyID">ID chứng từ ghi tăng</param>
        /// <param name="listPropertyID">danh sách Id của tài sản được ghi tăng trong chứng từ</param>
        /// <param name="sqlConnection">kết nối đến Database</param>
        /// <param name="trans">transaction</param>
        public void InsertIncrementPropertyDetail(Guid incrementPropertyID, List<Guid> listPropertyID, MySqlConnection sqlConnection, MySqlTransaction trans)
        {
            // câu lệnh query 
            string insertCommand = "INSERT INTO increment_property_detail (IncrementID , PropertyID) VALUES ";
            foreach (var item in listPropertyID)
            {
                if (item == listPropertyID[listPropertyID.Count - 1])
                {
                    insertCommand = insertCommand + $" ('{incrementPropertyID}', '{item}' );";
                }
                else
                {
                    insertCommand = insertCommand + $" ('{incrementPropertyID}', '{item}' ), ";
                }
            }
            var result = sqlConnection.Execute(insertCommand, commandType: System.Data.CommandType.Text, transaction: trans);
        }

        /// <summary>
        /// ghi tăng các tài sản 
        /// Author: TTDuc(17/10/2022)
        /// </summary>
        /// <param name="listProperty">danh sách tài sản được ghi tăng</param>
        /// <param name="sqlConnection">kết nối DB</param>
        /// <param name="trans">transaction</param>
        public void IncreaseProperty(List<Property> listProperty, MySqlConnection sqlConnection, MySqlTransaction trans)
        {
            foreach (var property in listProperty)
            {
                // Lấy tên Procedure
                string procedureName = "Proc_property_Increase";

                // chuẩn bị tham số đầu vào
                var parameters = new DynamicParameters();
                parameters.Add("v_PropertyID", property.PropertyID);
                parameters.Add("v_Budget", property.Budget);
                parameters.Add("v_MarkedPrice", property.MarkedPrice);

                // thực hiện truy vấn vào DB
                var result = sqlConnection.Execute(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure, transaction: trans);
            }
        }

        /// <summary>
        /// thêm mới chứng từ ghi tăng
        /// Author: TTDuc(17/10/2022)
        /// </summary>
        /// <param name="incrementProperty">Chứng từ</param>
        /// <returns>ID chứng từ đã được thêm</returns>
        public override Guid InsertOneRecord(IncrementProperty incrementProperty)
        {

            // thực hiện lấy id mới 
            incrementProperty.IncrementPropertyID = Guid.NewGuid();

            // thực hiện lấy danh sách id của tài sản 
            var listPropertyID = new List<Guid>();
            var listProperty = incrementProperty.ListProperty;
            foreach (var item in incrementProperty.ListProperty)
            {
                listPropertyID.Add(item.PropertyID);
            }

            // tên proc dùng để truy vấn
            string insertRecordProcedureName = $"Proc_increment_property_InsertOne";


            // chuẩn bị tham số đầu vào 
            var properties = typeof(IncrementProperty).GetProperties();
            var parameters = new DynamicParameters();
            foreach (var property in properties)
            {
                var value = property.GetValue(incrementProperty); // lấy giá trị của property

                var propertyName = property.Name; // lấy tên của property

                if (propertyName != "ListProperty")
                {
                    parameters.Add($"v_{propertyName}", value);
                }
            }

            // thực hiện gọi vào DB
            int numberOfAffetedRows = 0;
            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                sqlConnection.Open();
                using (MySqlTransaction trans = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        var incrementPropertyID = incrementProperty.IncrementPropertyID;

                        // thực hiện lưu trong bảng Detail
                        InsertIncrementPropertyDetail(incrementPropertyID, listPropertyID, sqlConnection, trans);

                        // thực hiện cập nhật trạng thái cho bảng property
                        IncreaseProperty(listProperty, sqlConnection, trans);

                        numberOfAffetedRows = sqlConnection.Execute(insertRecordProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure, transaction: trans);
                        trans.Commit();

                        if (numberOfAffetedRows > 0)
                        {
                            return incrementPropertyID;
                        }


                        return Guid.Empty;
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();

                        throw;
                    }
                }

            }
        }

        /// <summary>
        /// Xóa chứng từ ghi tăng
        /// Author: TTDuc(17/10/2022)
        /// </summary>
        /// <param name="incrememtPropertyID">ID chứng từ muốn xóa</param>
        /// <returns>ID chứng từ muốn xóa</returns>
        public override int DeleteRecordById(Guid incrememtPropertyID)
        {
            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                sqlConnection.Open();
                using (var trans = sqlConnection.BeginTransaction())
                {

                    try
                    {
                        // thực hiện reset status trong bảng tài sản
                        ResetStatusProperty(incrememtPropertyID, sqlConnection, trans);

                        // thực hiện xóa trong bảng chi tiết ghi tăng
                        DeleteDetailByID(incrememtPropertyID, sqlConnection, trans);

                        // câu lệnh truy vấn
                        string deleteRecordByIdCommand = $"DELETE FROM increment_property WHERE incrementPropertyID = '{incrememtPropertyID}';";

                        // thực thi câu lệnh

                        var result = sqlConnection.Execute(deleteRecordByIdCommand, transaction: trans);

                        trans.Commit();

                        //trả về kết quả 
                        return result;
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// set lại status của tài sản là chưa ghi tăng
        /// Author: TTDuc(17/10/2022)
        /// </summary>
        /// <param name="incrememtPropertyID"></param>
        /// <param name="sqlConnection"></param>
        /// <param name="trans"></param>
        public void ResetStatusProperty(Guid incrememtPropertyID, MySqlConnection sqlConnection, MySqlTransaction trans)
        {
            // lấy danh sách ID của tài sản được ghi tăng với ID = incrememtPropertyID
            string getIDcommand = $"Select PropertyID From increment_property_detail Where IncrementID = '{incrememtPropertyID}'";

            var result = sqlConnection.Query<Guid>(getIDcommand, commandType: System.Data.CommandType.Text, transaction: trans);

            string listIdToString = string.Join("','", result);

            // tạo câu lệnh reset
            string resetStatusCommand = $"UPDATE Property SET Status = 0 WHERE PropertyID IN ('{listIdToString}');";

            sqlConnection.Execute(resetStatusCommand, commandType: System.Data.CommandType.Text, transaction: trans);
        }

        /// <summary>
        /// xóa chứng từ ghi tăng chi tiết
        /// Author: TTDuc(17/10/2022)
        /// </summary>
        /// <param name="incrememtPropertyID">ID chứng từ</param>
        /// <param name="sqlConnection">kết nối DB</param>
        /// <param name="trans">transaction</param>
        public void DeleteDetailByID(Guid incrememtPropertyID, MySqlConnection sqlConnection, MySqlTransaction trans)
        {
            // tạo câu lệnh delete
            string deleteCommand = $"DELETE FROM increment_property_Detail WHERE IncrementID = '{incrememtPropertyID}'";

            // truy vấn vào Database
            sqlConnection.Execute(deleteCommand, commandType: System.Data.CommandType.Text, transaction: trans);
        }

        /// <summary>
        /// Lấy Chi tiết chứng từ 
        /// Author: TTDuc(17/10/2022)
        /// </summary>
        /// <param name="Id">ID chứng từ</param>
        /// <returns>Chi tiết chứng từ</returns>
        public override IEnumerable<dynamic> GetRecordById(Guid Id)
        {
            //var incrementProperty = base.GetRecordById(Id);

            // lấy danh sách tài sản thuộc bản ghi tăng với ID cho trược 
            string selectCommand = $"SELECT * FROM Property p RIGHT JOIN increment_property_detail i  ON p.PropertyID = i.PropertyID WHERE i.IncrementID = '{Id}'";

            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var listTS = sqlConnection.Query<Property>(selectCommand, commandType: System.Data.CommandType.Text);


                return listTS;
            }
        }

        /// <summary>
        /// xóa nhiều chứng từ
        /// Author: TTDuc(17/10/2022)
        /// </summary>
        /// <param name="listId">danh sách ID chứng từu muốn xóa</param>
        /// <returns></returns>
        public int DeleteMultipleIncrementProperty(List<Guid> listId)
        {
            // lấy procedure name
            string procedureNameCommand = "Proc_increment_property_DeleteMultiple";


            // tạo param
            var parameters = new DynamicParameters();
            var listIdToString = "";
            if (listId == null || listId.Count == 0)
            {
                return 0;
            }
            listIdToString = $"('{String.Join("','", listId)}')";
            parameters.Add("@v_ListIdToString", listIdToString);


            // tạo kết nối và truy vấn vào Database
            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                sqlConnection.Open();
                using (var trans = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        // thwucj hiện xóa trong bảng chứng từ ghi tăng
                        var result = sqlConnection.Execute(procedureNameCommand, parameters, commandType: System.Data.CommandType.StoredProcedure, transaction: trans);

                        foreach (var incrememtPropertyID in listId)
                        {
                            // thực hiện reset status trong bảng tài sản
                            ResetStatusProperty(incrememtPropertyID, sqlConnection, trans);

                            // thực hiện xóa trong bảng chi tiết ghi tăng
                            DeleteDetailByID(incrememtPropertyID, sqlConnection, trans);
                        }
                        trans.Commit();

                        return result;
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        throw;
                    }

                }

            }
        }

        /// <summary>
        /// Sửa chứng từ ghi tăng
        /// Author: TTDuc(17/10/2022)
        /// </summary>
        /// <param name="incrementProperty">Chứng từ ghi tăng</param>
        /// <returns>ID chứng từ</returns>
        public override Guid UpdateOneRecord(IncrementProperty incrementProperty)
        {

            // thực hiện lấy danh sách id của tài sản 
            var listPropertyID = new List<Guid>();
            foreach (var item in incrementProperty.ListProperty)
            {
                listPropertyID.Add(item.PropertyID);
            }

            // lấy id chứng từ ghi tăng
            var incrementPropertyID = incrementProperty.IncrementPropertyID;

            string updateRecordProcedureName = $"Proc_increment_property_UpdateOne";
            // chuẩn bị tham số đầu vào 
            var properties = typeof(IncrementProperty).GetProperties();
            var parameters = new DynamicParameters();
            foreach (var property in properties)
            {
                var value = property.GetValue(incrementProperty); // lấy giá trị của property

                var propertyName = property.Name; // lấy tên của property

                if (propertyName != "ListProperty")
                {
                    parameters.Add($"v_{propertyName}", value);
                }
            }

            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {

                sqlConnection.Open();
                using (var trans = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        //thực hiện cập nhật trạng thái về 0 trong bảng tài sản
                        ResetStatusProperty(incrementPropertyID, sqlConnection, trans);

                        //thực hiện xóa tất cả trong bảng detail với id của ghi tăng
                        DeleteDetailByID(incrementPropertyID, sqlConnection, trans);

                        // thực hiện lưu trong bảng Detail
                        InsertIncrementPropertyDetail(incrementPropertyID, listPropertyID, sqlConnection, trans);

                        // thực hiện cập nhật trạng thái cho bảng property
                        IncreaseProperty(incrementProperty.ListProperty, sqlConnection, trans);

                        // thực hiện update trong bảng ghi tăng tài sản
                        int numberOfAffetedRows = 0;
                        numberOfAffetedRows = sqlConnection.Execute(updateRecordProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure, transaction: trans);

                        trans.Commit();
                        if (numberOfAffetedRows > 0)
                        {
                            return incrementProperty.IncrementPropertyID;
                        }
                        return Guid.Empty;

                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
