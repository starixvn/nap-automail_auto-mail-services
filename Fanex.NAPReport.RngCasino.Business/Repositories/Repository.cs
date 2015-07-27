// -----------------------------------------------------------------------
// <copyright file="Repository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.RngCasino.Business.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.AccountingReports.SqLiteDatabase;
    using Fanex.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Repository : IRepository
    {
        private SqLite db;
        public Repository()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\SiteConfig\RNGCasinoWinLoss\SQLite\AutoMail-RNGCasinoWinLoss.s3db";
            string connectString = "Data Source={0};Version=3;";
            connectString = string.Format(connectString, path);
            db = new SqLite(connectString); 
        }

        public void UpdateLastTime()
        {
            string sql = "select * from log";
            var dt = db.GetDataTable(sql);
            if (dt.Rows.Count == 0)
            {
                sql = "Insert into Log(LastTime) values('" + DateTime.Now.ToString() + "')";
            }
            else
            {
                sql = "update Log set LastTime = '"+ DateTime.Now.ToString() +"'";
            }

            db.ExecuteNonQuery(sql);
        }

        public DateTime GetLastTime()
        {
            DateTime lastTime = DateTime.Now.AddMinutes(-60);

            string sql = "select * from log";
            var dt = db.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                lastTime = Converter.To<DateTime>(dt.Rows[0]["LastTime"]);
            }

            return lastTime;
        }
    }
}
