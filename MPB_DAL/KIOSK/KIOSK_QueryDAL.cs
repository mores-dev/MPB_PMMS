using MPB_Entities.KIOSK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPB_DAL.KIOSK
{
    public class KIOSK_QueryDAL : DALBase
    {
        public GroupDecord GetGroup(GroupPassengerDecordParam qry)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" SELECT ");
            sql.AppendLine(" GR_NO, ");
            sql.AppendLine(" GR_ID, ");
            sql.AppendLine(" ORDER_DATE, ");
            sql.AppendLine(" GR.PHONE CONTACT_PHONE, ");
            sql.AppendLine(" DTL_COUNT, ");
            sql.AppendLine(" USE_KIOSK, ");
            sql.AppendLine(" EDIT_LOCK, ");
            sql.AppendLine(" GR.CLOSE_STATUS, ");
            sql.AppendLine(" GA.GA_TYPE, ");
            sql.AppendLine(" RIGHT(GR.PHONE, 3) AS THREE_CODE ");
            sql.AppendLine(" FROM rGroupRecord GR ");
            sql.AppendLine(" LEFT JOIN cGAAccount GA ");
            sql.AppendLine(" ON GR.CREATEID = GA.ACCOUNT ");
            sql.AppendLine(" WHERE GR_NO = @GR_NO ");
            //sql.AppendLine(" AND RIGHT(GR.PHONE,3)  = @CONTACT_PHONE ");

            return SingleOrDefault<GroupDecord>(sql.ToString(), qry);
        }

        public List<GroupDecordDtl> GetPassengers(GroupPassengerDecordParam qry)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" SELECT  ");
            sql.AppendLine(" SEQNO,  ");
            sql.AppendLine(" GR_ID,  ");
            sql.AppendLine(" DTL.NAME,  ");
            sql.AppendLine(" CL1.CodeDesc AS ID_TYPE_STR,  ");
            sql.AppendLine(" ID_TYPE,  ");
            sql.AppendLine(" ID_NO, ");
            sql.AppendLine(" CONVERT(VARCHAR(10),BIRTHDAY,111) AS BIRTHDAY ");
            sql.AppendLine(" FROM rGroupRecordDtl DTL  ");
            sql.AppendLine(" LEFT JOIN cCodeList CL1 ON DTL.ID_TYPE = CL1.CodeID AND CL1.CodeKey = 'SYS01005' ");
            //sql.AppendLine(" LEFT JOIN cGAAccount GACC ON DTL.CREATEID = GACC.ACCOUNT");
            //sql.AppendLine(" INNER JOIN cCodeList CL2 ON CL2.CodeDesc LIKE '%' + GACC.GA_TYPE + '%' AND CL2.CodeKey = 'SYS01008' AND CL2.CodeID = @FuncCode ");           
            sql.AppendLine(" WHERE GR_ID = @GR_ID ");
            sql.AppendLine(" ORDER BY GRD_ID ");

            return Fetch<GroupDecordDtl>(sql.ToString(), qry);
        }

        public GroupPassengerDecord GetPassenger(GroupPassengerDecordParam qry)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT  ");
            sql.AppendLine("GR_ID, ");
            sql.AppendLine("GRD_ID, ");
            sql.AppendLine("NAME, ");
            sql.AppendLine("ID_TYPE, ");
            sql.AppendLine("ID_NO ");
            sql.AppendLine("FROM rGroupRecordDtl ");
            sql.AppendLine("WHERE GR_ID = @GR_ID ");
            sql.AppendLine("AND ID_TYPE = @ID_TYPE ");
            sql.AppendLine("AND ID_NO = @ID_NO ");

            return SingleOrDefault<GroupPassengerDecord>(sql.ToString(), qry);
        }

        public List<GroupPassengerDecord> GetGroupDtl(GroupPassengerDecordParam qry)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT  ");
            sql.AppendLine("GR_ID, ");
            sql.AppendLine("GRD_ID, ");
            sql.AppendLine("NAME, ");
            sql.AppendLine("ID_TYPE, ");
            sql.AppendLine("ID_NO ");
            sql.AppendLine("FROM rGroupRecordDtl ");
            sql.AppendLine("WHERE GR_ID = @GR_ID ");
            sql.AppendLine("AND ID_TYPE = @ID_TYPE ");

            return Fetch<GroupPassengerDecord>(sql.ToString(), qry);
        }

        public GroupPassengerDecord GetGroupPassenger(GroupPassengerDecordParam qry)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" SELECT");
            sql.AppendLine(" GR.GR_NO,");
            sql.AppendLine(" GR.GR_ID,");
            sql.AppendLine(" GR.C_ID,");
            sql.AppendLine(" GR.ORDER_DATE,");
            sql.AppendLine(" GR.PHONE CONTACT_PHONE, ");
            sql.AppendLine(" GR.DTL_COUNT, ");
            sql.AppendLine(" GR.CLOSE_STATUS,");
            sql.AppendLine(" DTL.GRD_ID,");
            sql.AppendLine(" DTL.ID_TYPE,");
            sql.AppendLine(" DTL.ID_NO,");
            sql.AppendLine(" DTL.NAME");
            sql.AppendLine(" FROM rGroupRecord GR");
            sql.AppendLine(" INNER JOIN rGroupRecordDtl DTL");
            sql.AppendLine(" ON GR.GR_ID = DTL.GR_ID");
            sql.AppendLine(" WHERE GR_NO = @GR_NO");
            sql.AppendLine(" AND RIGHT(GR.PHONE,3) = @CONTACT_PHONE");
            sql.AppendLine(" AND DTL.ID_TYPE = @ID_TYPE");
            sql.AppendLine(" AND DTL.ID_NO = @ID_NO");

            return SingleOrDefault<GroupPassengerDecord>(sql.ToString(), qry);
        }
    }
}
