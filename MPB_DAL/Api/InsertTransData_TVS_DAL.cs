using MPB_Entities.Api;
using System.Collections.Generic;

namespace MPB_DAL.Api
{
    public class InsertTransData_TVS_DAL : DALBase
    {
        public InsertTransData_TVS_DAL() { }

        public InsertTransData_TVS_DAL(DbManager db) : base(db) { }

        public int CheckDetail(InsertTransData_TVS_FUNC_Entities qc)
        {
            string sql;
            sql = "";
            sql += "SELECT ";
            
            sql += " COUNT(*) AS CNT ";

            //FROM AND LEFT JOIN 
            sql += " FROM pPsgrManifestDtl A ";

            //查詢條件 
            sql += " WHERE 1=1 ";
            if (qc.QR_TYPE == "1")
                sql += "   And A.ODR_NO = @ODR_NO ";
            else
                sql += "   And A.QRCODE = @QRCODE ";
            sql += "   And A.ST_ID = @ST_ID ";

            if (qc.QR_TYPE == "1")
                sql += " GROUP BY A.ODR_NO, A.ST_ID";
            else
                sql += " GROUP BY A.QRCODE, A.ST_ID";

            return SingleOrDefault<int>(@sql, qc);
        }

        public int CheckMaster(InsertTransData_TVS_FUNC_Entities qc)
        {
            string sql;
            sql = "";
            sql += "SELECT ";

            sql += " COUNT(*) AS CNT ";

            //FROM AND LEFT JOIN 
            sql += " FROM pPsgrManifest A ";

            //查詢條件 
            sql += " WHERE 1=1 ";
            sql += "   And A.BUSINESS_DATE = @BUSINESS_DATE ";
            sql += "   And A.R_ID = @R_ID ";
            sql += "   And A.SC_TIME = @SC_TIME ";
            sql += "   And A.V_ID = @V_ID ";
            sql += " GROUP BY A.BUSINESS_DATE, A.R_ID, A.SC_TIME, A.V_ID";

            return SingleOrDefault<int>(@sql, qc);
        }

        public int Insert_Master(InsertTransData_TVS_FUNC_Entities qc)
        {
            string sql;
            sql = "";
            sql = "Insert into pPsgrManifest (";
            //乘船日期
            sql += "BUSINESS_DATE";
            //航線序號
            sql += ", R_ID";
            //航班時刻
            sql += ", SC_TIME";
            //船隻序號
            sql += ", V_ID";
            //航商序號
            sql += ", C_ID";
            //場站序號
            sql += ", ST_ID";
            //航班序號
            sql += ", SC_ID";
            //設備編號
            sql += ", DEVICE_ID";

            //reserve columns
            sql += "\n";
            //資料建立紀錄
            sql += ", CREATEDT, CREATEID, MODIFYDT, MODIFYID)";

            sql += "\n" + " Select ";
            //乘船日期
            sql += "  @BUSINESS_DATE";
            //航線序號
            sql += ", @R_ID";
            //航班時刻
            sql += ", @SC_TIME";
            //船隻序號
            sql += ", @V_ID";
            //航商序號
            sql += ", @C_ID";
            //場站序號
            sql += ", @ST_ID";
            //航班序號
            sql += ", @SC_ID";
            //設備編號
            sql += ", @DEVICE_ID";

            //reserve columns
            sql += "\n";
            //資料建立紀錄
            sql += ", @CREATEDT, @CREATEID, @CREATEDT, @CREATEID ";

            return Execute(@sql, qc);
        }

