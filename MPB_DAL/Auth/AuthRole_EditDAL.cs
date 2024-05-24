
using MPB_Entities.Auth;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System;
using System.Collections.Generic;

namespace MPB_DAL.Auth
{
    public class AuthRole_EditDAL : DALBase
    {
        public AuthRole_EditDAL() { }

        public AuthRole_EditDAL(DbManager db) : base(db) { }

        /// <summary>
        /// 讀取 cAFCAccount 使用者檔
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public AuthRole_EditMain Select_RoleInfo(AuthRole_EditMain qc)
        {
            //page的table一定要寫化名(alias)，eg. Table t。也要Order by。

            string sql;
            sql = "";
            sql += "SELECT ";
            //群組代號
            sql += " A.RoleID";
            //群組名稱
            sql += " ,A.RoleName";
            //群組說明
            sql += " ,A.RoleType ";
            //是否啟用  
            sql += ", A.RoleStatus as Status";

            //資料建立者
            sql += " ,A.CREATEID + ' - ' + (SELECT U.NAME FROM cAFCAccount U WHERE U.ACCOUNT = A.CREATEID) CREATEID ";
            //資料建立日期時間
            sql += " ,CONVERT(VARCHAR, A.CREATEDT, 120)  as CREATEDT";
            //資料修改者
            sql += " ,A.MODIFYID + ' - ' + (SELECT U.NAME FROM cAFCAccount U WHERE U.ACCOUNT = A.CREATEID) MODIFYID ";
            //資料修改日期時間
            sql += " ,CONVERT(VARCHAR, A.MODIFYDT, 120)  as MODIFYDT";

            //FROM AND LEFT JOIN 
            sql += " FROM cRoleInfo   A    "; //群組檔
            //查詢條件 
            sql += " WHERE 1=1 ";
            //查詢條件 使用者代號
            sql += "   And A.RoleID = @RoleId ";

            return SingleOrDefault<AuthRole_EditMain>(@sql, qc);
        }

        public List<AuthRole_EditDetailGrid1> Select_Grid1_RoleFunc(AuthRole_EditMain qc)
        {
            string sql = "";

            sql += "Select ";
            //選單代號
            sql += "B.DeviceTypeID ";
            //選單代號
            sql += ", C.FuncID as MenuID";
            //程式代號
            sql += ", B.FuncID as ProgID";
            //執行
            sql += ", Case When A.IsRun = 'Y' Then 'true' else 'false' end as ProgExec";
            //新增
            sql += ", Case When A.IsInsert = 'Y' Then 'true' else 'false' end as ProgAdd";
            //修改
            sql += ", Case When A.IsEdit = 'Y' Then 'true' else 'false' end as ProgMod";
            //刪除
            sql += ", Case When A.IsDelete = 'Y' Then 'true' else 'false' end as ProgDel";
            //檢視
            sql += ", Case When A.IsView = 'Y' Then 'true' else 'false' end as ProgView";

            sql += " From cRoleFunc A ";  //群組程式
            sql += " Join cFuncInfo B On A.FuncID = B.FuncID and B.DeviceTypeID IN ('C', 'O', 'G', 'T', 'P', 'H') "; //程式清單
            sql += " JOIN (SELECT FuncID, DispOrder From cFuncInfo ";
            sql += "Where DeviceTypeID in ('C', 'O', 'G', 'T', 'P', 'H') And ParentFuncID = '000' ";
            sql += "      ) C On B.ParentFuncID = C.FuncID "; //選單項目
            //查詢條件
            sql += " Where 1 = 1 ";
            //群組代號
            sql += " And A.RoleID = @RoleId ";
            
            //排序
            sql += " Order by C.FuncID, B.DispOrder";

            return Fetch<AuthRole_EditDetailGrid1>(@sql, qc);
        }

        public List<CodeName> GetDeviceType()
        {
            string sql = "";
            sql = "Select DeviceTypeID as code, DeviceTypeDesc as name ";
            sql += "\n" + " From cDeviceType ";
            sql += "\n" + "Where DeviceTypeID in ('C', 'O', 'G', 'T', 'P', 'H') ";
            return Fetch<CodeName>(@sql);
        }

        public List<CatCodeName> GetMenuId()
        {
            string sql = "";
            sql = "Select DeviceTypeID as cat, FuncID as code, FuncName as name";
            sql += "\n" + " From cFuncInfo ";
            sql += "\n" + "Where ParentFuncID = '000' And DeviceTypeID in ('C', 'O', 'G', 'T', 'P', 'H') ";
            sql += "\n" + "order by DispOrder, FuncID ";
            return Fetch<CatCodeName>(@sql);
        }

        public List<CatCodeName> GetProgId()
        {
            string sql = "";
            sql = "Select ParentFuncID as cat, FuncID as code, FuncName as name";
            sql += "\n" + " From cFuncInfo ";
            sql += "\n" + "Where ParentFuncID != '000' ";
            sql += "\n" + "order by ParentFuncID, DispOrder, FuncID ";

            return Fetch<CatCodeName>(@sql);
        }

        /// <summary>
        /// cAFCAccount 使用者檔 KEY值檢查
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public List<AjaxKeyCountResult> Check_Key(AuthRole_EditMain qc)
        {
            string sql = "";
            sql += "SELECT ";
            sql += " count(*) KEY_COUNT ";
            sql += " From cRoleInfo ";
            sql += " WHERE 1 = 1  ";
            sql += " and RoleID = @RoleId ";
            return Fetch<AjaxKeyCountResult>(@sql, qc);
        }
    }
}
