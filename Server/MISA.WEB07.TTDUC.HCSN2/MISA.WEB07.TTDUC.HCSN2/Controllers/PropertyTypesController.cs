using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.WEB07.TTDUC.HCSN2.Entities;
using MySqlConnector;
using Swashbuckle.AspNetCore.Annotations;

namespace MISA.WEB07.TTDUC.HCSN2.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyTypesController : ControllerBase
    {
        /// <summary>
        /// API Lấy toàn bộ danh sách Loại tài sản
        /// </summary>
        /// <returns>Danh sách loại tài sản</returns>
        /// Created by: TTDUC (18/08/2022)
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(List<PropertyType>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllPropertyType()
        {
            try
            {
                // tạo kết nối với DB
                String connectionString = "Server=localhost;Port=3306;Database=misa.web07.hcsn2.ttduc;Uid=root;Pwd=anhduc000;";
                var mySqlconnection = new MySqlConnection(connectionString);

                // tạo câu lệnh truy vấn 
                String getPropertyTypeCommand = "SELECT * FROM propertyType;";

                // thực hiện truy vấn 
                var propertyTypes = mySqlconnection.Query<PropertyType>(getPropertyTypeCommand);

                // thực hiện trả kết quả về 
                if(propertyTypes != null)
                {
                    return StatusCode(StatusCodes.Status200OK, propertyTypes);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "e002");
                }

            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status400BadRequest, "e001");
            }
        }

    }
}
