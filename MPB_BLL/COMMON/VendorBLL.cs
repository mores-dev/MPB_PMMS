using MPB_DAL.COMMON;
using MPB_Entities.COMMON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace MPB_BLL.COMMON
{
    public class VendorBLL
    {
        VendorDAL vendorDAL = new VendorDAL();

        /// <summary>
        /// 取得航商下拉選單
        /// </summary>
        /// <returns></returns>
        public List<CodeName> GetVendorDropDown()
        {
            List<CodeName> list = vendorDAL.GetVendorDropDown();
            return list;
        }

        public List<CodeName> GetVendorDropDown(string cid)
        {
            List<CodeName> list = vendorDAL.GetVendorDropDown().Where(x => x.Code == cid).ToList();
            return list;
        }


        public static string GetCIdByNo(string key)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("53616960", "C00001");  //泰富
            dic.Add("088325806", "C00002");  //聯營處

            return dic[key];
        }

        public static string GetCIdByEng(string key)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("TF", "C00001");  //泰富
            dic.Add("TL", "C00002");  //聯營處
            try
            {
                return dic[key];
            }
            catch (Exception e)
            {
                return "";
            }



        }

    }
}
