using MISA.HCSN2.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.HCSN2.BL
{
    public interface IBaseBL<T>
    {
        /// <summary>
        /// lấy tất cả bản ghi
        /// Author: TTDUC 24/08/2022
        /// </summary>
        /// <returns>tất cả bản ghi của 1 bảng</returns>
        public IEnumerable<T> GetAllRecords();

        /// <summary>
        /// thông tin 1 bản ghi
        /// Author: TTDUC 24/08/2022
        /// </summary>
        /// <returns>thông tin 1 bản ghi</returns>
        public IEnumerable<dynamic> GetRecordById(Guid Id);

        /// <summary>
        /// xóa 1 bản ghi
        /// Author: TTDUC 05/09/2022
        /// </summary>
        /// <returns>Id bản ghi đã xóa</returns>
        public int DeleteRecordById(Guid Id);

        /// <summary>
        /// thêm mới 1 bản ghi
        /// Author : TTDuc (05/09/2022)
        /// </summary>
        /// <param name="record">thông tin bản ghi</param>
        /// <returns>ID bản ghi</returns>
        public Guid InsertOneRecord(T record);

        /// <summary>
        /// sửa 1 bản ghi
        /// Author : TTDuc (05/09/2022)
        /// </summary>
        /// <param name="record">thông tin bản ghi</param>
        /// <returns>ID bản ghi</returns>
        public Guid UpdateOneRecord(T record);
        public string GetNewCode();
    }
}

