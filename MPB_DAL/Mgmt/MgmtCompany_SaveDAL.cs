
using MPB_Entities.Mgmt;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System.Collections.Generic;

namespace MPB_DAL.Mgmt
{
    public class MgmtCompany_SaveDAL : DALBase
    {
        public MgmtCompany_SaveDAL() { }

        public MgmtCompany_SaveDAL(DbManager db) : base(db) { }

        public int GetMaxC_ID()
        {
            string sql = "";

            sql += " SELECT RIGHT(ISNULL(MAX(C_ID), '0'), 5) FROM cCompany";

            return SingleOrDefault<int>(sql);
        }

        public int Insert_cCompany(MgmtCompany_SaveMain sm)
        {
            string sql = "";

            sql = "Insert into cCompany (";
            //航商序號
            sql += "C_ID";
            //航商代號
            sql += ", C_CODE";
            //航商名稱
            sql += ", C_NAME";
            //航商名稱_英文
            sql += ", C_NAME_EN";
            //狀態
            sql += ", C_STATUS";
            //備註
            sql += ", C_MEMO";
            //統編
            sql += ", C_TAX_ID";
            //聯絡電話  
            sql += ", PHONE";
            //地址  
            sql += ", C_ADDRESS";
            //EMAIL  
            sql += ", EMAIL";
            //訂票電話  
            sql += ", ODR_PHONE";
            //官網網址  
            sql += ", LAND_URL";
            //票價網址  
            sql += ", PRICE_URL";
            //訂票網址  
            sql += ", ODR_URL";
            //LOGO網址  
            sql += ", LOGO_URL";

            //reserve columns
            sql += "\n" + ", SYNCTIME";
            //資料建立紀錄
            sql += ", CREATEDT, CREATEID, MODIFYDT, MODIFYID)";

            sql += "\n" + " Select ";
            //航商序號
            sql += "  @C_ID";
            //航商代號
            sql += ", @C_CODE";
            //航商名稱
            sql += ", @C_NAME";
            //航商名稱_英文
            sql += ", @C_NAME_EN";
            //狀態
            sql += ", @C_STATUS";
            //備註
            sql += ", @C_MEMO";
            //統編
            sql += ", @C_TAX_ID";
            //聯絡電話  
            sql += ", @Phone";
            //地址  
            sql += ", @C_Address";
            //EMAIL  
            sql += ", @Email";
            //訂票電話  
            sql += ", @Odr_Phone";
            //官網網址  
            sql += ", @LAND_URL";
            //票價網址  
            sql += ", @Price_URL";
            //訂票網址  
            sql += ", @Odr_URL";
            //LOGO網址  
            sql += ", @Logo_URL";

            //reserve columns
            sql += "\n" + ", ''";
            //資料建立紀錄
            sql += ", SysDateTime(), @CreateId, SysDateTime(), @CreateId ";

            return Execute(@sql, sm);
        }

        public int Update_cCompany(MgmtCompany_SaveMain sm)
        {
            string sql = "";
            sql += "UPDATE cCompany ";
            sql += " SET ";

            //航商名稱
            sql += " C_NAME = @C_NAME";
            //航商名稱
            sql += " , C_NAME_EN = @C_NAME_EN";
            //航商代號
            sql += " , C_CODE = @C_CODE";
            //狀態
            sql += " , C_STATUS = @C_STATUS";
            //備註
            sql += " , C_TAX_ID = @C_TAX_ID";
            //聯絡電話  
            sql += " , PHONE = @Phone";
            //地址  
            sql += " , C_ADDRESS = @C_Address";
            //EMAIL  
            sql += " , EMAIL = @Email";
            //訂票電話  
            sql += " , ODR_PHONE = @Odr_Phone";
            //官網網址  
            sql += " , LAND_URL = @LAND_URL";
            //票價網址  
            sql += " , PRICE_URL = @Price_URL";
            //訂票網址  
            sql += " , ODR_URL = @Odr_URL";
            //LOGO網址  
            sql += " , LOGO_URL = @Logo_URL";

            //資料修改者
            sql += " , MODIFYID = @ModifyId";
            //資料修改日期時間
            sql += " , MODIFYDT  = SysDateTime()";

            //WHERE 條件
            sql += " where 1 = 1 ";
            //條件 航商序號 
            sql += " AND C_ID = @C_ID";

            return Execute(@sql, sm);
        }

        public int Delete_cCompany(MgmtCompany_SaveMain sm)
        {
            string sql = "";
            sql += "UPDATE cCompany ";
            sql += " SET ";

            //狀態
            sql += " C_STATUS = '0',";

            //資料修改者
            sql += " MODIFYID = @ModifyId,";
            //資料修改日期時間
            sql += " MODIFYDT  = SysDateTime()";

            //WHERE 條件
            sql += " where 1 = 1 ";
            //條件 航商序號 
            sql += " AND C_ID = @C_ID";

            return Execute(@sql, sm);
        }

    }
}
