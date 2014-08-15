using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using SQLDal;
namespace BLL
{
    public class AboutUs
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

        public AboutUsInfo Get(AboutUsInfo model)
        {
            AboutUsDAL obj = new AboutUsDAL();
            return obj.Get(model);
        }


    }
}
