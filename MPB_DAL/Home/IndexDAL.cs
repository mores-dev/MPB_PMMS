using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPB_Entities.COMMON;

namespace MPB_DAL.Home
{
    public class IndexDAL : DALBase
    {
        /// <summary>
        /// 建構子
        /// </summary>
        public IndexDAL() { }
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="db">DbManager</param>
        public IndexDAL(DbManager db) : base(db) { }

        /// <summary>
        /// 取得系統代碼資料
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns>List<CodeName></returns>
        public List<CodeName> GetSubSystem(string userId)
        {
            throw new NotImplementedException();
//            return Fetch<CodeName>(@"select MENU_ID code, MENU_NAME name from ATH_MENU AM where MENU_ID in(
//                select AP.MENU_ID from ATH_ROLEPROG ARP
//                left join ATH_PROG AP on AP.PROG_ID = ARP.PROG_ID and AP.PROG_VISIABLE = 'Y'
//                left join ATH_USERROLE AUR on AUR.ROLE_ID = ARP.ROLE_ID
//                left join ATH_USER AU on rtrim(AU.USER_ID) = rtrim(AUR.USER_ID)
//                left join ATH_MENU AM on AM.MENU_ID=AP.MENU_ID and AM.MENU_VISIABLE = 'Y' 
//				left join ATH_ROLE AR on AR.ROLE_ID=AUR.ROLE_ID
//                where ARP.PROG_EXEC='Y' and AU.USER_ENABLE ='Y' and AR.SYS_KIND='I' and AM.SYS_KIND='I' and rtrim(AU.USER_ID) =@0)
//                and MENU_VISIABLE='Y' and SYS_KIND='I'
//                order by AM.MENU_SEQ", userId);

        }
    }
}
