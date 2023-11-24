using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.HCSN2.Common.Entities
{
    /// <summary>
    /// Dữ liệu trả về khi import excel
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của đối tượng trong mảng trả về</typeparam>
    public class ImportResult<T>
    {
        /// <summary>
        /// danh sách ID đã thêm thành công
        /// </summary>
        public List<T> listId { get; set; } = new List<T>();

        /// <summary>
        /// tổng số bản ghi bị trùng mã 
        /// </summary>
        public int totalCountError { get; set; }
    }
}
