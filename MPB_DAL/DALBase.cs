using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessUtility;
using MPB_Entities.Helper;

namespace MPB_DAL
{
    /// <summary>
    /// 資料存取的底層
    /// </summary>
    public class DALBase
    {
        /// <summary>
        /// NLog
        /// </summary>
        protected NLog.Logger Logger;

        /// <summary>
        /// DbManager
        /// </summary>
        protected DbManager _db;

        /// <summary>
        /// 每頁固定的筆數
        /// </summary>
        private const int _itemsPerPage = 10;

        /// <summary>
        /// 資料存取的底層建構子(傳入DbMangaer，可用來作Transaction使用)
        /// </summary>
        /// <param name="db"></param>
        public DALBase(DbManager db)
        {
            _db = db;
        }

        /// <summary>
        /// 資料存取的底層建構子
        /// </summary>
        public DALBase()
        {
            _db = DbManager.GetInstance();
        }

        /// <summary>
        /// 取得所有資料列
        /// </summary>
        public bool GetAllRecord { get; set; }

        //public PageList<T> PageList<T>(long page, string sqlCount, object[] countArgs, string sqlPage, object[] pageArgs, long itemsPerPage = _itemsPerPage)
        //{
        //    Page<T> result = _db.Page<T>(page, itemsPerPage, sqlCount, countArgs, sqlPage,
        //        pageArgs);

        //    return ConvertToPageList<T>(result);
        //}

        /// <summary>
        /// 分頁
        /// </summary>
        /// <typeparam name="T">返回的type</typeparam>
        /// <param name="page">頁數(從1開始)</param>
        /// <param name="sql">sql(table命名一定要寫alias)</param>
        /// <param name="itemsPerPage">每頁幾筆</param>
        /// <param name="args">參數</param>
        /// <returns>PageList</returns>
        public PageList<T> PageList<T>(long page, string sql, long itemsPerPage, params object[] args)
        {
            page = page < 1 ? 1 : page;//頁數最小為1
            Page<T> result = _db.Page<T>(page, itemsPerPage, sql, args);

            return ConvertToPageList<T>(result);
        }

        /// <summary>
        /// 分頁
        /// </summary>
        /// <typeparam name="T">返回的type</typeparam>
        /// <param name="page">頁數(從1開始)</param>
        /// <param name="sql">sql(table命名一定要寫alias)</param>
        /// <param name="args">參數</param>
        /// <returns>PageList</returns>
        public PageList<T> PageList<T>(long page, string sql, params object[] args)
        {
            page = page < 1 ? 1 : page;//頁數最小為1
            long itemsPerPage = _itemsPerPage;
            if (GetAllRecord)
            {
                page = 1;
                itemsPerPage = 65530;
            }

            return this.PageList<T>(page, sql, itemsPerPage, args);
        }

        private PageList<T> ConvertToPageList<T>(Page<T> source)
        {
            return new PageList<T>()
            {
                Context = source.Context,
                CurrentPage = source.CurrentPage,
                Items = source.Items,
                ItemsPerPage = source.ItemsPerPage,
                TotalItems = source.TotalItems,
                TotalPages = source.TotalPages,
            };
        }

        /// <summary>
        /// 返品符合結果的第一筆，若無則返回預設值
        /// </summary>
        /// <typeparam name="T">返回的type</typeparam>
        /// <param name="sql">sql</param>
        /// <param name="args">參數</param>
        /// <returns>返回type為T的結果</returns>
        public T SingleOrDefault<T>(string sql, params object[] args)
        {
            return _db.SingleOrDefault<T>(sql, args);
        }

        /// <summary>
        /// 執行不需返回結果的sql
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="args">參數</param>
        /// <returns>筆數</returns>
        public int Execute(string sql, params object[] args)
        {
            return _db.Execute(sql, args);
        }

        /// <summary>
        /// 執行返回單一欄位值的sql
        /// </summary>
        /// <typeparam name="T">返回的type</typeparam>
        /// <param name="sql">sql</param>
        /// <param name="args">參數</param>
        /// <returns>單一欄位值</returns>
        public T ExecuteScalar<T>(string sql, params object[] args)
        {
            return _db.ExecuteScalar<T>(sql, args);
        }

        /// <summary>
        /// 執行返回List&lt;T&gt;的sql
        /// </summary>
        /// <typeparam name="T">返回的type</typeparam>
        /// <param name="sql">sql</param>
        /// <param name="args">參數</param>
        /// <returns>List&lt;T&gt;</returns>
        public List<T> Fetch<T>(string sql, params object[] args)
        {
            return _db.Fetch<T>(sql, args);
        }

        /// <summary>
        /// 執行返回IEnumerable&lt;T&gt;的sql
        /// </summary>
        /// <typeparam name="T">返回的type</typeparam>
        /// <param name="sql">sql</param>
        /// <param name="args">參數</param>
        /// <returns>IEnumerable&lt;T&gt;</returns>
        public IEnumerable<T> Query<T>(string sql, params object[] args)
        {
            return _db.Query<T>(sql, args);
        }

        public void ExecuteProcedure(string procName, params DbParameter[] args)
        {
            _db.ExecuteProcedure(procName, args);
        }

        public DataSet ExecuteStoredProcedure(string procName, params DbParameter[] args)
        {
            return _db.ExecuteStoredProcedure(procName, args);
        }
    }
}
