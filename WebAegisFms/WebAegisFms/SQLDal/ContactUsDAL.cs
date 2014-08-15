using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Data.SqlClient;

namespace SQLDal
{
    public class ContactUsDAL
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


        public ContactUsInfo Get(ContactUsInfo model)
        {
            ContactUsInfo returnValue = new ContactUsInfo();
            try
            {
                string spName = "uspAboutInsertUpdate";
                SqlParameter[] param =  { 
                SqlHelper.SqlParameter("@aboutus_id", System.Data.SqlDbType.Int, model.ContactId)                
                };

                using (SqlDataReader rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, System.Data.CommandType.StoredProcedure, spName, param))
                {
                    while (rdr.Read())
                    {
                                                
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
