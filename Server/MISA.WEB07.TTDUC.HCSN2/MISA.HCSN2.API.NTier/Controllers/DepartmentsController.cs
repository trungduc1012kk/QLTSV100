using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.HCSN2.API.NTier.BaseControllers;
using MISA.HCSN2.API.NTier.Helper;
using MISA.HCSN2.BL;
using MISA.HCSN2.BL.DepartmentBL;
using MISA.HCSN2.Common.Entities;

namespace MISA.HCSN2.API.NTier.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentsController : BaseControllers<Department>
    {

        #region Feild

        private IDepartmentBL _departmentBL;

        #endregion

        #region Constructor

        public DepartmentsController(IDepartmentBL departmentBL) : base(departmentBL)
        {
            _departmentBL = departmentBL;
        }

        #endregion

        #region Method

        [HttpGet("validate-delete-department")]
        public IActionResult ValidateDeleteDepartment(Guid departmentID)
        {
            try
            {
                var isValid = _departmentBL.ValidateDeleteDepartment(departmentID);
                return StatusCode(StatusCodes.Status200OK, isValid);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, HandleError.GenerateExceptionResult(e));
            }
        }

        [HttpGet("filter")]
        public IActionResult GetDepartmentWithFilter(string keyword)
        {
            try
            {
                var res = _departmentBL.GetDepartmentWithFilter(keyword);
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
