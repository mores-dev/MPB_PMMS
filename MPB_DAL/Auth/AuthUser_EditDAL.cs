
using MPB_Entities.Auth;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using System;
using System.Collections.Generic;

namespace MPB_DAL.Auth
{
    public class AuthUser_EditDAL : DALBase
    {
        public AuthUser_EditDAL() { }

        public AuthUser_EditDAL(DbManager db) : base(db) { }

        /// <summary>
        /// 讀取 cAFCAccount 使用者檔
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public AuthUser_EditMain Select_cAFCAccount(AuthUser_EditMain qc)
        {
            //page的table一定要寫化名(alias)，eg. Table t。也要Order by。

            string sql;
            sql = "";
            sql += "SELECT Top 1";
            //使用者代號
            sql += " A.ACCOUNT as UserID";
            //使用者類別
            sql += " , A.UserType AS UserType";
            //使用者名稱
            sql += " ,A.NAME as UserName";
            //使用者密碼
            //sql += " ,A.PASSWORD as Pwd";
            //單位
            sql += " ,A.DEPT";
            //是否啟用  
            sql += ", A.AccStatus as Status";
            //航商  
            sql += ", A.C_ID ";

            sql += ", B.RoleID";

            //資料建立者
            sql += " ,A.CREATEID + ' - ' + (SELECT U.NAME FROM cAFCAccount U WHERE U.ACCOUNT = A.CREATEID) CREATEID ";
            //資料建立日期時間
            sql += " ,CONVERT(VARCHAR, A.CREATEDT, 120)  as CREATEDT";
            //資料修改者
            sql += " ,A.MODIFYID + ' - ' + (SELECT U.NAME FROM cAFCAccount U WHERE U.ACCOUNT = A.CREATEID) MODIFYID ";
            //資料修改日期時間
            sql += " ,CONVERT(VARCHAR, A.MODIFYDT, 120)  as MODIFYDT";

            //FROM AND LEFT JOIN 
            sql += " FROM cAFCAccount   A    "; //使用者檔 
            sql += " Join cAccountRole B ON A.ACCOUNT = B.ACCOUNT AND B.DeviceTypeID = 'C' ";
            //查詢條件 
            sql += " WHERE 1=1 ";
            //查詢條件 使用者代號
            sql += " and A.ACCOUNT = @UserId ";

            return SingleOrDefault<AuthUser_EditMain>(@sql, qc);
        }

        public List<CodeName> GetRoleId()
        {
            string sql = "";
            sql = @"Select RoleID code, RoleName name ";
            sql += "  From cRoleInfo ";
            sql += " Where RoleStatus = 'Y' ";
            sql += " order by RoleID";
            return Fetch<CodeName>(@sql);
        }

        public List<AuthUserRole_EditDetailGrid1> Select_Grid1_AccountRole(AuthUser_EditMain qc)
        {
            string sql = "";

            sql += "Select ";
            //群組代號
            sql += " A.RoleID";
            //使用者代號
            sql += ", A.ACCOUNT as UserID";

            //FROM Table (Grid Main Table) && LEFT JOIN
            sql += " From cAccountRole A ";  //使用者群組
            //查詢條件
            sql += " Where 1 = 1 ";
            //查詢條件 帳號
            sql += " And A.ACCOUNT = @UserId ";
            //查詢條件 設備類型
            sql += " And A.DeviceTypeID = @DeviceTypeId ";
            //排序
            sql += " Order by A.RoleID";

            return Fetch<AuthUserRole_EditDetailGrid1>(@sql, qc);
        }

        /// <summary>
        /// cAFCAccount 使用者檔 KEY值檢查
        /// </summary>
        /// <param name="qc"></param>
        /// <returns></returns>
        public List<AjaxKeyCountResult> Check_Key(AuthUser_EditMain qc)
        {
            string sql = "";
            sql += "SELECT ";
            sql += " count(*) KEY_COUNT ";
            sql += " From cAFCAccount ";
            sql += " WHERE 1 = 1  ";
            //sql += " And DeviceTypeID = 'C' ";
            sql += " and ACCOUNT = @UserId ";
            return Fetch<AjaxKeyCountResult>(@sql, qc);
        }
    }
}
