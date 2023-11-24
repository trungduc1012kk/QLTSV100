using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.HCSN2.Common.Entities
{
    public class Budget
    {
        /// <summary>
        /// Id nguồn ngân sách
        /// </summary>
        public Guid BudgetId { get; set; }

        /// <summary>
        /// Tên nguồn ngân sách
        /// </summary>
        public string BudgetName { get; set; }

        /// <summary>
        /// Mã nguồn ngân sách
        /// </summary>
        public string BudgetCode { get; set; }

        /// <summary>
        /// Số tiền trong ngân sách
        /// </summary>
        public Decimal Mount { get; set; }
    }
}
