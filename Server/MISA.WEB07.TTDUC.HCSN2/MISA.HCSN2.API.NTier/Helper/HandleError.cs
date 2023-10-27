using Microsoft.AspNetCore.Mvc.ModelBinding;
using MISA.HCSN2.Common;
using MISA.HCSN2.Common.Entities.DTO;
using MISA.HCSN2.Common.Resources;
using MySqlConnector;
using System.Diagnostics;

namespace MISA.HCSN2.API.NTier.Helper
{
    /// <summary>
    /// Class static gồm các hàm xử lý lỗi khi gọi API
    /// </summary>
    public static class HandleError
    {

        /// <summary>
        /// Sinh ra đối tượng lỗi trả về khi gặp exception
        /// Author : TTDuc (05/09/2022)
        /// </summary>
        /// <param name="exception">Đối tượng exception gặp phải</param>
        /// <returns>Đối tượng chứa thông tin lỗi trả về cho client</returns>
        public static ErrorResult? GenerateExceptionResult(Exception ex)
        {
            return new ErrorResult(
                Common.Enums.MISAErrorCode.Exception,
                ex.Message,
                ex.Data
                );
        }

        /// <summary>
        /// sinh ra đối tượng lỗi trả về khi lỗi server
        /// </summary>
        /// <returns>Đối tượng lỗi </returns>
        public static ErrorResult? GenerateServerErrorResult( )
        {
            var errorResult = new ErrorResult(
                Common.Enums.MISAErrorCode.ServerError,
                ErrorResource.ServerException,
                ErrorResource.ServerException);

            return errorResult;

        }

        /// <summary>
        /// sinh ra đối tượng lỗi trả về khi không tìm thấy dữ liệu
        /// </summary>
        /// <returns>Đối tượng lỗi </returns>
        public static ErrorResult? GenerateNotFoundErrorResult()
        {
            var errorResult = new ErrorResult(
                Common.Enums.MISAErrorCode.NotFound,
                ErrorResource.NotFound,
                ErrorResource.NotFoundDev);

            return errorResult;

        }

        public static ErrorResult? GenerateValidateErrorResult(ExceptionService ex)
        {
            var errorResult = new ErrorResult(
                ex.ErrorCode,
                ex.Message,
                ex.Data
                );

            return errorResult;
        }

        public static ErrorResult? GenerateDuplicateErrorResult(ExceptionService ex)
        {
            var errorResult = new ErrorResult(
                ex.ErrorCode,
                ex.Message,
                ex.Data
                );

            return errorResult;
        }
    }
}
