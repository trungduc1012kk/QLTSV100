using MISA.HCSN2.Common.Entities;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.HCSN2.DL.IncrementPropertyDL
{
    public interface IIncrementPropertyDL : IBaseDL<IncrementProperty>
    {
        /// <summary>
        /// phân trang và tìm kiếm chứng từ
        /// Author: TTDuc(17/10/2022)
        /// </summary>
        /// <param name="keyword">từ khóa lọc theo số chứng từ </param>
        /// <param name="offset">vị trí bắt đầu lấy</param>
        /// <param name="limit">số lượng trong 1 trang</param>
        /// <returns>trả về tổng số bản ghi và danh sách</returns>
        public PagingData<IncrementProperty> GetPagingIncrementProperty(string? keyword, int pageNumber, int pageSize);

        /// <summary>
        /// xóa nhiều ghi tăng
        /// </summary>
        /// <param name="listId">danh sách ID muốn xóa</param>
        /// <returns></returns>
        public int DeleteMultipleIncrementProperty(List<Guid> listId);
    }
}
