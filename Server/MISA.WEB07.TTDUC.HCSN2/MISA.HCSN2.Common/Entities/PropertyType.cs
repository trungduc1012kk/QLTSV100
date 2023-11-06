using MISA.HCSN2.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MISA.HCSN2.Common.Entities
{
    public class PropertyType
    {
        /// <summary>
        /// ID loại tài sản
        /// </summary>
        [Key]
        public Guid PropertyTypeID { get; set; }

        /// <summary>
        /// Mã loại tài sản
        /// </summary>
        [Required]
        [DuplicateAttribute]
        public String PropertyTypeCode { get; set; }

        /// <summary>
        /// tên loại tài sản
        /// </summary>
        [Required]
        public String PropertyTypeName { get; set; }

        /// <summary>
        /// Tỷ lệ hao mòn mặc định
        /// </summary>
        [Required]
        public double AttritionRateDefault { get; set; }

        /// <summary>
        /// Số năm sử dụng mặc định
        /// </summary>
        [Required]
        public int UsedYearDefault { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// Ngày tạo 
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public String CreatedBy { get; set; }

        /// <summary>
        /// Ngày sửa gần nhất
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Người sửa gần nhất
        /// </summary>
        public String ModifiedBy { get; set; }
    }
}
