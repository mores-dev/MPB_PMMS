using MPB_Entities.Ossl;
using System.Text;

namespace MPB_DAL.Ossl
{
    public class OsslTicket_DAL : DALBase
    {
        public OsslPersonalTicket Select_PersonalTicket(string tkNo)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine("SELECT");
            sql.AppendLine("PG_ID,");
            sql.AppendLine("TK_NO,");
            sql.AppendLine("ID_NO,");
            sql.AppendLine("ID_TYPE,");
            sql.AppendLine("[NAME],");
            sql.AppendLine("CONVERT(VARCHAR(10),BIRTHDAY,111) AS BIRTHDAY,");
            sql.AppendLine("PHONE");
            sql.AppendLine("FROM rPassengerTicket");
            sql.AppendLine("WHERE TK_NO = @TkNo");

            return SingleOrDefault<OsslPersonalTicket>(sql.ToString(), new { TkNo = tkNo });
        }

        public int Insert_PersonalTicket(OsslPersonalTicketEntites entites)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" INSERT INTO rPassengerTicket ");
            sql.AppendLine("            ( TK_NO  ");
            sql.AppendLine("            , ID_NO  ");
            sql.AppendLine("            , ID_TYPE  ");
            sql.AppendLine("            , NAME  ");
            sql.AppendLine("            , BIRTHDAY  ");
            sql.AppendLine("            , PHONE  ");
            sql.AppendLine("            , SOURCE");
            sql.AppendLine("            , C_ID  ");
            sql.AppendLine("            , ORDER_DATE  ");
            sql.AppendLine("            , CREATEDT  ");
            sql.AppendLine("            , CREATEID  ");
            sql.AppendLine("            , MODIFYDT  ");
            sql.AppendLine("            , MODIFYID ");
            sql.AppendLine("   )  OUTPUT inserted.PG_ID   VALUES ");
            sql.AppendLine("            ( @TK_NO, ");
            sql.AppendLine("             @ID_NO,  ");
            sql.AppendLine("             @ID_TYPE,  ");
            sql.AppendLine("             @NAME,  ");
            sql.AppendLine("             @BIRTHDAY, ");
            sql.AppendLine("             @PHONE,  ");
            sql.AppendLine("             @SOURCE,  ");
            sql.AppendLine("             @C_ID,  ");
            sql.AppendLine("             SYSDATETIME(), ");
            sql.AppendLine("             SYSDATETIME(), ");
            sql.AppendLine("             @CreateId,  ");
            sql.AppendLine("             SYSDATETIME(), ");
            sql.AppendLine("             @ModifyId ) ");


            return ExecuteScalar<int>(sql.ToString(), entites);
        }
        //更新小蜜蜂乘客資訊 不修改ORDER_DATE
        public int Update_PersonalTicket(OsslPersonalTicketEntites entites)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" UPDATE rPassengerTicket SET ");
            if(!string.IsNullOrEmpty(entites.ID_NO))
                sql.AppendLine("    ID_NO =  @ID_NO, ");
            if (!string.IsNullOrEmpty(entites.ID_TYPE))
                sql.AppendLine("    ID_TYPE =  @ID_TYPE,");
            if (!string.IsNullOrEmpty(entites.NAME))
                sql.AppendLine("    [NAME] =  @NAME, ");
            if (!string.IsNullOrEmpty(entites.BIRTHDAY))
                sql.AppendLine("    BIRTHDAY =  @BIRTHDAY, ");
            if (!string.IsNullOrEmpty(entites.PHONE))
                sql.AppendLine("    PHONE =  @PHONE, ");
            if (!string.IsNullOrEmpty(entites.SOURCE))
                sql.AppendLine("    SOURCE =  @SOURCE,");
            //sql.AppendLine("    ORDER_DATE =  SYSDATETIME(),");
            sql.AppendLine("    MODIFYDT =  SYSDATETIME(),");
            sql.AppendLine("    MODIFYID =  @ModifyId ");
            sql.AppendLine("WHERE TK_NO = @TK_NO");


            return Execute(sql.ToString(), entites);
        }
    }
}
