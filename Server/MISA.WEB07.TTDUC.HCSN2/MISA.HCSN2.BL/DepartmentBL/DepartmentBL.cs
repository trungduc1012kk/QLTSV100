using MISA.HCSN2.Common.Entities;
using MISA.HCSN2.DL;
using MISA.HCSN2.DL.DepartmentDL;
using MISA.HCSN2.DL.PropertyDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.HCSN2.BL.DepartmentBL
{
    public class DepartmentBL : BaseBL<Department>, IDepartmentBL
    {

        #region Field

        private IDepartmentDL _departmentDL ;


        #endregion

        #region Constructor

        public DepartmentBL(IDepartmentDL departmentDL) : base(departmentDL)
        {
            _departmentDL = departmentDL;
        }

        #endregion

        #region Method

        public bool ValidateDeleteDepartment(Guid departmentID)
        {

            return _departmentDL.ValidateDeleteDepartment(departmentID);
        }

        public IEnumerable<Department> GetDepartmentWithFilter(string keyword)
        {
            return _departmentDL.GetDepartmentWithFilter(keyword);
        }

        #endregion
    }
}
