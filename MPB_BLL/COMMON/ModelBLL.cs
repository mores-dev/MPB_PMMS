using Newtonsoft.Json.Linq;
using System.Linq;
using System.Reflection;

namespace MPB_BLL.COMMON
{
    public class ModelBLL : BLLBase
    {
        /// <summary>
        /// 同類別比較
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m1"></param>
        /// <param name="m2"></param>
        /// <returns></returns>
        public static bool Compare<T>(T m1, T m2) where T : class
        {
            PropertyInfo[] pps1 = m1.GetType().GetProperties();
            PropertyInfo[] pps2 = m2.GetType().GetProperties();
            foreach (PropertyInfo p1 in pps1)
            {
                PropertyInfo p2 = pps2.Where(x => x.Name == p1.Name && x.GetType().Equals(p1.GetType())).FirstOrDefault();
                if (p2 != null)
                {
                    if(p1.GetValue(m1)!=null && p2.GetValue(m1) != null)
                    {
                        if (p1.GetValue(m1).ToString() != p2.GetValue(m2).ToString())
                            return false;
                    }
                }
                    
            }
            return true;
        }

        /// <summary>
        /// 同屬性同型別轉移其值
        /// </summary>
        /// <param name="obj_in">輸入的model</param>
        /// <param name="obj_out">輸出的model</param>
        /// <returns></returns>
        public static void Convert<T>(object obj_in, ref T obj_out) where T : class
        {
            PropertyInfo[] pps_in = obj_in.GetType().GetProperties();
            PropertyInfo[] pps_out = obj_out.GetType().GetProperties();
            foreach (PropertyInfo pp_out in pps_out)
            {
                PropertyInfo pp_in = pps_in.Where(x => x.Name == pp_out.Name && x.GetType().Equals(pp_out.GetType())).FirstOrDefault();
                if (pp_in != null)
                    pp_out.SetValue(obj_out, pp_in.GetValue(obj_in));
            }

        }

        public static void JObjToModel<T>(JObject jObj, ref T obj_out)
        {
            PropertyInfo[] pps_out = obj_out.GetType().GetProperties();
            foreach (PropertyInfo pp_out in pps_out)
            {
                object str = null;
                var val = jObj[pp_out.Name];
                if (val != null)
                {
                    if (pp_out.PropertyType.Name == "Int32")
                        str = System.Convert.ToInt32(val);
                    else
                        str = val.ToString();
                }
                pp_out.SetValue(obj_out, str);
            }
        }
    }
}
