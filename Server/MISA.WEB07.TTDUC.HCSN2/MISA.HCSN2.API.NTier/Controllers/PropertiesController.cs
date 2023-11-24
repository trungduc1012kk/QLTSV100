using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MISA.HCSN2.API.NTier.BaseControllers;
using MISA.HCSN2.API.NTier.Helper;
using MISA.HCSN2.BL.PropertyBL;
using MISA.HCSN2.Common.Entities;
using MySqlConnector;
using OfficeOpenXml;
using Swashbuckle.AspNetCore.Annotations;
using System.IO;

namespace MISA.HCSN2.API.NTier.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class PropertiesController : BaseControllers<Property>
    {
        #region Field

        private IPropertyBL _propertyBL;

        #endregion

        #region Constructor

        public PropertiesController(IPropertyBL propertyBL) : base(propertyBL)
        {
            _propertyBL = propertyBL;
        }

        #endregion


        #region Method

        /// <summary>
        /// hàm lấy danh sách và tổng số bản ghi đã được lọc
        /// Author: TTDuc (06/09/2022)
        /// </summary>
        /// <returns>danh sách tài sản và tổng số bản ghi</returns>
        [SwaggerResponse(statusCode: StatusCodes.Status200OK)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError)]
        [HttpPost("paging-data")]
        public IActionResult GetPagingData(
            [FromQuery] string? keyword,
            [FromQuery] Guid? departmentId,
            [FromQuery] Guid? propertyTypeId,
            [FromQuery] int? status,
            [FromBody] List<Guid>? listID,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var properties = _propertyBL.GetPagingData(listID, keyword, departmentId, propertyTypeId, status, pageNumber, pageSize);
                if (properties != null)
                {
                    return StatusCode(StatusCodes.Status200OK, properties);
                }
                return StatusCode(StatusCodes.Status404NotFound, HandleError.GenerateNotFoundErrorResult());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateExceptionResult(e));
            }

        }

        /// <summary>
        /// hàm xóa nhiều tài sản 
        /// Author: TTDuc (06/09/2022)
        /// </summary>
        /// <returns>số tài sản bị xóa</returns>
        [SwaggerResponse(statusCode: StatusCodes.Status200OK)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError)]
        [HttpDelete("delete-multiple")]
        public IActionResult DeleteMultipleProperty(List<Guid> listId)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, _propertyBL.DeleteMultipleProperty(listId));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateExceptionResult(e));
            }
        }

        /// <summary>
        /// ghi tăng nhiều tài sản
        /// </summary>
        /// <param name="properties">danh sách tài sản</param>
        /// <param name="incrementID">ID ghi tăng</param>
        /// <returns></returns>
        [SwaggerResponse(statusCode: StatusCodes.Status200OK)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError)]
        [HttpPost("increase-property")]
        public IActionResult IncreaseProperty([FromBody] List<Property> properties, Guid incrementID)
        {
            int count = _propertyBL.IncreaseProperty(properties, incrementID);

            return StatusCode(StatusCodes.Status200OK, count);
        }

        /// <summary>
        /// xóa nhiều tài sản trong danh sách ghi tăng
        /// </summary>
        /// <param name="properties">danh sách tài sản</param>
        /// <param name="incrementID">ID ghi tăng</param>
        /// <returns></returns>
        [SwaggerResponse(statusCode: StatusCodes.Status200OK)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError)]
        [HttpPost("decrease-property")]
        public IActionResult DecreaseProperty([FromBody] List<Property> properties, Guid incrementID)
        {
            int count = _propertyBL.DecreaseProperty(properties, incrementID);

            return StatusCode(StatusCodes.Status200OK, count);
        }


        #endregion



    }
}
