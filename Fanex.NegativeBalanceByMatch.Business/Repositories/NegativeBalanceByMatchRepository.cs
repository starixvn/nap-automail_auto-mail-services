// -----------------------------------------------------------------------
// <copyright file="NegativeBalanceByMatchRepository.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.NegativeBalanceByMatch.Business.Repositories
{
    using System;
    using System.Collections.Generic;
    using Fanex.AccountingReports.SqLiteDatabase;
    using Fanex.NegativeBalanceByMatch.Business.Entities;
    using Fanex.Service.Core.Enums;
    using Fanex.Service.Core.Extensions;
    using Fanex.Common;
    using System.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NegativeBalanceByMatchRepository : INegativeBalanceByMatchRepository
    {
        private SqLite _db = null;

        public NegativeBalanceByMatchRepository()
        {
            _db = new SqLite(AppSettings.ConnectionString);
        }

        public int InsertTask(NegativeBalanceTaskInfo task)
        {
            int id = 0;
            try
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("Status", ((int)Status.Running).ToString());
                data.Add("CreatedDate", DateTime.Now.ToString());
                id = Converter.ToInt(_db.Insert("Log", data));
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }

            return id;
        }

        /// <summary>
        /// Updates the task status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="status">The status.</param>
        public void UpdateTaskStatus(int id, Status status)
        {
            try
            {
                Dictionary<string, string> data = new Dictionary<string, string>();
                data.Add("Status", ((int)status).ToString());
                data.Add("UpdatedDate", DateTime.Now.ToString());

                string where = " Id = " + id;
                _db.Update("Log", data, where);
            }
            catch (Exception ex)
            {
                LogExtensions.ExceptionHandler(ex);
                throw;
            }
        }
    }
}