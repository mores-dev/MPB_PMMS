using System;
using System.Collections.Generic;
using System.Linq;
using MPB_BLL.COMMON;
using MPB_DAL.FCM;
using MPB_Entities.COMMON;
using MPB_Entities.Helper;
using MPB_Entities.FCM;
using System.Web.Configuration;

namespace MPB_BLL.FCM
{
    public class PsgrUpload_QueryBLL : BLLBase
    {
        PsgrUpload_QueryDAL _dal = new PsgrUpload_QueryDAL();

        public PageList<PsgrUpload_QueryResult> GetPageList(PsgrUpload_QueryCondition qc)
        {
            return _dal.GetPageList(qc);
        }

        public void UploadData(string C_ID)
        {
            string DirName = WebConfigurationManager.AppSettings["BatchPath"].ToString();
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            string[] args = { "UPLOAD_MANIFEST", C_ID };
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.FileName = DirName + "BatchProc.exe ";
            proc.StartInfo.Arguments = String.Join(" ", args);
            try
            {
                proc.Start();
                proc.WaitForExit();
                proc.Close();
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
            }
        }
    }
}
