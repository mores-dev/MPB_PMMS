using MPB_Entities.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPB_DAL.Api
{
    public class QueryRegister_DAL : DALBase
    {
        public List<ORDER> GetOrders(string orderDate, string c_id)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT  ");
            sql.AppendLine("CONVERT(NVARCHAR,GR.ORDER_DATE,111) AS SHIPPING_DAT, ");
            sql.AppendLine(" GR.GR_NO AS ORDER_ID, ");
            sql.AppendLine(" GR.DTL_COUNT AS PEOPLE_CNT,  ");
            sql.AppendLine(" GR.CLOSE_STATUS AS REDEEM, ");
            sql.AppendLine(" GR.NAME AS CONTACT_NAME,  ");
            sql.AppendLine(" GR.PHONE AS CONTACT_PHONE, ");
            sql.AppendLine(" '' AS CONTACT_EMAIL,   ");
            sql.AppendLine("  GA.GA_TYPE AS CONTACT_TYPE, ");
            sql.AppendLine("  GA.UNI_ID AS AGENT_ID,  ");
            sql.AppendLine("  GA.NAME AS AGENT_NAME ");
            sql.AppendLine("FROM rGroupRecord GR ");
            sql.AppendLine("LEFT JOIN cGAAccount GA ON GR.CREATEID = GA.ACCOUNT ");
            sql.AppendLine("WHERE GR.C_ID LIKE '%'+@c_id+'%' ");
            sql.AppendLine("AND GR.ORDER_DATE = @order_date  ");

            return Fetch<ORDER>(sql.ToString(), new { order_date = orderDate, c_id = c_id });
        }

        public List<ORDER_DTL> GetOrderDtl(QueryRegisterDtl_FUNC qc)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT  ");
            sql.AppendLine("CONVERT(VARCHAR(10),GR.ORDER_DATE,120) AS SHIPPING_DATE, ");
            sql.AppendLine(" GR.GR_NO AS ORDER_ID, ");
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
            sql.AppendLine("AND GR.ORDER_DATE = @SHIPPING_DATE  ");
            if(!string.IsNullOrWhiteSpace(qc.ORDER_ID))
                sql.AppendLine("AND GR.GR_NO = @ORDER_ID");

            return Fetch<ORDER_DTL>(sql.ToString(), qc);
        }
    }
}
