using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.HCSN2.API.NTier.BaseControllers;
using MISA.HCSN2.BL.PropertyTypeBL;
using MISA.HCSN2.Common.Entities;

namespace MISA.HCSN2.API.NTier.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PropertyTypesController : BaseControllers<PropertyType>
    {
        #region Field

        private IPropertyTypeBL _propertyTypeBL;

        #endregion

        #region Constructor

        public PropertyTypesController(IPropertyTypeBL propertyTypeBL) : base(propertyTypeBL)
        {
            _propertyTypeBL = propertyTypeBL;
        }

        #endregion

    }
}
