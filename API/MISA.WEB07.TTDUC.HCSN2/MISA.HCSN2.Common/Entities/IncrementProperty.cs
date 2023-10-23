using MISA.HCSN2.Common.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.HCSN2.Common.Entities
{
    /// <summary>
    /// bảng ghi tăng tài sản
    /// Author: TTDuc(10/10/2022)
    /// </summary>
    [Table("increment_property")]
    public class IncrementProperty
    {
        /// <summary>
        /// ID ghi tăng
        /// </summary>
        [Key]
        public Guid IncrementPropertyID { get; set; }

        /// <summary>
        /// Mã ghi tăng
        /// </summary>
        [Required]
        [DuplicateAttribute]
        public string IncrementPropertyCode { get; set; }

        /// <summary>
        /// ngày chứng từ 
        /// </summary>
        public DateTime VoucherDate { get; set; }


        /// <summary>
        /// ngày ghi tăng
        /// </summary>
        public DateTime IncrementDate { get; set; }

        /// <summary>
        /// ghi chú
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// ngày sửa gần nhất
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// người sửa gần nhất
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// ngày tạo
        /// </summary>
        public DateTime CreatedDate { get; set; }


        /// <summary>
        /// người tạo
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// danh sách ID tài sản thuộc chứng từ
        /// </summary>
        public List<Property> ListProperty { get; set; }

        /// <summary>
        /// Tổng ngân sách 
        /// </summary>
        public decimal Budget { get; set; }
    }
}
