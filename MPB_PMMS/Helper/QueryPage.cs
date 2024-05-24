
namespace MPB_PMMS.Helper
{
    /// <summary>
    /// 查詢頁面共用程式
    /// </summary>
    public class QueryPage
    {

        /// <summary>
        /// 表格分頁
        /// </summary>
        /// <param name="TotalPage">總頁數</param>
        /// <param name="ToPage">目前頁面</param>
        /// <param name="TotalCount">總筆數</param>
        /// <returns></returns>
        public static string CreatePageList(long TotalPage, long ToPage, long TotalCount)
        {
            long go_page_first = 1;
            long go_page_last = TotalPage <= 0 ? 1 : TotalPage;
            long go_page_prev = ToPage <= 1 ? 1 : ToPage - 1;
            long go_page_next = ToPage < go_page_last ? ToPage + 1 : go_page_last;
            long page_max_length = TotalPage.ToString().Length;
            string Rtn = "<div class='txtBlack noWrap' style='margin-right:13px' align='right'>";
            Rtn += "共" + TotalCount + "筆，目前在";
            if (TotalPage > 5)
            {
                int inputWidth = 50 + (int)(page_max_length * 10);
                Rtn += "第<input type='text' style='width:" + inputWidth + "px' id='ToPage' class='vs_initialForm({M:" + page_max_length + ",S:" + (1 + page_max_length) + ",N:1,TA:c})' value='" + ToPage + "'>頁";
            }
            else
            {
                Rtn += "<select id='ToPage'>";
                for (long i = 1; i <= TotalPage; i++)
                {
                    Rtn += "<option value='" + i + "' " + ((i == ToPage) ? "Selected" : "") + ">第" + i + "頁</option>";
                }
                Rtn += "</select>";
            }
            Rtn += "/" + TotalPage + "頁";

            if (TotalPage > 1)
            {
                bool first = ToPage == 1;
                bool last = ToPage >= TotalPage;
                Rtn += "<input type='hidden' id='isLastPage'>";
                Rtn += first ? "<button id='btn_page_first' disabled>一</button><button id='btn_page_prev' disabled>上</button>"
                    : "<button id='btn_page_first'>一</button><button id='btn_page_prev'>上</button>";
                Rtn += last ? "<button id='btn_page_next' disabled>下</button><button id='btn_page_last' disabled>末</button>"
                    : "<button id='btn_page_next'>下</button><button id='btn_page_last'>末</button>";
            }
            else
                Rtn += "<input type='hidden' id='isLastPage'><button id='btn_page_first' disabled>一</button><button id='btn_page_prev' disabled>上</button><button id='btn_page_next' disabled>下</button><button id='btn_page_last' disabled>末</button>";
            
            Rtn += "</div>";
            Rtn += "<script>\n";
            Rtn += "var pagelist_go_page_first=" + go_page_first + ";";
            Rtn += "var pagelist_go_page_prev=" + go_page_prev + ";";
            Rtn += "var pagelist_go_page_next=" + go_page_next + ";";
            Rtn += "var pagelist_go_page_last=" + go_page_last + ";";
            Rtn += "</script>\n";
            Rtn += "<script src=\"/Scripts/jquery.vs.pagelist.js\"></script>";
            return Rtn;

        }

    }
}