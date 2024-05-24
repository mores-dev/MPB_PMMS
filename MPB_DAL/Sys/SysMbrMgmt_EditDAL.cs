
using MPB_Entities.Sys;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System.Collections.Generic;
using System;

namespace MPB_DAL.Sys
{
    public class SysMbrMgmt_EditDAL : DALBase
    {
        public SysMbrMgmt_EditDAL() { }

        public SysMbrMgmt_EditDAL(DbManager db) : base(db) { }

        public SysMbrMgmt_EditMain Select_cGAAccount(SysMbrMgmt_EditMain qc)
        {
            //page的table一定要寫化名(alias)，eg. Table t。也要Order by。

            string sql;
            sql = "";
            sql += "SELECT ";

            //類型
            sql += " A.GA_TYPE";
            //帳號
            sql += " , A.ACCOUNT AS ACCOUNT";
            //業者名稱
            sql += " , A.NAME AS GA_NAME";
            //統一編號
            sql += " , A.UNI_ID AS UNI_ID";
            //EMAIL
            sql += " , A.EMAIL AS EMAIL";
            //電話類型
            sql += " , A.PHONE_TYPE AS PHONE_TYPE";
            //電話號碼
            sql += " , A.PHONE AS PHONE";
            //聯絡人
            sql += " , A.CONTACT AS CONTACT";
            //狀態
            sql += " , A.AccStatus AS GA_STATUS";
            //資料建立者
            sql += "\n" + " ,A.CREATEID + ' - ' + (SELECT U.NAME FROM cAFCAccount U WHERE U.ACCOUNT = A.CREATEID) CREATEID ";
            //資料建立日期時間
            sql += " ,CONVERT(VARCHAR, A.CREATEDT, 120)  as CREATEDT";
            //資料修改者
            sql += " ,A.MODIFYID + ' - ' + (SELECT U.NAME FROM cAFCAccount U WHERE U.ACCOUNT = A.CREATEID) MODIFYID ";
            //資料修改日期時間
            sql += " ,CONVERT(VARCHAR, A.MODIFYDT, 120)  as MODIFYDT";

            //FROM AND LEFT JOIN 
            sql += " FROM cGAAccount A ";

            //查詢條件 
            sql += " WHERE 1=1 ";
            //查詢條件 序號
            sql += " and A.ACCOUNT = @GaAAA ";
            
            //查詢條件 航商代號
            if (!string.IsNullOrWhiteSpace(qc.C_ID))
                sql += " And A.C_ID LIKE '%' + @C_ID + '%' ";

            return SingleOrDefault<SysMbrMgmt_EditMain>(@sql, qc);
        }

        public List<AjaxKeyCountResult> Check_Key(SysMbrMgmt_EditMain qc)
        {
            string sql = "";
            sql += "SELECT ";
            sql += " count(*) KEY_COUNT ";
            sql += " From cGAAccount ";
            sql += " WHERE 1 = 1  ";
            sql += " and ACCOUNT = @GaAAA ";
            return Fetch<AjaxKeyCountResult>(@sql, qc);
        }
    }
}
