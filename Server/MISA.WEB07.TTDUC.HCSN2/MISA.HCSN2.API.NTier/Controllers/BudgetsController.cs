using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.HCSN2.API.NTier.BaseControllers;
using MISA.HCSN2.BL.BudgetBL;
using MISA.HCSN2.Common.Entities;

namespace MISA.HCSN2.API.NTier.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BudgetsController : BaseControllers<Budget>
    {
        #region Field

        private IBudgetBL _budgetBL;

        #endregion

        #region Constructor

        public BudgetsController(IBudgetBL budgetBL) : base(budgetBL)
        {
            _budgetBL = budgetBL;
        }
        
        #endregion
    }
}
