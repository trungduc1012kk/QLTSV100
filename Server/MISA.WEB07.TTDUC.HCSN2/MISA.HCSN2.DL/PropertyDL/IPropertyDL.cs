using MISA.HCSN2.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.HCSN2.DL.PropertyDL
{
    public interface IPropertyDL : IBaseDL<Property>
    {
        /// <summary>
        /// hàm lấy danh sách và tổng số bản ghi đã được lọc
        /// Author: TTDuc (06/09/2022)
        /// </summary>
        /// <returns>danh sách tài sản và tổng số bản ghi</returns>
        public PagingData<Property> GetPagingData(List<Guid>? listID, string? keyword, Guid? departmentId, Guid? propertyTypeId, int? status, int PageNumber, int pageSize);

        /// <summary>
        /// hàm xóa nhiều tài sản 
        /// Author: TTDuc (06/09/2022)
        /// </summary>
        /// <returns>số tài sản bị xóa</returns>
        public int DeleteMultipleProperty(List<Guid> listId);


        /// <summary>
        /// ghi tăng tài sản 
        /// Author: TTDuc ( 10/10/2022)
        /// </summary>
        /// <param name="propertyID">ID tài sản </param>
        /// <param name="increamentID">ID chứng từ ghi tăng </param>
        /// <param name="budget">nguồn ngân sách </param>
        /// <returns>ID tài sản đã được ghi tăng</returns>
        public Guid IncreaseProperty(Guid propertyID, Guid increamentID, string budget);

        /// <summary>
        /// xóa nhiều tài sản trong danh sách ghi tăng
        /// </summary>
        /// <param name="propertyID">ID tài sản</param>
        /// <param name="increamentID">ID ghi tăng</param>
        /// <returns>ID tài sản đã được xóa</returns>
        public Guid DecreaseProperty(Guid propertyID, Guid increamentID);
    }
}
