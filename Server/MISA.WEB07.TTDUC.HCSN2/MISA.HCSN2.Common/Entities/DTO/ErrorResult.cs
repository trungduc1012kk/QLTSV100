using MISA.HCSN2.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.HCSN2.Common.Entities.DTO
{
    public class ErrorResult
    {
        #region property
        /// <summary>
        /// định danh mã lỗi nội bộ
        /// </summary>
        public MISAErrorCode ErrorCode { get; set; } = MISAErrorCode.Exception;

        /// <summary>
        /// thông báo lỗi 
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// thông tin chi tiết hơn về lỗi
        /// </summary>
        public object? Data { get; set; }

        /// <summary>
        /// mã tra cứu lỗi 
        /// </summary>
        //public string? TraceId { get; set; }
        #endregion

        #region Constructor

        public ErrorResult()
        {
        }

        public ErrorResult(MISAErrorCode errorCode, string? mesage, object? data)
        {
            ErrorCode = errorCode;
            Message = mesage;
            Data = data;
        }

        #endregion
    }
}
