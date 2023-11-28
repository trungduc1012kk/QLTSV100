using MISA.HCSN2.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.HCSN2.BL.AuthBL
{
    public interface IAuthBL
    {
        public bool CheckAuth(User user);
    }
}
