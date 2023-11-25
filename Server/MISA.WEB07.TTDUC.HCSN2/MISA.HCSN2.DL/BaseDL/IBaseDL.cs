using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.HCSN2.DL
{
    public interface IBaseDL<T>
    {
        /// <summary>
        /// lấy tất cả bản ghi
        /// Author : TTDuc (05/09/2022)
        /// </summary>
        /// <returns>tất cả bản ghi của 1 bảng</returns>
        public IEnumerable<T> GetAllRecords();

        /// <summary>
        /// lấy thông tin bản ghi theo Id truyền vào
        /// Author : TTDuc (05/09/2022)
        /// </summary>
        /// <param name="Id">Id bản ghi muốn lấy</param>
        /// <returns>Thông tin bản ghi tương ứng với Id truyền vào</returns>
        public IEnumerable<dynamic> GetRecordById(Guid Id);


        /// <summary>
        /// xóa 1 bản ghi 
        /// Author : TTDuc (05/09/2022)
        /// </summary>
        /// <param name="Id">Id bản ghi muốn xóa</param>
        /// <returns>Id bản ghi đã xóa</returns>
        public int DeleteRecordById(Guid Id);

        /// <summary>
        /// thêm mới 1 record
        /// Author: TTDuc(05/09/2022)
        /// </summary>
        /// <param name="record">thông tin bản ghi muốn thêm</param>
        /// <returns>Id của bản ghi</returns>
        public Guid InsertOneRecord(T record);

        /// <summary>
        /// sửa 1 record
        /// Author: TTDuc(05/09/2022)
        /// </summary>
        /// <param name="record">thông tin bản ghi muốn sửa</param>
        /// <returns>Id của bản ghi</returns>
        public Guid UpdateOneRecord(T record);

        /// <summary>
        /// kiểm tra trùng mã
        /// Author: TTDuc(18/09/2022)
        /// </summary>
        /// <param name="record">Id bản ghi</param>
        /// <returns>trùng mã hay không</returns>
        public bool CheckDuplicateCode(T record );
        public string GetNewCode();

        public IEnumerable<T> GetRecordsByLstId(string lstId);


    }
}
