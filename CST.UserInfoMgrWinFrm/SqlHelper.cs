using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace CST.UserInfoMgrWinFrm
{
    class SqlHelper
    {
        ///#region get connection string
        /// <summary>
        /// get SQL connection string
        /// </summary>
        /// <returns></returns>
        //public static string GetConnectionString()
        //{
        //    return ConfigurationManager.ConnectionStrings["sql"].ConnectionString;
        //}
        //#endregion

        private static readonly string connStr = ConfigurationManager.ConnectionStrings["sql"].ConnectionString;


        #region Sql Adapter get Table
        /// <summary>
        /// execute Sql script, get Table by adapter
        /// </summary>
        /// <param name="sqlText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(string sqlText, CommandType type, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {

                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlText, conn))

                {
                    DataTable dt = new DataTable();
                    adapter.SelectCommand.CommandType = type;
                    adapter.SelectCommand.Parameters.AddRange(parameters);
                    adapter.Fill(dt);
                    return dt;

                }
            }



        }
        #endregion

        #region Execeute Noquery method
        public static int ExecuteNoQuery(string sql, CommandType type, params SqlParameter[] pars)
        {

            using (SqlConnection conn = new SqlConnection(connStr))

            using (SqlCommand cmd=new SqlCommand(sql,conn))
            {
                if (pars != null)
                {
                    cmd.Parameters.AddRange(pars);
                        
                 }

                cmd.CommandType = type;
                conn.Open();
                return cmd.ExecuteNonQuery();

            
            }

        }

        #endregion








        //


    }
}