        public int Insert_Detail(InsertTransData_TVS_FUNC_Entities qc)
        {
            string sql;
            sql = "";
            sql = "Insert into pPsgrManifestDtl (";
            //乘船日期
            sql += "BUSINESS_DATE";
            //航線序號
            sql += ", R_ID";
            //航班時刻
            sql += ", SC_TIME";
            //船隻序號
            sql += ", V_ID";
            //條碼
            sql += ", QRCODE";
            //條碼類型
            sql += ", QR_TYPE";
            //場站序號
            sql += ", ST_ID";
            //總人數
            sql += ", ALLOW_CNT";
            //座位人數
            sql += ", SEAT_CNT";
            //票券類型 0:未知  1:單趟  2:來回
            sql += ", TICKET_TYPE";
            //票券名稱
            sql += ", TICKET_NAME";
            //證件號碼
            sql += ", ID_NO";
            //證件類型
            sql += ", ID_TYPE";
            //乘客姓名
            sql += ", PSGR_NAME";
            //性別
            sql += ", SEX";
            //出生年月日
            sql += ", BIRTHDAY";
            //預約單號
            sql += ", ODR_NO";

            //reserve columns
            sql += "\n";
            //資料建立紀錄
            sql += ", CREATEDT, CREATEID, MODIFYDT, MODIFYID)";

            sql += "\n" + " Select ";
            //乘船日期
            sql += "  @BUSINESS_DATE";
            //航線序號
            sql += ", @R_ID";
            //航班時刻
            sql += ", @SC_TIME";
            //船隻序號
            sql += ", @V_ID";
            //條碼
            sql += ", @QRCODE";
            //條碼類型
            sql += ", @QR_TYPE";
            //場站序號
            sql += ", @ST_ID";
            //總人數
            sql += ", @ALLOW_CNT";
            //座位人數
            sql += ", @SEAT_CNT";
            //票券類型 0:未知  1:單趟  2:來回
            sql += ", @TICKET_TYPE";
            //票券名稱
            sql += ", @TICKET_NAME";
            //證件號碼
            sql += ", @ID_NO";
            //證件類型
            sql += ", @ID_TYPE";
            //乘客姓名
            sql += ", @PSGR_NAME";
            //性別
            sql += ", @SEX";
            //出生年月日
            sql += ", @BIRTHDAY";
            //預約單號
            sql += ", @ODR_NO";

            //reserve columns
            sql += "\n";
            //資料建立紀錄
            sql += ", @CREATEDT, @CREATEID, @MODIFYDT, @MODIFYID ";

            return Execute(@sql, qc);
        }

        public int Update_Detail(InsertTransData_TVS_FUNC_Entities qc)
        {
            string sql;
            sql = "";
            sql = "Update pPsgrManifestDtl Set ";
            //總人數
            sql += " ALLOW_CNT = @ALLOW_CNT";
            //座位人數
            sql += ", SEAT_CNT = @SEAT_CNT";
            //票券類別
            sql += ", TICKET_TYPE = @TICKET_TYPE";
            //票券名稱
            sql += ", TICKET_NAME = @TICKET_NAME";
            //證件號碼
            sql += ", ID_NO = @ID_NO";
            //證件類型
            sql += ", ID_TYPE = @ID_TYPE";
            //乘客姓名
            sql += ", PSGR_NAME = @PSGR_NAME";
            //性別
            sql += ", SEX = @SEX";
            //出生年月日
            sql += ", BIRTHDAY = @BIRTHDAY";

            //資料修改紀錄
            sql += ", MODIFYDT = @MODIFYDT";
            sql += ", MODIFYID = @MODIFYID";

            sql += " WHERE 1=1 ";
            sql += "   And BUSINESS_DATE = @BUSINESS_DATE ";
            sql += "   And R_ID = @R_ID ";
            sql += "   And SC_TIME = @SC_TIME ";
            sql += "   And V_ID = @V_ID ";
            sql += "   And QRCODE = @QRCODE ";

            return Execute(@sql, qc);
        }

        public int Delete_Detail(InsertTransData_TVS_FUNC_Entities qc)
        {
            string sql;
            sql = "";
            sql = "Delete pPsgrManifestDtl ";

            sql += " WHERE 1=1 ";
            sql += "   And BUSINESS_DATE = @BUSINESS_DATE ";
            sql += "   And R_ID = @R_ID ";
            //sql += "   And SC_TIME = @SC_TIME ";
            //sql += "   And V_ID = @V_ID ";
            sql += "   And QRCODE = @QRCODE ";

            return Execute(@sql, qc);
        }

        public int Delete_ManifestDetail(InsertTransData_TVS_FUNC_Entities qc)
        {
            string sql;
            sql = "";
            sql = "Delete cManifestDtl ";

            sql += " WHERE 1=1 ";
            sql += "   And SHIPPING_DATE = @BUSINESS_DATE ";
            if (qc.R_ID == "R00001")
                sql += "   And STATION = 'D' ";
            else if (qc.R_ID == "R00002")
                sql += "   And STATION = 'L' ";

            //sql += "   And SC_TIME = @SC_TIME ";
            //sql += "   And V_ID = @V_ID ";
            sql += "   And TICKET_NO = @QRCODE ";

            return Execute(@sql, qc);
        }
    }
}
