using Dapper;
using MISA.HCSN2.Common.Entities;
using MISA.HCSN2.Common.Utilities;
using MISA.HCSN2.DL.AuthBL;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.HCSN2.DL.AuthDL
{
    public class AuthDL : IAuthDL
    {
        public AuthDL() { }

        public bool CheckAuth(User user)
        {
            var isAuth = false;
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                //tạo câu lệnh truy vấn 
                string query = $"SELECT * FROM User where UserName = '{user.UserName}' AND Password = '{user.Password}';";

                // thực hiện chạy câu lệnh 
                var records = mySqlConnection.Query<User>(query);

                if(records != null && records.Any())
                {
                    isAuth = true;
                }
            }

            return isAuth;
        }
    }
}
