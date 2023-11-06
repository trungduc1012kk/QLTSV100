using MISA.HCSN2.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.HCSN2.BL.PropertyTypeBL
{
    public interface IPropertyTypeBL : IBaseBL<PropertyType>
    {
        public bool ValidateDeletePropertyType(Guid propertyTypeId);

        public IEnumerable<PropertyType> GetPropertyTypeWithFilter(string keyword);

    }
}
