using MISA.HCSN2.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.HCSN2.BL.IncrementPropertyBL
{
    public interface IIncrementPropertyBL : IBaseBL<IncrementProperty>
    {
        /// <summary>
        /// phân trang và tìm kiếm chứng từ
        /// </summary>
        /// <param name="keyword">từ khóa tìm kiếm</param>
        /// <param name="pageNumber">số trang</param>
        /// <param name="pageSize">số lượng trong 1 trang</param>
        /// <returns>tổng số bản ghi và danh sách chứng từ</returns>
        public PagingData<IncrementProperty> GetPagingIncrementProperty(string keyword, int pageNumber, int pageSize);

        /// <summary>
        /// hàm xóa chứng từ ghi tăng 
        /// Author: TTDuc (17/10/2022)
        /// </summary>
        /// <returns>số tài sản bị xóa</returns>
        int DeleteMultipleIncrementProperty(List<Guid> listId);
    }
}
