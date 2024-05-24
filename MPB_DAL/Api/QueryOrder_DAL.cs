using MPB_Entities.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPB_DAL.Api
{
    public class QueryOrder_DAL : DALBase
    {
        public List<QUERY_ORDER_DTL> GetOrderDtl(QueryOrder_FUNC_Entities qc)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT  ");
            sql.AppendLine("CONVERT(VARCHAR(10),GR.ORDER_DATE,120) AS SHIPPING_DATE, ");
            sql.AppendLine(" GR.GR_NO AS ORDER_ID, ");
            sql.AppendLine(" GR.PHONE  AS CONTACT_PHONE, ");
            sql.AppendLine(" RIGHT(GR.PHONE, 3) AS THREE_CODE, ");
            sql.AppendLine(" GR.DTL_COUNT AS PEOPLE_CNT,  ");
            sql.AppendLine(" GR.CLOSE_STATUS AS REDEEM, ");
            sql.AppendLine(" GA.GA_TYPE AS ORDER_TYPE,");
            sql.AppendLine(" GA.UNI_ID AS AGENT_ID,  ");
            sql.AppendLine(" GA.NAME AS AGENT_NAME,");
            sql.AppendLine(" GRD.ID_TYPE,");
            sql.AppendLine(" GRD.ID_NO,");
            sql.AppendLine(" GRD.NAME,");
            sql.AppendLine(" GRD.BIRTHDAY AS BIRTH,");
            sql.AppendLine(" GRD.PHONE");
            sql.AppendLine("FROM rGroupRecord GR ");
            sql.AppendLine("LEFT JOIN cGAAccount GA ON GR.CREATEID = GA.ACCOUNT ");
            sql.AppendLine("INNER JOIN (");
            sql.AppendLine("	SELECT ");
            sql.AppendLine("	GR_ID,");
            sql.AppendLine("	ID_TYPE,");
            sql.AppendLine("	ID_NO,");
            sql.AppendLine("	[NAME],");
            sql.AppendLine("	CONVERT(VARCHAR(10),BIRTHDAY,120) AS BIRTHDAY,");
            sql.AppendLine("	PHONE");
            sql.AppendLine("	FROM rGroupRecordDtl ");
            sql.AppendLine(") AS GRD ON GR.GR_ID = GRD.GR_ID");
            sql.AppendLine("WHERE GR.C_ID LIKE '%'+ @C_ID +'%' ");
            //出發日判斷移除 改由程式判斷 出發日非當日跳錯誤訊息
            //sql.AppendLine("AND GR.ORDER_DATE = @SHIPPING_DATE  ");
            if (!string.IsNullOrWhiteSpace(qc.ORDER_ID))
                sql.AppendLine("AND GR.GR_NO = @ORDER_ID");
            else if (string.IsNullOrWhiteSpace(qc.ID_NO))
                sql.AppendLine("AND 1 != 1"); 

            return Fetch<QUERY_ORDER_DTL>(sql.ToString(), qc);
        }
    }
}
