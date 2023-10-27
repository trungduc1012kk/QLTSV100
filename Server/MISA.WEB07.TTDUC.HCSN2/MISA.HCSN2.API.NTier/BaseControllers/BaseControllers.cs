using Microsoft.AspNetCore.Mvc;
using MISA.HCSN2.API.NTier.Helper;
using MISA.HCSN2.BL;
using MISA.HCSN2.BL.PropertyBL;
using MISA.HCSN2.Common;
using MISA.HCSN2.Common.Entities;
using MISA.HCSN2.Common.Enums;
using MySqlConnector;
using Swashbuckle.AspNetCore.Annotations;

namespace MISA.HCSN2.API.NTier.BaseControllers
{
    public class BaseControllers<T> : ControllerBase
    {


        #region Field

        private IBaseBL<T> _baseBL;

        #endregion

        #region Constructor

        public BaseControllers(IBaseBL<T> baseBL)
        {
            _baseBL = baseBL;
        }


        #endregion

        /// <summary>
        /// lấy danh sách bản ghi
        /// Author : TTDuc (05/09/2022)
        /// </summary>
        /// <returns>tất cả bản ghi</returns>
        [SwaggerResponse(statusCode: StatusCodes.Status200OK)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public IActionResult GetAllRecords()
        {
            try
            {
                var records = _baseBL.GetAllRecords();
                if(records != null)
                {

                return StatusCode(StatusCodes.Status200OK, records );
                }

                return StatusCode(StatusCodes.Status404NotFound, HandleError.GenerateNotFoundErrorResult());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateExceptionResult(ex));
            }
        }

        /// <summary>
        /// hàm lấy thông tin 1 bản ghi
        /// Author : TTDuc (05/09/2022)
        /// </summary>
        /// <param name="Id">Id bản ghi</param>
        /// <returns>thông tin bản ghi</returns>
        [SwaggerResponse(statusCode: StatusCodes.Status200OK)]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError)]
        [HttpGet("{Id}")]
        public IActionResult GetRecordById(Guid Id)
        {

            try
            {
                var result = _baseBL.GetRecordById(Id);
                if (result != null)
                {
                    return StatusCode(StatusCodes.Status200OK, result);

                }
                return StatusCode(StatusCodes.Status404NotFound, HandleError.GenerateNotFoundErrorResult());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateExceptionResult(ex));
            }
        }

        /// <summary>
        /// hàm xóa 1 bản ghi
        /// Author : TTDuc (05/09/2022)
        /// </summary>
        /// <param name="Id">Id bản ghi</param>
        /// <returns>ID bản ghi</returns>
        [SwaggerResponse(statusCode: StatusCodes.Status200OK)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError)]
        [HttpDelete("{Id}")]
        public IActionResult DeleteRecordById([FromRoute] Guid Id)
        {

            try
            {
                var numberOfAffectedRows = _baseBL.DeleteRecordById(Id);
                if (numberOfAffectedRows > 0)
                {
                    return StatusCode(StatusCodes.Status200OK, Id);
                }
                else return StatusCode(StatusCodes.Status404NotFound, HandleError.GenerateNotFoundErrorResult());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateExceptionResult(ex));
                
            }
        }



        /// <summary>
        /// thêm mới 1 bản ghi
        /// Author : TTDuc (06/09/2022)
        /// </summary>
        /// <param name="record">thông tin bản ghi</param>
        /// <returns>ID bản ghi</returns>
        [HttpPost]
        public virtual IActionResult InsertOneRecord([FromBody] T record)
        {
            try
            {

                var result = _baseBL.InsertOneRecord(record);

                if (result != Guid.Empty)
                {
                    return StatusCode(StatusCodes.Status201Created, result);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, HandleError.GenerateServerErrorResult());
            }
            catch (ExceptionService ex)
            {
                if(ex.ErrorCode == MISAErrorCode.DuplicateCode)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateDuplicateErrorResult(ex));
                }
                return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateValidateErrorResult(ex));
            }
        }


        /// <summary>
        /// sửa 1 bản ghi
        /// Author : TTDuc (06/09/2022)
        /// </summary>
        /// <param name="record">thông tin bản ghi</param>
        /// <returns>ID bản ghi</returns>
        [SwaggerResponse(statusCode: StatusCodes.Status200OK)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError)]
        [HttpPut("{Id}")]
        public IActionResult UpdateOneRecord([FromBody] T record)
        {
            try
            {

                var result = _baseBL.UpdateOneRecord(record);

                if (result != Guid.Empty)
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }
                    return StatusCode(StatusCodes.Status404NotFound, HandleError.GenerateNotFoundErrorResult());
            }
            catch (ExceptionService ex)
            {
                if (ex.ErrorCode == MISAErrorCode.DuplicateCode)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateDuplicateErrorResult(ex));
                }
                return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateValidateErrorResult(ex));
            }
        }

        /// <summary>
        /// hàm lấy mã cóe mới
        /// Author: TTDuc(06/09/2022)
        /// </summary>
        /// <returns>mã code mới</returns>
        [SwaggerResponse(statusCode: StatusCodes.Status200OK)]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError)]
        [HttpGet("new-code")]
        public IActionResult GetNewCode()
        {

            try
            {
                var result = _baseBL.GetNewCode();
                if (result != null)
                {
                    return StatusCode(StatusCodes.Status200OK, result);

                }
                return StatusCode(StatusCodes.Status404NotFound, HandleError.GenerateNotFoundErrorResult());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateExceptionResult(ex));
            }
        }

    }
}
