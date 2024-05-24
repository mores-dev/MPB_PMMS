using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPB_Entities.COMMON
{
    public class ResponseModel
    {
        public string code { get; set; }

        public string msg { get; set; }

        public object resultObj { get; set; }

        public ResponseModel Ok()
        {
            this.resultObj = null;
            this.msg = "";
            this.code = "00";
            return this;
        }

        public ResponseModel Ok(object resultObj)
        {
            this.resultObj = resultObj;
            this.msg = "";
            this.code = "00";
            return this;
        }

        public ResponseModel Error(string code, string msg)
        {
            this.code = code;
            this.msg = msg;
            this.resultObj = null;
            return this;
        }
    }

    public class ErrorModel
    {
        public ErrorModel()
        {
            
        }

        public ErrorModel(string _key, string _msg)
        {
            this.key = _key;
            this.msg = _msg;
        }

        public string key { get; set; }

        public string msg { get; set; }
    }


    public class ExcelFormatErrModel
    {
        public ExcelFormatErrModel(int row, int column)
        {
            this.row = row;
            this.column = column;
        }

        public int row { get; set; }
        public int column { get; set; }
        public string msg { get; set; }

        public string covertMsg()
        {
            return string.Format("第{0}列 第{1}行 {2}" , this.row, this.column , this.msg);
        }
    }
}
