using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using MISA.HCSN2.API.NTier.Helper;
using MISA.HCSN2.BL.DepartmentBL;
using MISA.HCSN2.BL.FileBL;
using MISA.HCSN2.BL.PropertyBL;
using MISA.HCSN2.BL.PropertyTypeBL;
using OfficeOpenXml;
using Swashbuckle.AspNetCore.Annotations;

namespace MISA.HCSN2.API.NTier.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private IFileBL _fileBL;
        private IDepartmentBL _departmentBL;
        private IPropertyTypeBL _propertyTypeBL;

        public FilesController(IWebHostEnvironment environment, 
            IFileBL fileBL,
            IDepartmentBL departmentBL,
            IPropertyTypeBL propertyTypeBL)
        {
            _environment = environment;
            _fileBL = fileBL;
            _departmentBL = departmentBL;
            _propertyTypeBL = propertyTypeBL;
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> Upload(IFormFile upload)
        {
            if (upload == null || upload.Length == 0)
                return BadRequest("Invalid file");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(upload.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
                return BadRequest("Invalid file extension");

            var uploadsFolderPath = Path.Combine(_environment.WebRootPath, "images");
            if (!Directory.Exists(uploadsFolderPath))
                Directory.CreateDirectory(uploadsFolderPath);

            string fileName = "Image_" + DateTime.Now.Ticks + fileExtension;
            var filePath = Path.Combine(uploadsFolderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await upload.CopyToAsync(stream);
            }

            var urlGetImage = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/Files/get-image/" + fileName;

            return Ok(new
            {
                url = urlGetImage,
            });
        }


        [HttpGet("get-image/{fileName}")]
        public IActionResult GetImage(string fileName)
        {
            var uploadsFolderPath = Path.Combine(_environment.WebRootPath, "images");
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var fileExtension = Path.GetExtension(filePath);
            var mimeType = GetMimeType(fileExtension);

            return File(fileStream, mimeType);
        }

        private string GetMimeType(string fileExtension)
        {
            var provider = new FileExtensionContentTypeProvider(); 
            if (!provider.TryGetContentType(fileExtension, out var mimeType))
            {
                // Set a default mime type if the provider doesn't recognize the extension
                mimeType = "application/octet-stream";
            }
            return mimeType;
        }


        /// <summary>
        /// chức năng nhập khẩu dữ liệu
        /// Author: TTDuc(09/09/2022)
        /// </summary>
        /// <param name="file">file excel</param>
        /// <returns>trả về danh sách ID đã thêm được và tổng số bản ghi bị lỗi</returns>
        [SwaggerResponse(statusCode: StatusCodes.Status200OK)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError)]
        [HttpPost("import-excel")]
        public IActionResult ImportFileExcel(IFormFile file)
        {

            try
            {
                var result = _fileBL.ImportFileExcel(file);

                return StatusCode(StatusCodes.Status200OK, result);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateExceptionResult(e));
            }
        }

        /// <summary>
        /// chức năng xuất khẩu dữ liệu
        /// Author: TTDuc(11/11/2023)
        /// </summary>
        /// <param name="file">file excel</param>
        /// <returns>trả về danh sách ID đã thêm được và tổng số bản ghi bị lỗi</returns>
        [SwaggerResponse(statusCode: StatusCodes.Status200OK)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError)]
        [HttpGet("export-excel")]
        public IActionResult ExportFileExcel(string ListID)
        {

            try
            {
                var result = _fileBL.ExportFileExcel();
                string excelName = $"PropertyList-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
                return File(result, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateExceptionResult(e));
            }
        }

        /// <summary>
        /// chức năng xuất khẩu dữ liệu
        /// Author: TTDuc(11/11/2023)
        /// </summary>
        /// <param name="file">file excel</param>
        /// <returns>trả về danh sách ID đã thêm được và tổng số bản ghi bị lỗi</returns>
        [SwaggerResponse(statusCode: StatusCodes.Status200OK)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError)]
        [HttpGet("export-template")]
        public IActionResult GetFileImportTemplate()
        {
            try
            {
                string path = Path.Combine(this._environment.WebRootPath, "FileTemplate/ImportTemplate.xlsx");
                byte[] bytes;
                using (var package = new ExcelPackage(new FileInfo(path)))
                {
                    // thêm 2 worksheet
                    var sheetName2 = "DanhSachPhongBan";
                    var sheetName3 = "DanhSachLoaiTaiSan";
                    ExcelWorksheet ws2 = package.Workbook.Worksheets.Add(sheetName2);
                    ExcelWorksheet ws3 = package.Workbook.Worksheets.Add(sheetName3);

                    // Lấy worksheet đầu tiên
                    var worksheet = package.Workbook.Worksheets.First(); 

                    // Lấy dữ liệu từ database
                    var departments = _departmentBL.GetAllRecords().ToList();
                    var propertyTypes = _propertyTypeBL.GetAllRecords().ToList();
                    var lastUsedRow = 1000;

                    // danh sách tài phòng ban
                    var departmentExorts = departments.Select(item => new { item.DepartmentCode, item.DepartmentName });
                    // Danh sách loại tài sản
                    var propertyTypeExports = propertyTypes.Select(item => new { item.PropertyTypeCode, item.PropertyTypeName, item.UsedYearDefault, item.AttritionRateDefault });

                    // load dữ liệu cho 2 worksheet thêm mới
                    ws2.Cells["A1"].LoadFromCollection(departmentExorts);
                    ws3.Cells["A1"].LoadFromCollection(propertyTypeExports);

                    // khai báo tên cho danh sách
                    ws2.Names.Add("DepartmentCode", ws2.Cells[$"A1:A{departments.Count}"]);
                    ws2.Names.Add("DepartmentName", ws2.Cells[$"B1:B{departments.Count}"]);
                    ws3.Names.Add("PropertyTypeCode", ws3.Cells[$"A1:A{propertyTypes.Count}"]);
                    ws3.Names.Add("PropertyTypeName", ws3.Cells[$"B1:B{propertyTypes.Count}"]);

                    // set validation cho phòng ban
                    var departmentCodeValidation = worksheet.DataValidations.AddListValidation($"C2:C{lastUsedRow}");
                    departmentCodeValidation.Formula.ExcelFormula = $"'{sheetName2}'!DepartmentCode";

                    var departmentNameValidation = worksheet.DataValidations.AddListValidation($"D2:D{lastUsedRow}");
                    departmentNameValidation.Formula.ExcelFormula = $"'{sheetName2}'!DepartmentName";


                    // set validation cho loại tài sản
                    var propertyTypeCodeValidation = worksheet.DataValidations.AddListValidation($"E2:E{lastUsedRow}");
                    propertyTypeCodeValidation.Formula.ExcelFormula = $"'{sheetName3}'!PropertyTypeCode";

                    var propertyTypeNameValidation = worksheet.DataValidations.AddListValidation($"F2:F{lastUsedRow}");
                    propertyTypeNameValidation.Formula.ExcelFormula = $"'{sheetName3}'!PropertyTypeName";


                    // set công thức cho tên phòng ban
                    worksheet.Cells[$"D2:D{lastUsedRow}"].Formula = $"IFERROR(INDEX('{sheetName2}'!DepartmentName,MATCH(OFFSET(INDIRECT(ADDRESS(ROW(), COLUMN())),0,-1),'{sheetName2}'!DepartmentCode,0)), \"\")";
                    worksheet.Cells[$"F2:F{lastUsedRow}"].Formula = $"IFERROR(INDEX('{sheetName3}'!PropertyTypeName,MATCH(OFFSET(INDIRECT(ADDRESS(ROW(), COLUMN())),0,-1),'{sheetName3}'!PropertyTypeCode,0)), \"\")";


                    // protect worksheet
                    worksheet.Protection.IsProtected = true;
                    ws2.Protection.IsProtected = true;
                    ws3.Protection.IsProtected = true;

                    //unlock cho các cột cho người dùng edit
                    var unlockCellName = new string[] { "A", "B", "C", "E", "G", "H", "I", "J", "K", "L", "M","N" };
                    for (int i = unlockCellName.Count() - 1; i >= 0; i--)
                    {
                        worksheet.Cells[$"{unlockCellName[i]}2:{unlockCellName[i]}{lastUsedRow}"].Style.Locked = false;
                    }

                    bytes = package.GetAsByteArray();
                }

                string excelName = $"ImportTemplate.xlsx";
                return File(bytes, "application/octet-stream", excelName);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateExceptionResult(e));
            }
        }
    }
}
