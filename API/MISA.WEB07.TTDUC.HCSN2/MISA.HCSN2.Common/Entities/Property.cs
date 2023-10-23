using MISA.HCSN2.Common.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MISA.HCSN2.Common.Entities
{
    [Table("property")]
    [Prefix("TS")]
    public class Property
    {

        /// <summary>
        /// ID tài sản
        /// </summary>
        [Key]
        public Guid PropertyID { get; set; }

        /// <summary>
        /// Mã tài sản
        /// </summary>
        [Required]
        [StringLength(20,MinimumLength = 7)]
        [DuplicateAttribute]
        public string PropertyCode { get; set; }

        /// <summary>
        /// Tên tài sản
        /// </summary>
        [Required]
        public string PropertyName { get; set; }

        /// <summary>
        /// ID Phòng ban sử dụng
        /// </summary>
        [Required] 
        public Guid DepartmentID { get; set; }

        /// <summary>
        /// Tên Phòng ban sử dụng
        /// </summary>
        [Required]
        public string DepartmentName { get; set; }

        /// <summary>
        /// Mã Phòng ban sử dụng
        /// </summary>
        [Required]
        public string DepartmentCode { get; set; }

        /// <summary>
        /// ID Loại tài sản
        /// </summary>
        [Required]
        public Guid PropertyTypeID { get; set; }

        /// <summary>
        /// Mã loai tài sản
        /// </summary>
        [Required]
        public string PropertyTypeCode { get; set; }

        /// <summary>
        /// Tên Loại tài sản
        /// </summary>
        [Required]
        public string PropertyTypeName { get; set; }

        /// <summary>
        /// Số lượng
        /// </summary>
        [Required]
        [Range(0,1000)]
        public int Quantity { get; set; }

        /// <summary>
        /// Nguyên giá
        /// </summary>
        [Required]
        [Range(0, 10000000000)]
        public double MarkedPrice { get; set; }

        /// <summary>
        /// Số năm sử dụng
        /// </summary>
        [Required]
        [Range(0, 100)]
        public int UsedYear { get; set; }

        /// <summary>
        /// Tỷ lệ hao mòn
        /// </summary>
        [Required]
        [Range(0, 100)]
        public double AttritionRate { get; set; }

        /// <summary>
        /// Giá trị hao mòn
        /// </summary>
        [Required]
        [Range(0, 10000000000)]
        public double AttritionValue { get; set; }

        /// <summary>
        /// Năm theo dõi
        /// </summary>
        [Required]
        public int TrackingYear { get; set; }

        /// <summary>
        /// Ngày mua 
        /// </summary>
        [Required]
        public DateTime PurchasingDate { get; set; }

        /// <summary>
        /// Ngày bắt đầu sử dụng
        /// </summary>
        [Required]
        public DateTime StartUsingDate { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Ngày Sửa gần nhất
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Người sửa gần nhất
        /// </summary>
        public string? ModifiedBy { get; set; }

        /// <summary>
        /// ID tài sản đã ghi tăng
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// nguồn ngân sách 
        /// </summary>
        public string Budget { get; set; }
    }
}
