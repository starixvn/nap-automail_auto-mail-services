// -----------------------------------------------------------------------
// <copyright file="GameWinLimitRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NAPReport.GameWinLimitControl.Business.Repositories
{
    using System.Data;
    using Fanex.Data.Repository;
    using Fanex.Data;
    using Fanex.Common;
    using Fanex.Service.Core.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GameWinLimitRepository : IGameWinLimitRepository
    {
        public DataTable GetMemberWinLimit(CriteriaBase criteria)
        {
            var dataTable = new DataTable();
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                var dataset = db.ExecuteDataSet();
                var output = db.GetOuputParams();

                int errcode = Converter.ToInt(output["errcode"]);
                if (errcode == 0 && dataset.Tables.Count > 0)
                {
                    dataTable = dataset.Tables[0];
                }
            }

            if (AppSettings.IsSimulated)
            {
                dataTable = CreateDataMemberWinLimit();
            }

            return dataTable;
        }

        private DataTable CreateDataMemberWinLimit()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Currency", typeof(string));
            dt.Columns.Add("Username", typeof(string));
            dt.Columns.Add("Username2", typeof(string));
            dt.Columns.Add("CustId", typeof(int));
            dt.Columns.Add("ProductId", typeof(int));
            dt.Columns.Add("ProductName", typeof(string));
            dt.Columns.Add("SiteName", typeof(string));
            dt.Columns.Add("MaxWinLimit", typeof(double));
            dt.Columns.Add("TotalWinAmt", typeof(double));
            dt.Columns.Add("Percentage", typeof(double));

            DataRow row = dt.NewRow();
            row["ProductName"] = "VIRTUAL SPOPRTS";
            row["ProductId"] = "1";
            row["Username"] = "Member1";
            row["CustId"] = "45666";
            row["Currency"] = "USD";
            row["MaxWinLimit"] = "10030";
            row["TotalWinAmt"] = "1050";
            row["Percentage"] = "1.05";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["ProductName"] = "VIRTUAL SPOPRTS";
            row["ProductId"] = "1";
            row["Username"] = "Member2";
            row["CustId"] = "456654";
            row["Currency"] = "MYR";
            row["MaxWinLimit"] = "12300";
            row["TotalWinAmt"] = "1300";
            row["Percentage"] = "1.0833";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["ProductName"] = "NUMBER GAME";
            row["ProductId"] = "2";
            row["Username"] = "Member1";
            row["CustId"] = "45666";
            row["Currency"] = "USD";
            row["MaxWinLimit"] = "10040";
            row["TotalWinAmt"] = "1050";
            row["Percentage"] = "1.05";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["ProductName"] = "NUMBER GAME";
            row["ProductId"] = "2";
            row["Username"] = "Member2";
            row["CustId"] = "456654";
            row["Currency"] = "MYR";
            row["MaxWinLimit"] = "13200";
            row["TotalWinAmt"] = "13030";
            row["Percentage"] = "1.0833";
            dt.Rows.Add(row);

            return dt;
        }

        public Result UpdateNotificationMemberWinLimit(CriteriaBase criteria)
        {
            Result result = new Result();

            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                db.ExecuteNonQuery();

                var output = db.GetOuputParams();
                result.ErrCode = Converter.To<int>(output["errcode"]);
                result.ErrMsg = Converter.To<string>(output["errmsg"]);
            }

            return result;
        }
    }
}
