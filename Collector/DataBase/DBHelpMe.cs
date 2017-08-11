using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Collector.Helper;
using Collector.Interface;


namespace Collector.DataBase
{
    /// <summary>
    ///DBHelper 的摘要说明
    /// </summary>
    public abstract class DBHelpMe
    {
        private ILog Log;
        public DBHelpMe(ILog iLog, string constr)
        {
            ConnStr = constr;
            Log = iLog;
        }
        //连接数据库字符串。
        public string ConnStr
        {
            get;
            set;

        }

        private DBTypeEnum _dbType;

        public DBTypeEnum DbType
        {
            get { return _dbType; }
            set { _dbType = value; }
        }

        public enum DBTypeEnum
        {
            MySql,
            SqlServer,
            Sqllite,
            Oracle
        }




        public abstract IDbConnection GetConnection();


        public static string BuildConStr(string ip, string db, string username, string password, DBHelpMe.DBTypeEnum _dbType)
        {
            switch (_dbType)
            {
                case DBTypeEnum.MySql:
                    return string.Format("Database={0};Data Source={1};User Id={2};Password={3};pooling=false;CharSet=utf8;port=3306", db, ip, username, password);

                case DBTypeEnum.SqlServer:
                    return string.Format("Initial Catalog={0};Data Source={1};User Id={2};Password={3}", db, ip, username, password);

                case DBTypeEnum.Oracle:
                    return string.Format("User ID={0};Password={1};Data Source=(DESCRIPTION = (ADDRESS_LIST= (ADDRESS = (PROTOCOL = TCP)(HOST = {2})(PORT = {3}))) (CONNECT_DATA = (SERVICE_NAME = {4})))", username, password, ip.Split(',')[0], ip.Split(',')[1], db);
                case DBTypeEnum.Sqllite:
                    return string.Format("Data Source={0};Version=3", db);

                default:
                    return "";

            }
        }


        #region Dapper方法
        //-------------------------------
        /// <summary>
        /// 执行SQL 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int ExecuteSql(string sql, object param, IDbTransaction trans = null)
        {

            try
            {
                using (var con = GetConnection())
                {
                    int i = con.Execute(sql, param, trans);


                    return i;
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.Message, ex);
                return -1;
            }

        }



       

        /// <summary>
        /// 执行sql,返回集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="trans"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public IEnumerable<T> ExecuteSql_All<T>(string sql, object param, IDbTransaction trans = null, int? commandTimeout = null)
        {
            try
            {
                using (var con = GetConnection())
                {
                    IEnumerable<T> result = con.Query<T>(sql, param, trans, true, commandTimeout, CommandType.Text);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.Message, ex);
                return null;
            }
        }



        //------------------------
        /// <summary>
        /// 执行存储过程,无返回结果集  
        /// </summary>
        /// <param name="proName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int ExecuteSP(string proName, object param, IDbTransaction trans = null, int? commandTimeout = null)
        {
            try
            {
                using (var con = GetConnection())
                {
                    int result = con.Execute(proName, param, trans, commandTimeout, CommandType.StoredProcedure);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.Message, ex);
                return -1;
            }
        }

        /// <summary>
        /// 执行存储过程,返回数据实体结果集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="proName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public IList<T> ExecuteSP_ToList<T>(string proName, object param, IDbTransaction trans = null, int? commandTimeout = null)
        {
            try
            {
                using (var con = GetConnection())
                {
                    IEnumerable<T> result = con.Query<T>(proName, param, trans, true, commandTimeout, CommandType.StoredProcedure);
                    return result.ToList<T>();
                }
            }
            catch (Exception ex)
            {
                Log.WriteError(ex.Message, ex);
                return null;
            }
        }

        /// <summary>
        /// 判断表是否在数据库中
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsExsit(string name, DBTypeEnum type)
        {
            switch (type)
            {
                case DBTypeEnum.MySql:
                    break;
                case DBTypeEnum.SqlServer:
                    {
                        string sql = string.Format(@"IF EXISTS  (SELECT  * FROM dbo.SysObjects WHERE ID = object_id(N'{0}') AND OBJECTPROPERTY(ID, 'IsTable') = 1) 
                            select 1 else select 0", name);
                        int result = ExecuteSql_All<int>(sql, null).First();
                        return result == 1;
                    }
                case DBTypeEnum.Sqllite:
                    {
                        string sql = "select count(*)  from sqlite_master where type='table' and name = '@name'";
                        int result = ExecuteSql_All<int>(sql, new { name = name }).First();
                        return result == 1;
                    }
                case DBTypeEnum.Oracle:
                    break;
                default:
                    break;
            }
            return false;
        } 
        #endregion


      

      
        //#region 函数
        //                //方法一 : 直接写sql, 然后用Query调用
        //sql = "select func_test(@id);";
        //var res = conn.Query<int>(sql, new { id = 10 }).FirstOrDefault();  //90
        //Console.WriteLine("Count = " + res); //Count = 90

        ////方法二 : 直接用Query方法, 传入函数名, 参数, 但是注意要把CommondType设置为StoredProcedure
        ////而且调用的时候, 是必须要有 Return参数的, 否则会报错
        //var para = new  DynamicParameters();
        //para.Add("@idIn", 20);
        //para.Add("@res", 0, DbType.Int32, ParameterDirection.ReturnValue);
        //var res1 = conn.Query("func_test", para, null, true, null, CommandType.StoredProcedure).FirstOrDefault();  //0
        //Console.WriteLine("Query @res = " + para.Get<int>("@res"));  //Query @res = 80

        ////方法三 : 使用Execute方法也是可以的, 要注意加一个返回参数
        //var param = new DynamicParameters();
        //param.Add("@idIn", 25);
        //param.Add("@res", 0, DbType.Int32, ParameterDirection.ReturnValue);
        //var res2 = conn.Execute("func_test", param, null, null, CommandType.StoredProcedure);  //0
        //Console.WriteLine("Execute @res = " + param.Get<int>("@res"));  //Execute @res = 75 
        //    #endregion

        //#region 存储过程
        //        //方法一
        //sql = "call pro_test(@id);";
        //var res = conn.Query<int>(sql, new { id = 15 }).FirstOrDefault();  //85
        //Console.WriteLine("res = " + res);  //res = 85

        ////方法二
        //var param = new DynamicParameters();
        //param.Add("@idIn", 20);
        //param.Add("@count", 0, DbType.Int32, ParameterDirection.Output);
        //var res2 = conn.Query<Tch_Teacher>("pro_test1", param, null, true, null, CommandType.StoredProcedure);//res2.Count = 80
        //Console.WriteLine("Query count = " + param.Get<object>("@count"));   //Query count = 80

        ////方法三
        //var res3 = conn.Execute("pro_test1", param, null, null, CommandType.StoredProcedure); //0
        //Console.WriteLine("Execute count = " + param.Get<object>("@count"));  //Execute count = 80 
        //    #endregion

    }
}
