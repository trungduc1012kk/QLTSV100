using MISA.HCSN2.Common.Entities;
using MISA.HCSN2.DL;
using MISA.HCSN2.DL.BudgetDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.HCSN2.BL.BudgetBL
{
    public class BudgetBL : BaseBL<Budget>, IBudgetBL
    {
        private IBudgetDL _budgetDL;
        public BudgetBL(IBudgetDL budgetDL) : base(budgetDL)
        {
            _budgetDL = budgetDL;
        }
    }
}
