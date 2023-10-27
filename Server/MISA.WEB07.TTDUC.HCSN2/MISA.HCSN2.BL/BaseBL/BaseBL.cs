using MISA.HCSN2.Common;
using MISA.HCSN2.Common.Entities;
using MISA.HCSN2.Common.Enums;
using MISA.HCSN2.Common.Resources;
using MISA.HCSN2.DL;
using MISA.HCSN2.DL.IncrementPropertyDL;
using MISA.HCSN2.DL.PropertyDL;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.HCSN2.BL
{
    public class BaseBL<T> : IBaseBL<T>
    {
        #region Field

        private IBaseDL<T> _baseDL;

        #endregion


        #region Constructor

        public BaseBL(IBaseDL<T> baseDL)
        {
            _baseDL = baseDL;
        }

        #endregion


        #region Method

        /// <summary>
        /// lấy tất cả bản ghi
        /// Author: TTDUC 24/08/2022
        /// </summary>
        /// <returns>tất cả bản ghi của 1 bảng</returns>
        public virtual IEnumerable<T> GetAllRecords()
        {

            return _baseDL.GetAllRecords();
        }

        /// <summary>
        /// lấy thông tin bản ghi theo Id truyền vào
        /// Author: TTDUC 24/08/2022
        /// </summary>
        /// <returns>thông tin 1 bản ghi</returns>
        public virtual IEnumerable<dynamic> GetRecordById(Guid Id)
        {
            return _baseDL.GetRecordById(Id);
        }

        /// <summary>
        /// lấy thông tin bản ghi theo Id truyền vào
        /// Author: TTDuc(06/09/2022)
        /// </summary>
        /// <returns>thông tin 1 bản ghi</returns>
        public virtual int DeleteRecordById(Guid Id)
        {
            return _baseDL.DeleteRecordById(Id);
        }

        /// <summary>
        /// thêm mới 1 bản ghi
        /// Author : TTDuc (06/09/2022)
        /// </summary>
        /// <param name="record">thông tin bản ghi</param>
        /// <returns>ID bản ghi</returns>
        public virtual Guid InsertOneRecord(T record)
        {

            Validate(record);
            ValidateRecord(record);

            return _baseDL.InsertOneRecord(record);
        }

        /// <summary>
        /// sửa 1 bản ghi
        /// Author : TTDuc (06/09/2022)
        /// </summary>
        /// <param name="record">thông tin bản ghi</param>
        /// <returns>ID bản ghi</returns>
        public virtual Guid UpdateOneRecord(T record)
        {

            Validate(record);
            ValidateRecord(record);

            return _baseDL.UpdateOneRecord(record);
        }

        /// <summary>
        /// validate chung
        /// Author : TTDuc (19/09/2022)
        /// </summary>
        /// <param name="record">thông tin bản ghi</param>
        public virtual void Validate(T record)
        {

            // lấy danh sách property
            string className = typeof(T).Name;
            var properties = typeof(T).GetProperties();

            // tạo dictionerry để lưu lại lỗi
            var errorsData = new Dictionary<string, object>();

            // duyệt để validate theo từng property
            foreach (var property in properties)
            {
                var errorMsg = new List<String>(); // tạo biến để lưu lại lỗi theo từng property

                var value = (property.GetValue(record, null) ?? string.Empty).ToString(); // lấy giá trị property

                var attrs = property.GetCustomAttributes(true); // lấy dánh sách attribute

                foreach (object attr in attrs)
                {
                    var attrName = attr.GetType().Name; // lấy tên attr 

                    // check null
                    if (attrName == "RequiredAttribute")
                    {
                        if (string.IsNullOrEmpty(value))
                        {
                            errorMsg.Add($"{property.Name} {ErrorResource.Required}");

                        }
                    }
                    // check độ dài
                    if (attrName == "StringLengthAttribute" && !string.IsNullOrEmpty(value))
                    {
                        int max = Int32.Parse(attr.GetType().GetProperty("MaximumLength").GetValue(attr, null).ToString());
                        int min = Int32.Parse(attr.GetType().GetProperty("MinimumLength").GetValue(attr, null).ToString());

                        if (value.Length > max || value.Length < min)
                        {
                            errorMsg.Add($"{property.Name} {ErrorResource.StringLength} {min} đến {max}!");
                        }

                    }
                    // check giới hạn lớn nhất hoặc nhỏ nhất
                    if (attrName == "RangeAttribute" && !string.IsNullOrEmpty(value))
                    {
                        var max = float.Parse(attr.GetType().GetProperty("Maximum").GetValue(attr, null).ToString());
                        var min = float.Parse(attr.GetType().GetProperty("Minimum").GetValue(attr, null).ToString());
                        var valueToInt = float.Parse(value);

                        if (valueToInt > max || valueToInt <= min)
                        {
                            errorMsg.Add($"{property.Name} {ErrorResource.Range} {min} đến {max}!");
                        }

                    }
                    // check trùng mã 
                    if (attrName == "DuplicateAttribute" && !string.IsNullOrEmpty(value))
                    {

                        if (!_baseDL.CheckDuplicateCode(record))
                        {
                            errorMsg.Add($"{property.Name} {ErrorResource.DuplicateKey}");
                            errorsData.Add($"{className}Code", errorMsg);
                            throw new ExceptionService(ErrorResource.ValidateFail, errorsData, MISAErrorCode.DuplicateCode);
                        }
                    }

                }


                if (errorMsg.Count > 0)
                {
                    errorsData.Add(property.Name, errorMsg);
                }
            }

            if (errorsData.Count > 0)
            {
                throw new ExceptionService(ErrorResource.ValidateFail, errorsData, MISAErrorCode.Validate);
            }

        }

        public string GetNewCode()
        {
            return _baseDL.GetNewCode();
        }

        /// <summary>
        /// hàm xử lý validate riêng cho từng đối tượng
        /// Author : TTDuc (19/09/2022)
        /// </summary>
        /// <param name="record"></param>
        public virtual void ValidateRecord(T record)
        {
        }

        #endregion
    }
}
