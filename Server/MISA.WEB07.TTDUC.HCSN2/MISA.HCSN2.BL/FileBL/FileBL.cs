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
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            var departments = _departmentDL.GetAllRecords().ToList();
            var propertyTypes = _propertyTypeDL.GetAllRecords().ToList();

            // đếm số bản ghi lỗi 
            var countEror = 0;
            var numberOfColumn = 14;
            // mở file 
            var stream = file.OpenReadStream();
            using (ExcelPackage package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.First(); // chọn wordsheet đầu tiên
                var rowCount = worksheet.Dimension.End.Row; // đếm số dòng trong file

                // đọc danh sách property
                for (var row = 2; row <= rowCount; row++)
                {
                    try
                    {
                        int count = this.CheckRowNull(worksheet.Cells, row, numberOfColumn);

                        // nếu count == 0  thì dòng trống
                        if (count == 0) continue;

                        // nếu 0 < count < numberOfColumn thì có cột nhập thiếu thông tin
                        if (count < numberOfColumn)
                        {
                            countEror++;
                            var errCell = worksheet.Cells[row, 15];
                            errCell.Value = "Vui lòng nhập đầy đủ thông tin";
                            continue;
                        }


                        // import property
                        var idResult = Guid.Empty;
                        try
                        {
                            // còn lại thì nhập đủ thông tin
                            var property = new Property();
                            property.PropertyID = Guid.NewGuid();
                            property.PropertyCode = worksheet.Cells[row, 1].Value?.ToString();
                            property.PropertyName = worksheet.Cells[row, 2].Value?.ToString();
                            property.DepartmentCode = worksheet.Cells[row, 3].Value?.ToString();
                            property.DepartmentName = worksheet.Cells[row, 4].Value?.ToString();
                            property.PropertyTypeCode = worksheet.Cells[row, 5].Value?.ToString();
                            property.PropertyTypeName = worksheet.Cells[row, 6].Value?.ToString();
                            property.Quantity = Int32.Parse(worksheet.Cells[row, 7].Value?.ToString());
                            property.MarkedPrice = double.Parse(worksheet.Cells[row, 8].Value?.ToString());
                            property.UsedYear = Int16.Parse(worksheet.Cells[row, 9].Value?.ToString());
                            property.AttritionRate = double.Parse(worksheet.Cells[row, 10].Value?.ToString());
                            property.AttritionValue = double.Parse(worksheet.Cells[row, 11].Value?.ToString());
                            property.TrackingYear = Int16.Parse(worksheet.Cells[row, 12].Value?.ToString());
                            property.PurchasingDate = DateTime.Parse(worksheet.Cells[row, 13].Value.ToString());
                            property.StartUsingDate = DateTime.Parse(worksheet.Cells[row, 14].Value.ToString());
                            property.CreatedDate = DateTime.Now;
                            property.CreatedBy = "Import";
                            property.ModifiedDate = DateTime.Now;
                            property.ModifiedBy = "Import";

                            var department = departments.Find(item => (item.DepartmentCode == property.DepartmentCode));
                            var propertyType = propertyTypes.Find(item => (item.PropertyTypeCode == property.PropertyTypeCode));

                            property.DepartmentID = department.DepartmentID;
                            property.DepartmentName = department.DepartmentName;
                            property.PropertyTypeID = propertyType.PropertyTypeID;
                            property.PropertyTypeName = propertyType.PropertyTypeName;


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

        private int CheckRowNull(ExcelRange cells, int row, int numberOfColumn)
        {
            int count = 0;
            for (int i = numberOfColumn; i > 0; i--)
            {
                if (cells[row, i].Value != null && !string.IsNullOrEmpty(cells[row, i].Value.ToString()))
                {
                    count++;
                }
            }

            return count;
        }

        public byte[] ExportFileExcel(ExcelPackage pack)
        {
            var properties = _propertyDL.GetAllRecords().Select(item => new { item.PropertyCode, item.PropertyName, item.DepartmentCode, item.DepartmentName, item.PropertyTypeCode, item.PropertyTypeName, item.Quantity, item.MarkedPrice, item.UsedYear, item.AttritionRate, item.AttritionValue, item.TrackingYear, PurchasingDate = item.PurchasingDate.ToString("dd/MM/yyyy"), StartUsingDate = item.StartUsingDate.ToString("dd/MM/yyyy") }); ;
            var departments = _departmentDL.GetAllRecords().ToList();
            var propertyTypes = _propertyTypeDL.GetAllRecords().ToList();

            var departmentExorts = departments.Select(item => new { item.DepartmentCode, item.DepartmentName });
            var propertyTypeExports = propertyTypes.Select(item => new { item.PropertyTypeCode, item.PropertyTypeName });
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelWorksheet ws1 = pack.Workbook.Worksheets.First();
            ExcelWorksheet ws2 = pack.Workbook.Worksheets.Add("Danh sách phòng ban");
            ExcelWorksheet ws3 = pack.Workbook.Worksheets.Add("Danh sách loại tài sản");

            ws1.Cells["A2"].LoadFromCollection(properties);
            ws2.Cells["A1"].LoadFromCollection(departmentExorts);
            ws3.Cells["A1"].LoadFromCollection(propertyTypeExports);

            return pack.GetAsByteArray();
        }

        public byte[] ExportSelectExcel(string lstId, ExcelPackage pack)
        {
            var properties = _propertyDL.GetRecordsByLstId(lstId).Select(item => new { item.PropertyCode, item.PropertyName, item.DepartmentCode, item.DepartmentName, item.PropertyTypeCode, item.PropertyTypeName, item.Quantity, item.MarkedPrice, item.UsedYear, item.AttritionRate, item.AttritionValue, item.TrackingYear, PurchasingDate = item.PurchasingDate.ToString("dd/MM/yyyy"), StartUsingDate =  item.StartUsingDate.ToString("dd/MM/yyyy") });
            var departments = _departmentDL.GetAllRecords().ToList();
            var propertyTypes = _propertyTypeDL.GetAllRecords().ToList();

            var departmentExorts = departments.Select(item => new { item.DepartmentCode, item.DepartmentName });
            var propertyTypeExports = propertyTypes.Select(item => new { item.PropertyTypeCode, item.PropertyTypeName });
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            ExcelWorksheet ws1 = pack.Workbook.Worksheets.First();
            ExcelWorksheet ws2 = pack.Workbook.Worksheets.Add("Danh sách phòng ban");
            ExcelWorksheet ws3 = pack.Workbook.Worksheets.Add("Danh sách loại tài sản");

            ws1.Cells["A2"].LoadFromCollection(properties);
            ws2.Cells["A1"].LoadFromCollection(departmentExorts);
            ws3.Cells["A1"].LoadFromCollection(propertyTypeExports);

            return pack.GetAsByteArray();
        }
    }
}
