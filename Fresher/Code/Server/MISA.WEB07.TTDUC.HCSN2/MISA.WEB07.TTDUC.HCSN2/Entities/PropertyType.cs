namespace MISA.WEB07.TTDUC.HCSN2.Entities
{
    public class PropertyType
    {
        /// <summary>
        /// ID loại tài sản
        /// </summary>
        public Guid PropertyTypeID { get; set; }

        /// <summary>
        /// Mã loại tài sản
        /// </summary>
        public String PropertyTypeCode { get; set; }

        /// <summary>
        /// tên loại tài sản
        /// </summary>
        public String PropertyTypeName { get; set; }

        /// <summary>
        /// Tỷ lệ hao mòn mặc định
        /// </summary>
        public double AttritionRateDefault { get; set; }

        /// <summary>
        /// Số năm sử dụng mặc định
        /// </summary>
        public int UsedYearDefault { get; set; }


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
