using Microsoft.AspNetCore.Http;
using MISA.HCSN2.BL.DepartmentBL;
using MISA.HCSN2.BL.PropertyBL;
using MISA.HCSN2.BL.PropertyTypeBL;
using MISA.HCSN2.Common;
using MISA.HCSN2.Common.Entities;
using MISA.HCSN2.DL.DepartmentDL;
using MISA.HCSN2.DL.PropertyDL;
using MISA.HCSN2.DL.PropertyTypeDL;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace MISA.HCSN2.BL.FileBL
{
    public class FileBL : IFileBL
    {
        private IPropertyDL _propertyDL;

        private IDepartmentDL _departmentDL;

        private IPropertyTypeDL _propertyTypeDL;

        private IPropertyBL _propertyBL;

        public FileBL(IPropertyDL propertyDL,
            IDepartmentDL departmentDL,
            IPropertyTypeDL propertyTypeDL,
            IPropertyBL propertyBL)
        {
            _propertyDL = propertyDL;
            _departmentDL = departmentDL;
            _propertyTypeDL = propertyTypeDL;
            _propertyBL = propertyBL;
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

            var departments = _departmentDL.GetAllRecords().ToList();
            var propertyTypes = _propertyTypeDL.GetAllRecords().ToList();

            // đếm số bản ghi lỗi 
            var countEror = 0;

            // mở file 
            var stream = file.OpenReadStream();
            using (ExcelPackage package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.First(); // chọn wordsheet đầu tiên
                var rowCount = worksheet.Dimension.Rows > 1000 ? 1000 : worksheet.Dimension.Rows; // đếm số dòng trong file

                // đọc danh sách property
                for (var row = 2; row <= rowCount; row++)
                {
                    try
                    {
                        // đọc từng dòng trong file
                        var property = new Property()
                        {
                            PropertyID = Guid.NewGuid(),
                            PropertyCode = worksheet.Cells[row, 1].Value?.ToString(),
                            PropertyName = worksheet.Cells[row, 2].Value?.ToString(),
                            DepartmentCode = worksheet.Cells[row, 3].Value?.ToString(),
                            DepartmentName = worksheet.Cells[row, 4].Value?.ToString(),
                            PropertyTypeCode = worksheet.Cells[row, 5].Value?.ToString(),
                            PropertyTypeName = worksheet.Cells[row, 6].Value?.ToString(),
                            Quantity = Int32.Parse(worksheet.Cells[row, 7].Value?.ToString()),
                            MarkedPrice = double.Parse(worksheet.Cells[row, 8].Value?.ToString()),
                            UsedYear = Int16.Parse(worksheet.Cells[row, 9].Value?.ToString()),
                            AttritionRate = double.Parse(worksheet.Cells[row, 10].Value?.ToString()),
                            AttritionValue = double.Parse(worksheet.Cells[row, 11].Value?.ToString()),
                            TrackingYear = Int16.Parse(worksheet.Cells[row, 12].Value?.ToString()),
                            PurchasingDate = DateTime.FromOADate(float.Parse(worksheet.Cells[row, 13].Value?.ToString())),
                            StartUsingDate = DateTime.FromOADate(float.Parse(worksheet.Cells[row, 14].Value?.ToString())),
                            CreatedDate = DateTime.Now,
                            CreatedBy = "Import",
                            ModifiedDate = DateTime.Now,
                            ModifiedBy = "Import",
                        };

                        

                        var department = departments.Find(item => (item.DepartmentCode == property.DepartmentCode));
                        var propertyType = propertyTypes.Find(item => (item.PropertyTypeCode == property.PropertyTypeCode));

                        property.DepartmentID = department.DepartmentID;
                        property.DepartmentName = department.DepartmentName;
                        property.PropertyTypeID = propertyType.PropertyTypeID;
                        property.PropertyTypeName = propertyType.PropertyTypeName;

                        var isRequired = true;
                        var propertyRequired = property.GetType().GetProperties();
                        for (int i = 0; i < propertyRequired.Count(); i++)
                        {
                            var value = propertyRequired[i].GetValue(property, null);
                            if (value == null)
                            {
                                isRequired = false;
                            }
                        }

                        if (!isRequired) continue;

                        // import property
                        var idResult = Guid.Empty;
                        try
                        {
                            idResult = _propertyBL.InsertOneRecord(property);

                        }
                        catch (ExceptionService e)
                        {
                            var errCell = worksheet.Cells[row, 15];
                            errCell.Value = e.Data.Values.Cast<string>().ToList();
                        }

                        if (idResult != Guid.Empty)
                        {
                            listIdInsert.Add(idResult);
                        }
                        else
                        {
                            countEror++;
                        }

                    }
                    catch (Exception ex)
                    {
                        countEror++;
                        var errCell = worksheet.Cells[row, 15];
                        errCell.Value = "Lỗi sai kiểu dữ liệu";
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

        public byte[] ExportFileExcel()
        {
            var properties = _propertyDL.GetAllRecords();
            var departments = _departmentDL.GetAllRecords().ToList();
            var propertyTypes = _propertyTypeDL.GetAllRecords().ToList();

            var departmentExorts = departments.Select(item => new {item.DepartmentCode, item.DepartmentName});
            var propertyTypeExports = propertyTypes.Select(item => new { item.PropertyTypeCode, item.PropertyTypeName });
            using ExcelPackage pack = new ExcelPackage();

            ExcelWorksheet ws1 = pack.Workbook.Worksheets.Add("Danh sách tài sản");
            ExcelWorksheet ws2 = pack.Workbook.Worksheets.Add("Danh sách phòng ban");
            ExcelWorksheet ws3 = pack.Workbook.Worksheets.Add("Danh sách loại tài sản");

            ws1.Cells["A1"].LoadFromCollection(properties);
            ws2.Cells["A1"].LoadFromCollection(departmentExorts);
            ws3.Cells["A1"].LoadFromCollection(propertyTypeExports);

            return pack.GetAsByteArray();

        }
    }
}
