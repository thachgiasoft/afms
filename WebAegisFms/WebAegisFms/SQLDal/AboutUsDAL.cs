using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Data.SqlClient;

namespace SQLDal
{
    public class AboutUsDAL
    {
        public bool Insert(AboutUsInfo model)
        {
            bool retrunValue = true;
            try
            {
                string spName = "uspAboutInsertUpdate";
                SqlParameter[] param =  { 
                SqlHelper.SqlParameter("@aboutus1", System.Data.SqlDbType.VarChar, model.AboutUs1, true),
                SqlHelper.SqlParameter("@aboutus2", System.Data.SqlDbType.VarChar, model.AboutUs2, true),
                SqlHelper.SqlParameter("@aboutus3", System.Data.SqlDbType.VarChar, model.AboutUs3, true),
                SqlHelper.SqlParameter("@userId", System.Data.SqlDbType.VarChar,model.LoginUser , true)
                };

                int Resultval = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionStringLocalTransaction, System.Data.CommandType.StoredProcedure, spName, param);

                if (Resultval <= 0)
                {
                    retrunValue = false;
                }

            }
            catch (Exception ex)
            { 
                retrunValue=false;
            }

            return retrunValue;
        }


        public bool Set(AboutUsInfo model)
        {
            bool retrunValue = true;
            try
            {
                string spName = "uspAboutInsertUpdate";
                SqlParameter[] param = { 
                SqlHelper.SqlParameter("@aboutus_id", System.Data.SqlDbType.Int, model.AboutUsId),
                SqlHelper.SqlParameter("@aboutus1", System.Data.SqlDbType.VarChar, model.AboutUs1, true),
                SqlHelper.SqlParameter("@aboutus2", System.Data.SqlDbType.VarChar, model.AboutUs2, true),
                SqlHelper.SqlParameter("@aboutus3", System.Data.SqlDbType.VarChar, model.AboutUs3, true),
                SqlHelper.SqlParameter("@userId", System.Data.SqlDbType.VarChar,model.LoginUser , true)
                };

                int Resultval = SqlHelper.ExecuteNonQuery(SqlHelper.ConnectionStringLocalTransaction, System.Data.CommandType.StoredProcedure, spName, param);

                if (Resultval <= 0)
                {
                    retrunValue = false;
                }




            }
            catch (Exception ex)
            {
                retrunValue = false;
            }

            return retrunValue;
        }


        public AboutUsInfo Get(AboutUsInfo model)
        {
            AboutUsInfo returnValue = new AboutUsInfo();
            try
            {
                string spName = "uspAboutInsertUpdate";
                SqlParameter[] param =  { 
                SqlHelper.SqlParameter("@aboutus_id", System.Data.SqlDbType.Int, model.AboutUsId)                
                };

                using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, System.Data.CommandType.StoredProcedure, spName, param))
                {
                    while (rdr.Read())
                    {
                        returnValue.AboutUsId = rdr.GetInt32(0);
                        returnValue.AboutUs1 = rdr.GetString(1);
                        returnValue.AboutUs2 = rdr.GetString(2);
                        returnValue.AboutUs3 = rdr.GetString(3);
                        
                    }
                }
            }
            catch (Exception ex)
            { 

            }

            return returnValue;
        }




    }
}
