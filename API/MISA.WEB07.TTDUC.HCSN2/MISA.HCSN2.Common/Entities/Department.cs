﻿namespace MISA.HCSN2.Common.Entities
{
    public class Department
    {
        /// <summary>
        /// ID Phòng ban sử dụng
        /// </summary>
        public Guid DepartmentID { get; set; }

        /// <summary>
        /// Mã Phòng ban sử dụng
        /// </summary>
        public String DepartmentCode { get; set; }

        /// <summary>
        /// Tên Phòng ban sử dụng
        /// </summary>
        public String DepartmentName { get; set; }

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
