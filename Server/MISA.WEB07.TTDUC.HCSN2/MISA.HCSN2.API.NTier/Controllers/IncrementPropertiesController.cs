using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.HCSN2.API.NTier.BaseControllers;
using MISA.HCSN2.API.NTier.Helper;
using MISA.HCSN2.BL;
using MISA.HCSN2.BL.IncrementPropertyBL;
using MISA.HCSN2.Common.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace MISA.HCSN2.API.NTier.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class IncrementPropertiesController : BaseControllers<IncrementProperty>
    {
        #region Field

        private IIncrementPropertyBL _incrementPropertyBL;

        #endregion

        #region Constructor
        public IncrementPropertiesController(IIncrementPropertyBL incrementPropertyBL) : base(incrementPropertyBL)
        {
            _incrementPropertyBL = incrementPropertyBL;
        }

        #endregion


        #region Method

        /// <summary>
        /// lấy dánh sách chứng từ 
        /// Author: TTDuc(11/10/2022)
        /// </summary>
        /// <param name="keyword">từ khóa tìm kiếm</param>
        /// <param name="pageNumber">số trang </param>
        /// <param name="pageSize">số chứng từ trong 1 trang</param>
        /// <returns></returns>
        [SwaggerResponse(statusCode: StatusCodes.Status200OK)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError)]
        [HttpGet("paging-data")]
        public IActionResult GetPagingIncrementProperty([FromQuery] string keyword, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return StatusCode(StatusCodes.Status200OK, _incrementPropertyBL.GetPagingIncrementProperty(keyword, pageNumber, pageSize));
        }

        /// <summary>
        /// hàm xóa chứng từ ghi tăng 
        /// Author: TTDuc (17/10/2022)
        /// </summary>
        /// <returns>số tài sản bị xóa</returns>
        [SwaggerResponse(statusCode: StatusCodes.Status200OK)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError)]
        [HttpDelete("delete-multiple")]
        public IActionResult DeleteMultipleIncrementProperty(List<Guid> listId)
        {
            try
            {
                return StatusCode(StatusCodes.Status200OK, _incrementPropertyBL.DeleteMultipleIncrementProperty(listId));
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateExceptionResult(e));
            }
        }

        #endregion


    }
}
