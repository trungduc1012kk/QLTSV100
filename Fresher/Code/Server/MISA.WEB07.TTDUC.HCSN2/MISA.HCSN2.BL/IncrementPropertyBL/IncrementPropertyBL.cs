using MISA.HCSN2.Common;
using MISA.HCSN2.Common.Entities;
using MISA.HCSN2.Common.Enums;
using MISA.HCSN2.Common.Resources;
using MISA.HCSN2.DL;
using MISA.HCSN2.DL.IncrementPropertyDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.HCSN2.BL.IncrementPropertyBL
{
    public class IncrementPropertyBL : BaseBL<IncrementProperty>, IIncrementPropertyBL
    {
        #region Field

        private IIncrementPropertyDL _incrementPropertyDL;

        #endregion

        #region Constructor

        public IncrementPropertyBL(IIncrementPropertyDL incrementPropertyDL) : base(incrementPropertyDL)
        {
            _incrementPropertyDL = incrementPropertyDL;
        }

        #endregion

        #region Method

        /// <summary>
        /// phân trang và tìm kiếm chứng từ
        /// Author: TTDuc(17/10/2022)
        /// </summary>
        /// <param name="keyword">từ khóa tìm kiếm</param>
        /// <param name="pageNumber">số trang</param>
        /// <param name="pageSize">số lượng trong 1 trang</param>
        /// <returns>tổng số bản ghi và danh sách chứng từ</returns>
        public PagingData<IncrementProperty> GetPagingIncrementProperty(string? keyword, int pageNumber, int pageSize)
        {
            return _incrementPropertyDL.GetPagingIncrementProperty(keyword,pageNumber,pageSize);
        }

        /// <summary>
        /// xóa nhiều chứng từ
        /// Author: TTDuc(17/10/2022)
        /// </summary>
        /// <param name="listId">danh sách id chứng từ</param>
        /// <returns>số chứng từ được xóa</returns>
        public int DeleteMultipleIncrementProperty(List<Guid> listId)
        {
            return _incrementPropertyDL.DeleteMultipleIncrementProperty(listId);
        }

        /// <summary>
        /// xử lý validate riêng cho chứng từ
        /// Author: TTDuc(17/10/2022)
        /// </summary>
        /// <param name="incrementProperty"></param>
        /// <exception cref="ExceptionService"></exception>
        public override void ValidateRecord(IncrementProperty incrementProperty)
        {
            var errorsData = new Dictionary<string, object>();

            List<string> errors = new List<string>();
            if (incrementProperty.ListProperty.Count < 1)
            {
                errors.Add(ErrorResource.ListPropertyNotNull);
            }

            if (errors.Count > 0)
            {
                errorsData.Add("ListProperty", errors);
            }

            if (errorsData.Count > 0)
            {
                throw new ExceptionService(ErrorResource.ValidateFail, errorsData, MISAErrorCode.Validate);
            }

            return;
        }

        #endregion
    }
}
