using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MISA.WEB07.TTDUC.HCSN2.Entities
{
    [Table("proprety")]
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
        [Required(ErrorMessage = "e004")]
        public string PropertyCode { get; set; }

        /// <summary>
        /// Tên tài sản
        /// </summary>
        public String PropertyName { get; set; }

        /// <summary>
        /// ID Phòng ban sử dụng
        /// </summary>
        public Guid DepartmentID { get; set; }

        /// <summary>
        /// Tên Phòng ban sử dụng
        /// </summary>
        public String DepartmentName { get; set; }

        /// <summary>
        /// Mã Phòng ban sử dụng
        /// </summary>
        public String DepartmentCode { get; set; }

        /// <summary>
        /// ID Loại tài sản
        /// </summary>
        public Guid PropertyTypeID { get; set; }

        /// <summary>
        /// Mã loai tài sản
        /// </summary>
        public String PropertyTypeCode { get; set; }

        /// <summary>
        /// Tên Loại tài sản
        /// </summary>
        public String PropertyTypeName { get; set; }

        /// <summary>
        /// Số lượng
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Nguyên giá
        /// </summary>
        public double MarkedPrice { get; set; }

        /// <summary>
        /// Số năm sử dụng
        /// </summary>
        public int UsedYear { get; set; }

        /// <summary>
        /// Tỷ lệ hao mòn
        /// </summary>
        public double AttritionRate { get; set; }

        /// <summary>
        /// Giá trị hao mòn
        /// </summary>
        public Double AttritionValue { get; set; }

        /// <summary>
        /// Năm theo dõi
        /// </summary>
        public int TrackingYear { get; set; }

        /// <summary>
        /// Ngày mua 
        /// </summary>
        public DateTime PurchasingDate { get; set; }

        /// <summary>
        /// Ngày bắt đầu sử dụng
        /// </summary>
        public DateTime StartUsingDate { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public String CreatedBy { get; set; }

        /// <summary>
        /// Ngày Sửa gần nhất
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Người sửa gần nhất
        /// </summary>
        public String ModifiedBy { get; set; }
    }
}
