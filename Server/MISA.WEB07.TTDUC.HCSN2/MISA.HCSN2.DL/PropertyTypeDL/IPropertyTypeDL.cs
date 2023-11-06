using MISA.HCSN2.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.HCSN2.DL.PropertyTypeDL
{
    public interface IPropertyTypeDL : IBaseDL<PropertyType>
    {
        public bool ValidateDeletePropertyType(Guid propertyTypeId);

        public IEnumerable<PropertyType> GetPropertyTypeWithFilter(string keyword);
    }
}
