using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Service
{
    public class DbService
    {
        protected readonly SqlSugarClient dbEnjoy = new SqlSugarClient(new ConnectionConfig()
        {
            ConnectionString = ConfigurationManager.AppSettings["ConnStringSQL"].ToString(),//必填, 数据库连接字符串
            DbType = DbType.SqlServer,         //必填, 数据库类型
            IsAutoCloseConnection = true
        });

        /// <summary>
        /// 每次调用都会生成最新的实例
        /// </summary>
        public static SqlSugarClient Instance
        {
            get
            {
                try
                {
                    return new SqlSugarClient(new ConnectionConfig()
                    {
                        ConnectionString = ConfigurationManager.AppSettings["ConnStringSQL"].ToString(),
                        DbType = DbType.SqlServer,
                        IsAutoCloseConnection = true,
                        InitKeyType = InitKeyType.SystemTable
                    });
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
        }
    }
}
