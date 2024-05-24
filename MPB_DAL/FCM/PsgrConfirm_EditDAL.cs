
using MPB_Entities.FCM;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System.Collections.Generic;
using System;

namespace MPB_DAL.FCM
{
    public class PsgrConfirm_EditDAL : DALBase
    {
        public PsgrConfirm_EditDAL() { }

        public PsgrConfirm_EditDAL(DbManager db) : base(db) { }

        public PsgrConfirm_EditMain Select_Manifest(PsgrConfirm_EditMain qc)
        {
            //page的table一定要寫化名(alias)，eg. Table t。也要Order by。

            string sql;
            sql = "";
            sql += "SELECT ";

            //航商序號
            sql += " A.C_ID";
            //航商名稱
            sql += " , A.C_NAME";
            //航站
            sql += " , A.STATION";
            //航班時間
            sql += " , A.VOYAGE_TIME";
            //船舶代號
            sql += " , A.VESSEL_ID";
            //航班名稱
            sql += " , A.VOYAGE_TIME + ' ' + A.R_NAME as R_NAME";
            //船隻名稱
            sql += " , A.VESSEL_NAME";

            //資料建立者
            sql += "\n" + " ,A.CREATEID + ' - ' + (SELECT U.NAME FROM cAFCAccount U WHERE U.ACCOUNT = A.CREATEID) CREATEID ";
            //資料建立日期時間
            sql += " ,CONVERT(VARCHAR, A.CREATEDT, 120)  as CREATEDT";
            //資料修改者
            sql += " ,A.MODIFYID + ' - ' + (SELECT U.NAME FROM cAFCAccount U WHERE U.ACCOUNT = A.CREATEID) MODIFYID ";
            //資料修改日期時間
            sql += " ,CONVERT(VARCHAR, A.MODIFYDT, 120)  as MODIFYDT";

            //FROM AND LEFT JOIN 
            sql += " FROM cManifest A ";

            //查詢條件 
            sql += " WHERE 1=1 ";
            sql += "  AND A.SHIPPING_DATE = CONVERT(DATE, GETDATE())";
            sql += "  And A.STATION = @Station";
            sql += "  And A.VOYAGE_TIME = @VoyageTime";
            sql += "  ANd A.VESSEL_ID = @VesselId";
            sql += "  AND A.C_ID = @C_ID ";

            return SingleOrDefault<PsgrConfirm_EditMain>(@sql, qc);
        }

        public List<PsgrConfirm_EditDetailGrid1> Select_Grid1_ManifestDtl(PsgrConfirm_EditMain qc)
        {
            //page的table一定要寫化名(alias)，eg. Table t。也要Order by。

            string sql;
            sql = "";
            sql += "SELECT ";

            //航商序號
            sql += " A.C_ID";
            //航商名稱
            sql += " , A.C_NAME";
            //航站
            sql += " , A.STATION";
            //航班時間
            sql += " , A.VOYAGE_TIME";
            //船舶代號
            sql += " , A.VESSEL_ID";
            //航班名稱
            sql += " , A.R_NAME";
            //船隻名稱
            sql += " , A.VESSEL_NAME";
            //乘客姓名
            sql += " , B.PSGR_NAME";
            //證件別
            sql += " , ISNULL(C.CodeDesc, '') as ID_TYPE";
            //證件號碼
            sql += " , B.ID_NO";
            //證件號碼
            sql += " , B.ID_NO as ID_NO_ENCODE";
            //出生日期  
            sql += " , Convert(Varchar, B.BIRTH, 111) as BIRTH ";

            //資料建立者
            sql += "\n" + " ,A.CREATEID + ' - ' + (SELECT U.NAME FROM cAFCAccount U WHERE U.ACCOUNT = A.CREATEID) CREATEID ";
            //資料建立日期時間
            sql += " ,CONVERT(VARCHAR, A.CREATEDT, 120)  as CREATEDT";
            //資料修改者
            sql += " ,A.MODIFYID + ' - ' + (SELECT U.NAME FROM cAFCAccount U WHERE U.ACCOUNT = A.CREATEID) MODIFYID ";
            //資料修改日期時間
            sql += " ,CONVERT(VARCHAR, A.MODIFYDT, 120)  as MODIFYDT";

            //FROM AND LEFT JOIN 
            sql += " FROM cManifest A ";
            sql += " Join cManifestDtl B ON A.SHIPPING_DATE = B.SHIPPING_DATE";
            sql += "  And A.STATION = B.STATION AND A.VOYAGE_TIME = B.VOYAGE_TIME AND A.VESSEL_ID = B.VESSEL_ID";

            sql += " Left Join cCodeList C ON C.CodeKey = 'SYS01005' AND B.ID_TYPE = C.CodeID";

            //查詢條件 
            sql += " WHERE 1=1 ";
            sql += "  AND A.SHIPPING_DATE = CONVERT(DATE, GETDATE())";
            sql += "  And A.STATION = @Station";
            sql += "  And A.VOYAGE_TIME = @VoyageTime";
            sql += "  ANd A.VESSEL_ID = @VesselId";
            sql += "  AND A.C_ID = @C_ID ";

            return Fetch<PsgrConfirm_EditDetailGrid1>(@sql, qc);
        }

        public int Insert_ManifestDtl(PsgrConfirm_EditMain em)
        {
            string sql;

            sql = "Insert into cManifestDtl (";
            //日期
            sql += "SHIPPING_DATE";
            //航站
            sql += ", STATION";
            //班次時間
            sql += ", VOYAGE_TIME";
            //船號
            sql += ", VESSEL_ID";
            //證件號碼
            sql += ", ID_NO";
            //票券號碼、票券名稱、登船時間
            sql += ", TICKET_NO, TICKET_NAME, BOARDING_DT";
            //證件類別
            sql += ", ID_TYPE";
            //乘客姓名
            sql += ", PSGR_NAME";
            //出生日期
            sql += ", BIRTH";
            //性別
            sql += ", SEX";
            //手機、旅行社代碼、旅行社名稱
            sql += ", PHONE, AGENT_ID, AGENT_NAME";
            //資料建立紀錄
            sql += ", CREATEDT, CREATEID, MODIFYDT, MODIFYID)";

            sql += " Select ";
            //日期
            sql += " GETDATE()";
            //航站
            sql += ", @Station";
            //班次時間
            sql += ", @VoyageTime";
            //船號
            sql += ", @VesselId";
            //證件號碼
            sql += ", @IdNoEncode";
            //票券號碼、票券名稱、登船時間
            sql += ", '', '', SysDateTime()";
            //證件類別
            sql += ", @IdType";
            //乘客姓名
            sql += ", @PsgrName";
            //出生日期
            sql += ", @Birth";
            //性別
            sql += ", @Sex";
            //手機、旅行社代碼、旅行社名稱
            sql += ", '', '', ''";
            //資料建立紀錄
            sql += ", SysDateTime(), @CreateId, SysDateTime(), @CreateId";


            return Execute(@sql, em);
        }

        public int Delete_ManifestDtl(PsgrConfirm_EditMain em)
        {
            string sql;

            sql = "Delete";
            sql += " From cManifestDtl ";
            sql += "Where 1 = 1";
            sql += "\r\n";
            sql += "  And SHIPPING_DATE = CONVERT(DATE, GETDATE())";
            sql += "  And STATION = @Station";
            sql += "  And VOYAGE_TIME = @VoyageTime";
            sql += "  And VESSEL_ID = @VesselId";
            sql += "  And ID_NO = @IdNoEncode";

            return Execute(@sql, em);
        }
    }
}
