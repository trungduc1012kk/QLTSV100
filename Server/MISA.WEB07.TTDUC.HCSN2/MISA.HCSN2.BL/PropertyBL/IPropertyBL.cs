using Microsoft.AspNetCore.Http;
using MISA.HCSN2.Common.Entities;

namespace MISA.HCSN2.BL.PropertyBL
{
    public interface IPropertyBL : IBaseBL<Property>
    {
        #region Method

        /// <summary>
        /// hàm lấy danh sách và tổng số bản ghi đã được lọc
        /// Author: TTDuc (06/09/2022)
        /// </summary>
        /// <returns>danh sách tài sản và tổng số bản ghi</returns>
        public PagingData<Property> GetPagingData(List<Guid>? listId,string? keyword, Guid? departmentId, Guid? propertyTypeId, int? status, int PageNumber, int pageSize);

        /// <summary>
        /// hàm xóa nhiều tài sản 
        /// Author: TTDuc (06/09/2022)
        /// </summary>
        /// <returns>số tài sản bị xóa</returns>
        public int DeleteMultipleProperty(List<Guid> listId);

        /// <summary>
        /// xóa nhiều tài sản trong danh sách ghi tăng
        /// Author : TTDuc(10/10/2022)
        /// </summary>
        /// <returns>số bản ghi đã được ghi tăng</returns>
        public int IncreaseProperty(List<Property> properties, Guid incrementID);

        /// <summary>
        /// xóa tài sản khỏi danh sách ghi tăng khi xóa ghi tăng
        /// </summary>
        /// <param name="properties">dnah sách tài sản được ghi tăng</param>
        /// <param name="incrementID">ID của chứng từ ghi tăng</param>
        /// <returns>số bản ghi được xóa</returns>
        public int DecreaseProperty(List<Property> properties, Guid incrementID);


        #endregion
    }
}
