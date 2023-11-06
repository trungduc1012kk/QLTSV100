using MISA.HCSN2.Common.Entities;
using MISA.HCSN2.DL.PropertyTypeDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.HCSN2.BL.PropertyTypeBL
{
    public class PropertyTypeBL : BaseBL<PropertyType>, IPropertyTypeBL
    {
        #region Field 

        private IPropertyTypeDL _propertyTypeDL;

        #endregion

        #region Constructor

        public PropertyTypeBL(IPropertyTypeDL propertyTypeDL) : base(propertyTypeDL)
        {
            _propertyTypeDL = propertyTypeDL;
        }

        #endregion

        #region Method

        public bool ValidateDeletePropertyType(Guid propertyTypeId)
        {

            return _propertyTypeDL.ValidateDeletePropertyType(propertyTypeId);
        }

        public IEnumerable<PropertyType> GetPropertyTypeWithFilter(string keyword)
        {
            return _propertyTypeDL.GetPropertyTypeWithFilter(keyword);
        }

        #endregion
    }
}
