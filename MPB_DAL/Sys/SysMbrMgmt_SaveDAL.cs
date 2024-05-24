
using MPB_Entities.Sys;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System.Collections.Generic;
using System;

namespace MPB_DAL.Sys
{
    public class SysMbrMgmt_SaveDAL : DALBase
    {
        public SysMbrMgmt_SaveDAL() { }

        public SysMbrMgmt_SaveDAL(DbManager db) : base(db) { }

        public int Update_cGAAccount(SysMbrMgmt_SaveMain sm)
        {
            string sql = "";
            sql += "UPDATE cGAAccount ";
            sql += " SET ";

            sql += "   GA_TYPE   = @GaType";
            sql += " , NAME      = @GaName";
            if (!string.IsNullOrWhiteSpace(sm.GA_PPP))
                sql += " , PASSWORD  = @GA_PPP";
            sql += " , UNI_ID    = @UniId";
            sql += " , EMAIL     = @Email";
            sql += " , PHONE_TYPE= @PhoneType";
            sql += " , PHONE     = @Phone";
            sql += " , CONTACT   = @Contact";
            sql += " , AccStatus = @GaStatus";

            //資料修改者
            sql += " , MODIFYID = @ModifyId";
            //資料修改日期時間
            sql += " , MODIFYDT  = SysDateTime()";

            //WHERE 條件
            sql += " where 1 = 1 ";
            //條件 序號 
            sql += " AND ACCOUNT = @GaAAA";

            return Execute(@sql, sm);
        }

        public string Select_Company()
        {
            string sql = "";

            sql += " select C_ID + ','    ";
            sql += " from cCompany   ";
            sql += " Where 1 = 1    ";
            sql += " AND C_ID NOT IN('C00000') ";
            sql += " For XML Path('')";

            return SingleOrDefault<string>(@sql);
        }

        public int Insert_ARole(SysMbrMgmt_SaveMain sm)
        {
            string sql = "";


            sql += " INSERT INTO cAccountRole (";
            sql += "   ACCOUNT      ";
            sql += " , DeviceTypeID ";
            sql += " , RoleID       ";

            sql += " ) Select ";

            sql += "   @GaAAA ";
            sql += " , 'A'      ";
            sql += " , '3000'   ";

            return Execute(@sql, sm);
        }

        public int Insert_GAA(SysMbrMgmt_SaveMain sm)
        {
            string sql = "";


            sql += " INSERT INTO cGAAccount (";
            sql += "   ACCOUNT      ";
            sql += " , DeviceTypeID ";
            sql += " , C_ID         ";
            sql += " , GA_ID        ";
            sql += " , GA_TYPE      ";
            sql += " , NAME         ";
            sql += " , PASSWORD     ";
            sql += " , UNI_ID       ";
            sql += " , EMAIL        ";
            sql += " , PHONE_TYPE   ";
            sql += " , PHONE        ";
            sql += " , CONTACT      ";
            sql += " , AccStatus    ";
            sql += " , SYNCTIME     ";
            sql += " , CREATEDT     ";
            sql += " , CREATEID     ";
            sql += " , MODIFYDT     ";
            sql += " , MODIFYID     ";

            sql += " ) Select ";

            sql += "   @GaAAA ";
            sql += " , 'A' ";
            sql += " , @C_ID    ";
            sql += " , @GaAAA ";
            sql += " , @GaType  ";
            sql += " , @GaName  ";
            sql += " , @GA_PPP  ";
            sql += " , @UniId   ";
            sql += " , @Email   ";
            sql += " , @PhoneType   ";
            sql += " , @Phone   ";
            sql += " , @Contact ";
            sql += " , 'Y'  ";
            sql += " , ''   ";
            sql += " , GETDATE()    ";
            sql += " , @CreateId    ";
            sql += " , GETDATE()    ";
            sql += " , @CreateId    ";

            return Execute(@sql, sm);
        }

        public int Update_PPP(SysMbrMgmt_SaveMain sm)
        {
            string sql = "";
            sql += "UPDATE cGAAccount ";
            sql += " SET ";

            //密碼
            sql += " PASSWORD = @GA_PPP";

            //資料修改者
            sql += " , MODIFYID = @ModifyId";
            //資料修改日期時間
            sql += " , MODIFYDT  = SysDateTime()";

            //WHERE 條件
            sql += " where 1 = 1 ";
            //條件 序號 
            sql += " AND ACCOUNT = @GA_ACCOUNT";

            return Execute(@sql, sm);
        }

    }
}
