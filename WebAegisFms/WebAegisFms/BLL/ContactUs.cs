using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using SQLDal;
namespace BLL
{
    public class ContactUs
    {
        public bool Insert(AboutUsInfo model)
        {
            AboutUsDAL obj = new AboutUsDAL();
            return obj.Insert(model);
        }


        public bool Set(AboutUsInfo model)
        {
            AboutUsDAL obj = new AboutUsDAL();
            return obj.Set(model);
        }

        public ContactUsInfo Get(ContactUsInfo model)
        {
            ContactUsDAL dal = new ContactUsDAL();
            return dal.Get(model);
        }


    }
}
