using MISA.HCSN2.Common.Entities;
using MISA.HCSN2.DL.AuthBL;
using MISA.HCSN2.DL.BudgetDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.HCSN2.BL.AuthBL
{
    public class AuthBL : IAuthBL
    {
        private IAuthDL _authDL;
        public AuthBL(IAuthDL authDL)
        {
            _authDL = authDL;
        }

        public bool CheckAuth(User user)
        {
            return _authDL.CheckAuth(user);
        }
    }
}
