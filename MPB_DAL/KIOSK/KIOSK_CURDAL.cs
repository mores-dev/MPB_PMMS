using MPB_Entities.KIOSK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPB_DAL.KIOSK
{
    public class KIOSK_CURDAL : DALBase
    {
        public KIOSK_CURDAL()
        {

        }

        public KIOSK_CURDAL(DbManager db): base(db) { }

        /// <summary>
        /// 取得新的排序編號
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public int Get_NewSeqNo(KIOSK_CUR passenger)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("  SELECT ISNULL(Max(SEQNO), 0)+1 as SEQNO FROM rGroupRecordDtl GROUP BY  GR_ID HAVING GR_ID= @GR_ID ");
            return SingleOrDefault<int>(sql.ToString(), passenger);
        }

        /// <summary>
        /// 新增一筆乘客
        /// </summary>
        /// <param name="passenger"></param>
        /// <returns></returns>
        public int Insert_Passenger(KIOSK_CUR passenger)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("INSERT INTO rGroupRecordDtl ");
            sql.AppendLine("(GR_ID, SEQNO, NAME, ID_NO, ID_TYPE, BIRTHDAY, CREATEID, CREATEDT, MODIFYID, MODIFYDT)");
            sql.AppendLine("OUTPUT Inserted.GRD_ID VALUES ");
            sql.AppendLine("(@GR_ID, @SEQNO, @NAME, @ID_NO, @ID_TYPE, @BIRTHDAY, @CreateId, SysDateTime(), @ModifyId, SysDateTime())");

            return ExecuteScalar<int>(sql.ToString(), passenger);
        }

        /// <summary>
        /// 刪除一筆乘客
        /// </summary>
        /// <param name="passenger"></param>
        /// <returns></returns>
        public int Delete_Passenger(KIOSK_CUR grdId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" DELETE rGroupRecordDtl WHERE GRD_ID = @GRD_ID ");
        

            return Execute(sql.ToString(), grdId);
        }

        /// <summary>
        /// 修改乘客
        /// </summary>
        /// <param name="passenger"></param>
        /// <returns></returns>
        public int Update_Passenger(KIOSK_CUR passenger)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE rGroupRecordDtl SET ");
            if(!string.IsNullOrWhiteSpace(passenger.NAME))
                sql.AppendLine("NAME = @NAME ,");
            if (!string.IsNullOrWhiteSpace(passenger.ID_TYPE))
                sql.AppendLine("ID_TYPE = @ID_TYPE ,");
            if (!string.IsNullOrWhiteSpace(passenger.ID_NO))
                sql.AppendLine("ID_NO = @ID_NO,");
            if (!string.IsNullOrWhiteSpace(passenger.BIRTHDAY))
                sql.AppendLine("BIRTHDAY = @BIRTHDAY,");
            sql.AppendLine("MODIFYID = @ModifyId,");
            sql.AppendLine("MODIFYDT = SysDateTime()");
            sql.AppendLine("WHERE GRD_ID  = @GRD_ID");

            return Execute(sql.ToString(), passenger);
        }

        /// <summary>
        /// 修改團體
        /// </summary>
        /// <param name="passenger"></param>
        /// <returns></returns>
        public int Update_Group(KIOSK_CUR group)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE rGroupRecord SET ");
            if (!string.IsNullOrWhiteSpace(group.NAME))
                sql.AppendLine("NAME = @NAME ,");
            if (!string.IsNullOrWhiteSpace(group.PHONE))
                sql.AppendLine("PHONE = @PHONE ,");
            if (!string.IsNullOrWhiteSpace(group.CLOSE_STATUS))
                sql.AppendLine("CLOSE_STATUS = @CLOSE_STATUS,");
            sql.AppendLine("MODIFYID = @ModifyId,");
            sql.AppendLine("MODIFYDT = SysDateTime()");
            sql.AppendLine("WHERE GR_NO  = @GR_NO AND ORDER_DATE = @ORDER_DATE ");

            return Execute(sql.ToString(), group);
        }

        /// <summary>
        /// 修改乘客人數
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public int Update_DtlCount(KIOSK_CUR grId)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("UPDATE rGroupRecord SET DTL_COUNT = C.CNT OUTPUT Inserted.DTL_COUNT ");
            sql.AppendLine("FROM ");
            sql.AppendLine("(SELECT GR_ID,COUNT(GR_ID) AS CNT  FROM rGroupRecordDtl  ");
            sql.AppendLine("GROUP BY GR_ID ");
            sql.AppendLine("HAVING GR_ID = @GR_ID) AS C ");
            sql.AppendLine("WHERE rGroupRecord.GR_ID = @GR_ID ");

            return ExecuteScalar<int>(sql.ToString(), grId);
        }
    }
}
