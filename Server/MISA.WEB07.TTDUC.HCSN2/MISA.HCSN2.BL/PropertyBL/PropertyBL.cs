using Microsoft.AspNetCore.Http;
using MISA.HCSN2.Common.Entities;
using MISA.HCSN2.DL.PropertyDL;
using OfficeOpenXml;

namespace MISA.HCSN2.BL.PropertyBL
{

    public class PropertyBL : BaseBL<Property>, IPropertyBL
    {

        #region Field

        private IPropertyDL _propertyDL;

        #endregion

        #region Constructor

        public PropertyBL(IPropertyDL propertyDL) : base(propertyDL)
        {
            _propertyDL = propertyDL;
        }

        #endregion

        #region Method

        /// <summary>
        /// hàm lấy danh sách và tổng số bản ghi đã được lọc
        /// Author: TTDuc (06/09/2022)
        /// </summary>
        /// <returns>danh sách tài sản và tổng số bản ghi</returns>
        public PagingData<Property> GetPagingData(List<Guid>? listID,string? keyword, Guid? departmentId, Guid? propertyTypeId, int? status, int PageNumber, int pageSize)
        {
            return _propertyDL.GetPagingData(listID, keyword, departmentId, propertyTypeId, status, PageNumber, pageSize);
        }

        /// <summary>
        /// hàm xóa nhiều tài sản 
        /// Author: TTDuc (06/09/2022)
        /// </summary>
        /// <returns>số tài sản bị xóa</returns>
        public int DeleteMultipleProperty(List<Guid> listId)
        {
            return _propertyDL.DeleteMultipleProperty(listId);
        }

        /// <summary>
        /// ghi tăng tài sản 
        /// Author : TTDuc(10/10/2022)
        /// </summary>
        /// <returns>số bản ghi đã được ghi tăng</returns>
        public int IncreaseProperty(List<Property> properties, Guid incrementID)
        {
            int count = 0;

            foreach (var property in properties)
            {
                var id = _propertyDL.IncreaseProperty(property.PropertyID, incrementID, property.Budget);

                if(id != Guid.Empty)
                {
                    count++;
                }
            }   

            return count;
        }

        /// <summary>
        /// xóa nhiều tài sản trong danh sách ghi tăng
        /// </summary>
        /// <param name="properties">dnah sách tài sản được ghi tăng</param>
        /// <param name="incrementID">ID của chứng từ ghi tăng</param>
        /// <returns>số bản ghi được xóa</returns>
        public int DecreaseProperty(List<Property> properties, Guid incrementID)
        {
            int count = 0;

            foreach (var property in properties)
            {
                var id = _propertyDL.DecreaseProperty(property.PropertyID, incrementID);

                if (id != Guid.Empty)
                {
                    count++;
                }
            }

            return count;
        }

        #endregion
    }
}
