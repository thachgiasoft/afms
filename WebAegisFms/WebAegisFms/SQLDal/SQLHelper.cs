using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using Common;

namespace SQLDal
{
    /// <summary>
    /// The SqlHelper class is intended to encapsulate high performance, 
    /// scalable best practices for common uses of SqlClient.
    /// </summary>
    public abstract class SqlHelper
    {

        //Database connection strings
        //public static readonly string ConnectionStringLocalTransaction = TrippleDES.Decrypt(ConfigurationManager.ConnectionStrings["SQLConnString"].ConnectionString);

        public static readonly string ConnectionStringLocalTransaction = ConfigurationManager.ConnectionStrings["SQLConnString"].ConnectionString;

        // Hashtable to store cached parameters
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {

            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //if (conn.State != ConnectionState.Open)
                //    conn.Open();
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                conn.Close();
                return val;
                
            }
        }
        /*Satish_ITS@20100116:SQL Server Encryption/Decryption*/
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, bool useEncryption, params SqlParameter[] commandParameters)
        {

            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                if (useEncryption)
                {
                    cmdText = "OPEN SYMMETRIC KEY RxEncrKey DECRYPTION BY CERTIFICATE RxEncrCert; " + cmdText;
                    //conn.Open();
                    //string sqlCerificateSelect = "OPEN SYMMETRIC KEY RxEncrKey DECRYPTION BY CERTIFICATE RxEncrCert";
                    //cmd = new SqlCommand(sqlCerificateSelect, conn);
                    //cmd.ExecuteNonQuery();
                }

                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                conn.Close();
                return val;
            }
        }
        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) against an existing database connection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">an existing database connection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {

            SqlCommand cmd = new SqlCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();            
            return val;
        }

        /*Satish_ITS@20100116:SQL Server Encryption/Decryption*/
        public static int ExecuteNonQuery(SqlConnection connection, CommandType cmdType, string cmdText, bool useEncryption, params SqlParameter[] commandParameters)
        {

            SqlCommand cmd = new SqlCommand();

            if (useEncryption)
            {
                cmdText = "OPEN SYMMETRIC KEY RxEncrKey DECRYPTION BY CERTIFICATE RxEncrCert; " + cmdText;
                //string sqlCerificateSelect = "OPEN SYMMETRIC KEY RxEncrKey DECRYPTION BY CERTIFICATE RxEncrCert";
                //cmd = new SqlCommand(sqlCerificateSelect, connection);
                //cmd.ExecuteNonQuery();
            }

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }


        /// <summary>
        /// Execute a SqlCommand (that returns no resultset) using an existing SQL Transaction 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="trans">an existing sql transaction</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /*Satish_ITS@20100116:SQL Server Encryption/Decryption*/

        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, bool useEncryption, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            if (useEncryption)
            {
                cmdText = "OPEN SYMMETRIC KEY RxEncrKey DECRYPTION BY CERTIFICATE RxEncrCert; " + cmdText;
                ////SqlConnection conn = trans.Connection;
                //string sqlCerificateSelect = "OPEN SYMMETRIC KEY RxEncrKey DECRYPTION BY CERTIFICATE RxEncrCert";
                //cmd = new SqlCommand(sqlCerificateSelect, trans.Connection, trans);
                //cmd.ExecuteNonQuery();
            }

            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }


        /// <summary>
        /// Execute a SqlCommand that returns a resultset against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>A SqlDataReader containing the results</returns>
        public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, CommandBehavior behavior, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(behavior);
                //cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }
        /*Satish_ITS@20100116:Encryption/Decryption  ExecuteReader function*/
        public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, bool useEncryption, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(connectionString);

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                if (useEncryption)
                {
                    cmdText = "OPEN SYMMETRIC KEY RxEncrKey DECRYPTION BY CERTIFICATE RxEncrCert; " + cmdText;
                    //conn.Open();
                    //string sqlCerificateSelect = "OPEN SYMMETRIC KEY RxEncrKey DECRYPTION BY CERTIFICATE RxEncrCert";
                    //cmd = new SqlCommand(sqlCerificateSelect, conn);
                    //cmd.ExecuteNonQuery();
                }

                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        /// <summary>
        /// Execute a SqlCommand that returns the first column of the first record against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //try
                //{
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                connection.Close();
                return val;
                //}
                //finally
                //{
                //    connection.Close();
                //}
            }
        }

        public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, bool useEncryption, params SqlParameter[] commandParameters)
        {
            //SqlCommand cmd = new SqlCommand();

            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{

            //    if (useEncryption)
            //    {
            //        string sqlCerificateSelect = "OPEN SYMMETRIC KEY RxEncrKey DECRYPTION BY CERTIFICATE RxEncrCert";
            //        cmd = new SqlCommand(sqlCerificateSelect, connection);

            //        if (connection.State != ConnectionState.Open)
            //            connection.Open();

            //        cmd.ExecuteNonQuery();
            //    }

            //    PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            //    object val = cmd.ExecuteScalar();
            //    cmd.Parameters.Clear();
            //    connection.Close();
            //    return val;
            //}

            SqlCommand cmd = new SqlCommand();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                if (useEncryption)
                {
                    cmdText = "OPEN SYMMETRIC KEY RxEncrKey DECRYPTION BY CERTIFICATE RxEncrCert; " + cmdText;
                }
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// Execute a SqlCommand that returns the first column of the first record against an existing database connection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">an existing database connection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or T-SQL command</param>
        /// <param name="commandParameters">an array of SqlParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        //public static object ExecuteScalar(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        //{

        //    SqlCommand cmd = new SqlCommand();

        //    PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
        //    object val = cmd.ExecuteScalar();
        //    cmd.Parameters.Clear();
        //    return val;
        //}

        //public static object ExecuteScalar(SqlConnection connection, CommandType cmdType, string cmdText, bool useEncryption,params SqlParameter[] commandParameters)
        //{

        //    SqlCommand cmd = new SqlCommand();

        //    if (useEncryption == true)
        //    {
        //        connection.Open();
        //        string sqlCerificateSelect = "OPEN SYMMETRIC KEY RxEncrKey DECRYPTION BY CERTIFICATE RxEncrCert";
        //        cmd = new SqlCommand(sqlCerificateSelect, connection);
        //        cmd.ExecuteNonQuery();
        //    }

        //    PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
        //    object val = cmd.ExecuteScalar();
        //    cmd.Parameters.Clear();
        //    return val;
        //}

        /// <summary>
        /// add parameter array to the cache
        /// </summary>
        /// <param name="cacheKey">Key to the parameter cache</param>
        /// <param name="cmdParms">an array of SqlParamters to be cached</param>
        public static void CacheParameters(string cacheKey, params SqlParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }

        /// <summary>
        /// Retrieve cached parameters
        /// </summary>
        /// <param name="cacheKey">key used to lookup parameters</param>
        /// <returns>Cached SqlParamters array</returns>
        public static SqlParameter[] GetCachedParameters(string cacheKey)
        {
            SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];

            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }

        /// <summary>
        /// Prepare a command for execution
        /// </summary>
        /// <param name="cmd">SqlCommand object</param>
        /// <param name="conn">SqlConnection object</param>
        /// <param name="trans">SqlTransaction object</param>
        /// <param name="cmdType">Cmd type e.g. stored procedure or text</param>
        /// <param name="cmdText">Command text, e.g. Select * from Products</param>
        /// <param name="cmdParms">SqlParameters to use in the command</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandTimeout = conn.ConnectionTimeout;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                {
                    if (parm.SqlValue == null) { parm.SqlValue = DBNull.Value; }
                    cmd.Parameters.Add(parm);
                }
            }
        }

        public static void OpenKeyByCertificate(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand();
            string sqlCerificateSelect = "OPEN SYMMETRIC KEY RxEncrKey DECRYPTION BY CERTIFICATE RxEncrCert";

            cmd = new SqlCommand(sqlCerificateSelect, conn);

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.ExecuteNonQuery();
        }

        //KP_ITSPune. start
        public static SqlParameter SqlParameter(string paramName, SqlDbType dbType, object value, bool isDBNullCheck = false, ParameterDirection direction = ParameterDirection.Input)
        {
            System.Data.SqlClient.SqlParameter param = new System.Data.SqlClient.SqlParameter(paramName, dbType);
            param.Direction = direction;

            param.Value = value;

            if(isDBNullCheck)
                if(string.IsNullOrEmpty(Functions.ToString(value)))
                {
                    param.Value = DBNull.Value;
                }

            return param;
        }

        public static SqlParameter SqlParameter(string paramName, SqlDbType dbType, int Size, object value, bool isDBNullCheck = false, ParameterDirection direction = ParameterDirection.Input)
        {
            System.Data.SqlClient.SqlParameter param = new System.Data.SqlClient.SqlParameter(paramName, dbType, Size);
            param.Direction = direction;
            param.Value = value;

            if (isDBNullCheck)
                if (string.IsNullOrEmpty(Functions.ToString(value)))
                {
                    param.Value = DBNull.Value;
                }

            return param;
        }
        //KP_ITSPune. end

    }
}
