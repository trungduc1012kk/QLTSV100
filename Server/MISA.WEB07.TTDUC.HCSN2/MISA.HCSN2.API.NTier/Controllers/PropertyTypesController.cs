using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.HCSN2.API.NTier.BaseControllers;
using MISA.HCSN2.API.NTier.Helper;
using MISA.HCSN2.BL.PropertyTypeBL;
using MISA.HCSN2.Common.Entities;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using System.Collections.Generic;
using System.Drawing.Printing;

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

        #region Method

        [HttpGet("validate-delete-property-type")]
        public IActionResult ValidateDeletePropertyType(Guid propertyTypeId)
        {
            try
            {
                var isValid = _propertyTypeBL.ValidateDeletePropertyType(propertyTypeId);
                return StatusCode(StatusCodes.Status200OK, isValid);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateExceptionResult(e));
            }
        }

        [HttpGet("filter")]
        public IActionResult GetPropertyTypeWithFilter(string keyword)
        {
            try
            {
                var res = _propertyTypeBL.GetPropertyTypeWithFilter(keyword);
                return StatusCode(StatusCodes.Status200OK, res);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateExceptionResult(e));
            }
        }
        #endregion
    }
}
