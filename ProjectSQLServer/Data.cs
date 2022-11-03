using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data;

namespace ProjectSQLServer
{
    public class Data
    {
        private static SqlConnection con;
        private static SqlCommand command;
        private static SqlTransaction dataTransaction;
        private static SqlDataReader dataReader;

        public static void Connect()
        {
            con = new SqlConnection("data source=.;Database=PROJECTSql;user id=sa;password=123456;");
            con.Open();
        }

        public static void Disconnect()
        {
            con.Close();
        }

        public static void BeginTrans()
        {
            dataTransaction = con.BeginTransaction();
        }

        public static void CommitTrans()
        {
            dataTransaction.Commit();
        }

        public static void RollbackTrans()
        {
            dataTransaction.Rollback();
        }

        public static void Command(string sqlCommand, object[] paramValue)
        {
            using (command = new SqlCommand(sqlCommand, con, dataTransaction))
            {
                if(paramValue!=null)
                {
                    for (int x=0; x<paramValue.Length;x++)
                    {
                        if (paramValue[x].GetType().Equals(typeof(byte[])))
                        {
                            SqlParameter param = new SqlParameter("@" + x, SqlDbType.VarBinary, -1);
                            param.Value = paramValue[x];
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            SqlParameter param = null;
                            System.Type type = paramValue[x].GetType();
                            switch (type.Name)
                            {
                                case "String":
                                    param = new SqlParameter("@" + x, SqlDbType.VarChar, -1);
                                    break;
                                case "DateTime":
                                    param = new SqlParameter("@" + x, SqlDbType.DateTime);
                                    break;
                                case "Int64":
                                    param = new SqlParameter("@" + x, SqlDbType.BigInt);
                                    break;
                                case "Int32":
                                    param = new SqlParameter("@" + x, SqlDbType.Int);
                                    break;
                                case "Int16":
                                    param = new SqlParameter("@" + x, SqlDbType.SmallInt);
                                    break;
                                case "Byte":
                                    param = new SqlParameter("@" + x, SqlDbType.TinyInt);
                                    break;
                                case "Single":
                                case "Double":
                                case "Decimal":
                                    param = new SqlParameter("@" + x, SqlDbType.Float, 500);
                                    break;
                                case "Guid":
                                    param = new SqlParameter("@" + x, SqlDbType.UniqueIdentifier);
                                    break;
                                case "Byte[]":
                                    param = new SqlParameter("@" + x, SqlDbType.VarBinary, -1);
                                    break;
                                case "DBNull":
                                    param = new SqlParameter("@" + x, SqlDbType.VarChar, 500);
                                    param.SqlValue = "";
                                    break;
                            }
                            param.Value = paramValue[x];
                            command.Parameters.Add(param);
                        }
                    }
                }

                command.Prepare();
                command.ExecuteNonQuery();
            }
        }

        public static DataTable SelectDatatable(string sqlCommand, object[] paramValue)
        {
            DataTable dt = new DataTable();

            using (command = new SqlCommand(sqlCommand, con, dataTransaction))
            {
                if (paramValue != null)
                {
                    for (int x = 0; x < paramValue.Length; x++)
                    {
                        if (paramValue[x].GetType().Equals(typeof(byte[])))
                        {
                            SqlParameter param = new SqlParameter("@" + x, SqlDbType.VarBinary, -1);
                            param.Value = paramValue[x];
                            command.Parameters.Add(param);
                        }
                        else
                        {
                            SqlParameter param = null;
                            System.Type type = paramValue[x].GetType();
                            switch (type.Name)
                            {
                                case "String":
                                    param = new SqlParameter("@" + x, SqlDbType.VarChar, -1);
                                    break;
                                case "DateTime":
                                    param = new SqlParameter("@" + x, SqlDbType.DateTime);
                                    break;
                                case "Int64":
                                    param = new SqlParameter("@" + x, SqlDbType.BigInt);
                                    break;
                                case "Int32":
                                    param = new SqlParameter("@" + x, SqlDbType.Int);
                                    break;
                                case "Int16":
                                    param = new SqlParameter("@" + x, SqlDbType.SmallInt);
                                    break;
                                case "Byte":
                                    param = new SqlParameter("@" + x, SqlDbType.TinyInt);
                                    break;
                                case "Single":
                                case "Double":
                                case "Decimal":
                                    param = new SqlParameter("@" + x, SqlDbType.Float, 500);
                                    break;
                                case "Guid":
                                    param = new SqlParameter("@" + x, SqlDbType.UniqueIdentifier);
                                    break;
                                case "Byte[]":
                                    param = new SqlParameter("@" + x, SqlDbType.VarBinary, -1);
                                    break;
                                case "DBNull":
                                    param = new SqlParameter("@" + x, SqlDbType.VarChar, 500);
                                    param.SqlValue = "";
                                    break;
                            }
                            param.Value = paramValue[x];
                            command.Parameters.Add(param);
                        }
                    }
                }
            }

            using (dataReader = command.ExecuteReader())
            {
                if(dataReader.HasRows)
                {
                    dt.Load(dataReader);
                }
            }

            return dt;
        }
    }
}
