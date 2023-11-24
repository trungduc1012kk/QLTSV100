using Microsoft.AspNetCore.Http;
using MISA.HCSN2.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.HCSN2.BL.FileBL
{
    public interface IFileBL
    {
        public byte[] ExportFileExcel();


        /// <summary>
        /// nhập khẩu excel
        /// Author : TTDuc (10/09/2022)
        /// </summary>
        /// <param name="file">File nhập khẩu</param>
        /// <returns>danh sách Id inport thành công và số bản ghi lỗi</returns>
        public ImportResult<Guid> ImportFileExcel(IFormFile file);
    }
}
