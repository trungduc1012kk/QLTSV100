using Dapper;
using MISA.HCSN2.Common.Attributes;
using MISA.HCSN2.Common.Utilities;
using MySqlConnector;
using System.ComponentModel.DataAnnotations;

namespace MISA.HCSN2.DL
{
    public class BaseDL<T> : IBaseDL<T>
    {


        #region Method

        /// <summary>
        /// lấy tất cả bản ghi
        /// Author: TTDUC 24/08/2022
        /// </summary>
        /// <returns>tất cả bản ghi của 1 bảng</returns>
        public virtual IEnumerable<T> GetAllRecords()
        {
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                //tạo câu lệnh truy vấn 
                string tableName = EntityUtilities.GetTableName<T>();
                String getAllRecordsCommand = $"SELECT * FROM {tableName} order by ModifiedDate Desc, CreatedDate Desc;";

                // thực hiện chạy câu lệnh 
                var records = mySqlConnection.Query<T>(getAllRecordsCommand);

                return records;
            }
        }

        /// <summary>
        /// lấy thông tin 1 bản ghi 
        /// Author : TTDuc (05/09/2022)
        /// </summary>
        /// <param name="Id">Id bản ghi muốn lấy</param>
        /// <returns>thông tin bản ghi đã lấy</returns>
        public virtual IEnumerable<dynamic> GetRecordById(Guid Id)
        {
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                // lấy tên table name
                string tableName = EntityUtilities.GetTableName<T>();

                string className = typeof(T).Name;

                // câu lệnh truy vấn
                string getRecordByIdCommand = $"SELECT * FROM {tableName} WHERE {className}ID = '{Id}';";

                // chạy câu lệnh truy vấn
                var record = mySqlConnection.QueryFirstOrDefault<T>(getRecordByIdCommand);

                // trả về giá trị
                yield return record;
            }
        }

        /// <summary>
        /// xóa 1 bản ghi 
        /// Author : TTDuc (05/09/2022)
        /// </summary>
        /// <param name="Id">Id bản ghi muốn xóa</param>
        /// <returns>Id bản ghi đã xóa</returns>
        public virtual int DeleteRecordById(Guid Id)
        {
            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                // lấy tên table name
                string tableName = EntityUtilities.GetTableName<T>();

                string className = typeof(T).Name;

                // câu lệnh truy vấn
                string deleteRecordByIdCommand = $"DELETE FROM {tableName} WHERE {className}ID = '{Id}';";

                // thực thi câu lệnh

                var result = sqlConnection.Execute(deleteRecordByIdCommand);

                //trả về kết quả 
                return result;
            }
        }

        /// <summary>
        /// hàm thêm mới 1 bản ghi
        /// Author: TTDuc (05/09/2022)
        /// </summary>
        /// <param name="record">thông tin bản ghi</param>
        /// <returns>ID bản ghi đó</returns>
        public virtual Guid InsertOneRecord(T record)
        {

            var newId = Guid.NewGuid();

            var primaryKeyProperty = typeof(T).GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(typeof(KeyAttribute), true).Count() > 0);
            primaryKeyProperty.SetValue(record, newId);

            // tên proc dùng để truy vấn
            string tableName = EntityUtilities.GetTableName<T>();
            string insertRecordProcedureName = $"Proc_{tableName}_InsertOne";


            // chuẩn bị tham số đầu vào 
            var properties = typeof(T).GetProperties();
            var parameters = new DynamicParameters();

            foreach (var property in properties)
            {
                var value = property.GetValue(record); // lấy giá trị của property

                var propertyName = property.Name; // lấy tên của property

                parameters.Add($"v_{propertyName}", value);
            }

            // thực hiện gọi vào DB
            int numberOfAffetedRows = 0;
            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                numberOfAffetedRows = sqlConnection.Execute(insertRecordProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                if (numberOfAffetedRows > 0)
                {
                    return newId;
                }
                return Guid.Empty;
            }

        }

        /// <summary>
        /// hàm sửa 1 bản ghi
        /// Author: TTDuc (05/09/2022)
        /// </summary>
        /// <param name="record">thông tin bản ghi</param>
        /// <returns>ID bản ghi đó</returns>
        public virtual Guid UpdateOneRecord(T record)
        {
            // tên proc dùng để truy vấn
            string tableName = EntityUtilities.GetTableName<T>();

            string updateRecordProcedureName = $"Proc_{tableName}_UpdateOne";


            // chuẩn bị tham số đầu vào 
            var properties = typeof(T).GetProperties();
            var parameters = new DynamicParameters();

            foreach (var property in properties)
            {
                var value = property.GetValue(record); // lấy giá trị của property

                var propertyName = property.Name; // lấy tên của property

                parameters.Add($"v_{propertyName}", value);
            }

            // thực hiện gọi vào DB
            int numberOfAffetedRows = 0;
            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                numberOfAffetedRows = sqlConnection.Execute(updateRecordProcedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                var result = Guid.Empty;

                if (numberOfAffetedRows > 0)
                {
                    var primaryKeyProperty = typeof(T).GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(typeof(KeyAttribute), true).Count() > 0);
                    var Id = primaryKeyProperty?.GetValue(record);
                    if (Id != null)
                    {
                        result = (Guid)Id;
                    }
                }
                return result;

            }

        }

        /// <summary>
        /// kiểm tra trùng mã
        /// Author: TTDuc(18/09/2022)
        /// </summary>
        /// <param name="record">Id bản ghi</param>
        /// <returns>trùng mã hay không</returns>
        public bool CheckDuplicateCode(T record)
        {
            // lấy id record
            var primaryKeyProperty = typeof(T).GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(typeof(KeyAttribute), true).Count() > 0);
            var id = primaryKeyProperty?.GetValue(record);

            // lấy mã record
            var duplicateProperty = typeof(T).GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(typeof(DuplicateAttribute), true).Count() > 0);
            var code = duplicateProperty?.GetValue(record);

            //lấy tên procedure
            string tableName = EntityUtilities.GetTableName<T>();
            string procedureName = $"Proc_{tableName}_CheckCode";

            //tạo các param
            string className = typeof(T).Name;
            var parameters = new DynamicParameters();
            parameters.Add($"v_{className}Code", code);
            parameters.Add($"v_{className}ID", id);


            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                // thực hiện chạy câu lệnh 
                var result = mySqlConnection.QueryFirstOrDefault<T>(procedureName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                if (result == null)
                {
                    return true;
                }
            }

            return false;
        }

        public string GetNewCode()
        {
            string tableName = EntityUtilities.GetTableName<T>();
            string className = typeof(T).Name;

            string newCode = "";
            // câu lệnh lấy code dài nhất
            var maxLengthCommand = $"SELECT MAX(LENGTH({className}Code)) FROM {tableName};";

            using (var sqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {

                var maxLengthCode = sqlConnection.QueryFirstOrDefault<int>(maxLengthCommand);
                var getMaxCodeCommand = $"SELECT MAX({className}Code) FROM {tableName} WHERE LENGTH({className}Code) = {maxLengthCode};";
                var maxCode = sqlConnection.QueryFirstOrDefault<String>(getMaxCodeCommand);

                // nếu phần mã thay đổi bằng null thì tạo ra mã mới
                string prefix = EntityUtilities.GetPreFix<T>();
                if (maxCode == null) return $"{prefix}00001";

                // lấy vị trí xuất hiện của số đuôi
                int indexOfNumber = 0;
                for (int i = maxCode.Length - 1; i >= 0; i--)
                {
                    if (maxCode[i] < '0' || maxCode[i] > '9')
                    {
                        indexOfNumber = i + 1;
                        break;
                    }
                }

                // nếu tồn tài số đuôi thì thực hiện cộng 1 không có thì thực hiện thêm đuôi là 0
                if (indexOfNumber < maxCode.Length)
                {
                    // phần số thay đổi và độ dài của số đuôi
                    var autoNumber = maxCode.Substring(indexOfNumber);
                    int lengthOfAutoNumber = autoNumber.Length;

                    string newNumber = (Int64.Parse(autoNumber) + 1).ToString();

                    // nếu độ dài số đuôi nhỏ hơn số đuôi trước đó thì thực hiện nối '0' vào trước 
                    if (newNumber.Length < lengthOfAutoNumber)
                    {
                        for (int i = newNumber.Length; i < lengthOfAutoNumber; i++)
                        {
                            newNumber = '0' + newNumber;
                        }
                    }

                    newCode = maxCode.Substring(0, indexOfNumber) + newNumber;
                }
                else
                {
                    newCode = maxCode + '0';
                }
            }

            return newCode;
        }

        #endregion
    }
}
