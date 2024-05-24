using System.Collections.Generic;
using DataAccessUtility;

namespace MPB_Entities.Api
{
    [System.Web.Mvc.Bind(Exclude = "")]
    public class CheckGroupStatus_TVS_FUNC_Entities
    {
        public string GR_NO { get; set; }
    }

    public class CheckGroupStatus_TVS_RSPN_Entities
    {
        [Column("GR_NO")]
        public string GR_NO { get; set; }

        [Column("EDIT_LOCK")]
        public string EDIT_LOCK { get; set; }
    }
}
