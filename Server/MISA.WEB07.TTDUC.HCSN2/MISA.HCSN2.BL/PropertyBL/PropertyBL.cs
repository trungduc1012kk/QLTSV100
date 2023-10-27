using Microsoft.AspNetCore.Http;
using MISA.HCSN2.Common.Entities;
using MISA.HCSN2.DL.PropertyDL;
using OfficeOpenXml;

namespace MISA.HCSN2.BL.PropertyBL
{

    public class PropertyBL : BaseBL<Property>, IPropertyBL
    {

        #region Field

        private IPropertyDL _propertyDL;

        #endregion

        #region Constructor

        public PropertyBL(IPropertyDL propertyDL) : base(propertyDL)
        {
            _propertyDL = propertyDL;
        }

        #endregion

        #region Method

        /// <summary>
        /// hàm lấy danh sách và tổng số bản ghi đã được lọc
        /// Author: TTDuc (06/09/2022)
        /// </summary>
        /// <returns>danh sách tài sản và tổng số bản ghi</returns>
        public PagingData<Property> GetPagingData(List<Guid>? listID,string? keyword, Guid? departmentId, Guid? propertyTypeId, int? status, int PageNumber, int pageSize)
        {
            return _propertyDL.GetPagingData(listID, keyword, departmentId, propertyTypeId, status, PageNumber, pageSize);
        }

        /// <summary>
        /// hàm xóa nhiều tài sản 
        /// Author: TTDuc (06/09/2022)
        /// </summary>
        /// <returns>số tài sản bị xóa</returns>
        public int DeleteMultipleProperty(List<Guid> listId)
        {
            return _propertyDL.DeleteMultipleProperty(listId);
        }

        /// <summary>
        /// sửa 1 bản ghi
        /// Author : TTDuc (10/09/2022)
        /// </summary>
        /// <param name="file">File nhập khẩu</param>
        /// <returns>danh sách Id inport thành công và số bản ghi lỗi</returns>
        public ImportResult<Guid> ImportFileExcel(IFormFile file)
        {
            // lưu lại danh sách Id được import
            List<Guid> listIdInsert = new List<Guid>(); 

            // đếm số bản ghi lỗi 
            var countEror = 0;

            // mở file 
            var stream = file.OpenReadStream();

            using (ExcelPackage package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.First(); // chọn wordsheet đầu tiên
                var rowCount = worksheet.Dimension.Rows; // đếm số dòng trong file

                // đọc danh sách property
                for (var row = 2; row <= rowCount; row++)
                {
                    try
                    {
                        // đọc từng dòng trong file
                        var property = new Property()
                        {
                            PropertyID = Guid.NewGuid(),
                            PropertyCode = worksheet.Cells[row, 2].Value?.ToString(),
                            PropertyName = worksheet.Cells[row, 3].Value?.ToString(),
                            DepartmentID = Guid.Parse(worksheet.Cells[row, 4].Value?.ToString()),
                            DepartmentName = worksheet.Cells[row, 5].Value?.ToString(),
                            DepartmentCode = worksheet.Cells[row, 6].Value?.ToString(),
                            PropertyTypeID = Guid.Parse(worksheet.Cells[row, 7].Value?.ToString()),
                            PropertyTypeCode = worksheet.Cells[row, 8].Value?.ToString(),
                            PropertyTypeName = worksheet.Cells[row, 9].Value?.ToString(),
                            Quantity = Int32.Parse(worksheet.Cells[row, 10].Value?.ToString()),
                            MarkedPrice = double.Parse(worksheet.Cells[row, 11].Value?.ToString()),
                            UsedYear = Int16.Parse(worksheet.Cells[row, 12].Value?.ToString()),
                            AttritionRate = double.Parse(worksheet.Cells[row, 13].Value?.ToString()),
                            AttritionValue = double.Parse(worksheet.Cells[row, 14].Value?.ToString()),
                            TrackingYear = Int16.Parse(worksheet.Cells[row, 15].Value?.ToString()),
                            PurchasingDate = DateTime.FromOADate(float.Parse(worksheet.Cells[row, 16].Value?.ToString())),
                            StartUsingDate = DateTime.FromOADate(float.Parse(worksheet.Cells[row, 17].Value?.ToString())),
                            CreatedDate = DateTime.Now,
                            CreatedBy = "Trung Germany",
                            ModifiedDate = DateTime.Now,
                            ModifiedBy = "Trung Germany",
                        };

                        // import property
                        var idResult = _propertyDL.InsertOneRecord(property);

                        if (idResult != null)
                        {
                            listIdInsert.Add(idResult);
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);

                        countEror++;
                    }
                }

                // trả về số bản ghi lỗi và danh sách Id đã import thành công 
                return new ImportResult<Guid>()
                {
                    listId = listIdInsert,
                    totalCountError = countEror,
                };
            }

        }

        /// <summary>
        /// ghi tăng tài sản 
        /// Author : TTDuc(10/10/2022)
        /// </summary>
        /// <returns>số bản ghi đã được ghi tăng</returns>
        public int IncreaseProperty(List<Property> properties, Guid incrementID)
        {
            int count = 0;

            foreach (var property in properties)
            {
                var id = _propertyDL.IncreaseProperty(property.PropertyID, incrementID, property.Budget);

                if(id != Guid.Empty)
                {
                    count++;
                }
            }   

            return count;
        }

        /// <summary>
        /// xóa nhiều tài sản trong danh sách ghi tăng
        /// </summary>
        /// <param name="properties">dnah sách tài sản được ghi tăng</param>
        /// <param name="incrementID">ID của chứng từ ghi tăng</param>
        /// <returns>số bản ghi được xóa</returns>
        public int DecreaseProperty(List<Property> properties, Guid incrementID)
        {
            int count = 0;

            foreach (var property in properties)
            {
                var id = _propertyDL.DecreaseProperty(property.PropertyID, incrementID);

                if (id != Guid.Empty)
                {
                    count++;
                }
            }

            return count;
        }

        #endregion
    }
}
