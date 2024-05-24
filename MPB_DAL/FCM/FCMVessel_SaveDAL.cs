
using MPB_Entities.FCM;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System.Collections.Generic;

namespace MPB_DAL.FCM
{
    public class FCMVessel_SaveDAL : DALBase
    {
        public FCMVessel_SaveDAL() { }

        public FCMVessel_SaveDAL(DbManager db) : base(db) { }

        public int GetMaxV_ID()
        {
            string sql = "";

            sql += " SELECT RIGHT(ISNULL(MAX(V_ID), '0'), 5) FROM cVessel";

            return SingleOrDefault<int>(sql);
        }

        public int Insert_cVessel(FCMVessel_SaveMain sm)
        {
            string sql = "";

            sql = "Insert into cVessel (";
            //航商序號
            sql += "C_ID";
            //船隻序號
            sql += ", V_ID";
            //船舶號數
            sql += ", V_CODE";
            //船舶號數
            sql += ", VesselNo";
            //船名
            sql += ", V_NAME";
            //船名
            sql += " , API_NAME";
            //船名_英文
            sql += ", V_NAME_EN";
            //狀態
            sql += ", V_STATUS";
            //乘客限額
            sql += ", MAXIMUM";
            //所屬公司名稱
            sql += ", BelongCompany";
            //所屬公司統編
            sql += ", BelongTAX_ID";
            //AIS船舶編號
            sql += ", MMSI";
            //國際船舶編號
            sql += ", IMO";
            //船呼
            sql += ", CallSign";
            //船舶種類
            sql += ", VesselType";
            //船舶總噸位(噸)
            sql += ", GrossTonnage";
            //船長(公尺)
            sql += ", VesselLength";
            //船寬(公尺)
            sql += ", VesselWidth";
            //吃水深度(公尺)
            sql += ", LoadDraft";
            //備註
            sql += ", MEMO";

            //reserve columns
            sql += "\n" + ", SYNCTIME";
            //資料建立紀錄
            sql += ", CREATEDT, CREATEID, MODIFYDT, MODIFYID)";

            sql += "\n" + " Select ";
            //航商序號
            sql += "  @C_ID";
            //船隻序號
            sql += ", @V_ID";
            //船舶號數
            sql += ", @V_CODE";
            //船舶號數
            sql += ", @V_CODE";
            //船隻名稱
            sql += ", @V_NAME";
            //船名 MTNet
            sql += " , @API_NAME";
            //船名_英文
            sql += ", @V_NAME_EN";
            //狀態
            sql += ", @V_STATUS";
            //乘客限額
            sql += ", @MAXIMUM";
            //所屬公司名稱
            sql += ", @BelongCompany";
            //所屬公司統編
            sql += ", @BelongTAX_ID";
            //AIS船舶編號
            sql += ", @MMSI";
            //國際船舶編號
            sql += ", @IMO";
            //船呼
            sql += ", @CallSign";
            //船舶種類
            sql += ", @VesselType";
            //船舶總噸位(噸)
            sql += ", @GrossTonnage";
            //船長(公尺)
            sql += ", @VesselLength";
            //船寬(公尺)
            sql += ", @VesselWidth";
            //吃水深度(公尺)
            sql += ", @LoadDraft";
            //備註
            sql += ", @Memo";

            //reserve columns
            sql += "\n" + ", ''";
            //資料建立紀錄
            sql += ", SysDateTime(), @CreateId, SysDateTime(), @CreateId ";

            return Execute(@sql, sm);
        }

        public int Update_cVessel(FCMVessel_SaveMain sm)
        {
            string sql = "";
            sql += "UPDATE cVessel ";
            sql += " SET ";

            //船隻名稱
            sql += " V_NAME = @V_NAME";
            //船隻名稱
            sql += " , API_NAME = @API_NAME";
            //船隻名稱
            sql += " , V_NAME_EN = @V_NAME_EN";
            //船舶號數
            sql += " , V_CODE = @V_CODE";
            //狀態
            sql += " , V_STATUS = @V_STATUS";
            //乘客限額
            sql += " , MAXIMUM = @MAXIMUM";
            //所屬公司名稱
            sql += " , BelongCompany = @BelongCompany";
            //所屬公司統編
            sql += " , BelongTAX_ID = @BelongTAX_ID";
            //AIS船舶編號
            sql += " , MMSI = @MMSI";
            //國際船舶編號
            sql += " , IMO = @IMO";
            //船呼
            sql += " , CallSign = @CallSign";
            //船舶種類
            sql += " , VesselType = @VesselType";
            //船舶總噸位(噸)
            sql += " , GrossTonnage = @GrossTonnage";
            //船長(公尺)
            sql += " , VesselLength = @VesselLength";
            //船寬(公尺)
            sql += " , VesselWidth = @VesselWidth";
            //吃水深度(公尺)
            sql += " , LoadDraft = @LoadDraft";
            //備註
            sql += " , MEMO = @Memo";

            //資料修改者
            sql += " , MODIFYID = @ModifyId";
            //資料修改日期時間
            sql += " , MODIFYDT  = SysDateTime()";

            //WHERE 條件
            sql += " where 1 = 1 ";
            //條件 航商序號 
            sql += " AND C_ID = @C_ID";
            //條件 船隻序號 
            sql += " AND V_ID = @V_ID";

            return Execute(@sql, sm);
        }

        public int Delete_cVessel(FCMVessel_SaveMain sm)
        {
            string sql = "";
            sql += "UPDATE cVessel ";
            sql += " SET ";

            //狀態
            sql += " V_STATUS = '0',";

            //資料修改者
            sql += " MODIFYID = @ModifyId,";
            //資料修改日期時間
            sql += " MODIFYDT  = SysDateTime()";

            //WHERE 條件
            sql += " where 1 = 1 ";
            //條件 航商序號 
            sql += " AND C_ID = @C_ID";
            //條件 船隻序號 
            sql += " AND V_ID = @V_ID";

            return Execute(@sql, sm);
        }

    }
}
