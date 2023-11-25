using MISA.HCSN2.Common.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MISA.HCSN2.Common.Entities
{
    public class PropertyExport
    {

        /// <summary>
        /// Mã tài sản
        /// </summary>
        public string PropertyCode { get; set; }

        /// <summary>
        /// Tên tài sản
        /// </summary>
        [Required]
        public string PropertyName { get; set; }



        /// <summary>
        /// Mã Phòng ban sử dụng
        /// </summary>
        [Required]
        public string DepartmentCode { get; set; }

        /// <summary>
        /// Tên Phòng ban sử dụng
        /// </summary>
        [Required]
        public string DepartmentName { get; set; }

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
    }
}
