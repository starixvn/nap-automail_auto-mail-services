// -----------------------------------------------------------------------
// <copyright file="ServiceBase.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Robot.Report.RngCasino.Helper.Implement
{
    using System;
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using Fanex.NAP.Framework.Data;
    using Robot.Report.RngCasino.Helper;
    using Robot.Report.RngCasino.Helper.Interface;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ServiceBase : IServiceBase
    {
        protected readonly IDynamicRepository DynamicRepository;

        public ServiceBase(IDynamicRepository repository = null)
        {
            DynamicRepository = repository ?? new DynamicRepository();
        }

        protected IDynamicRepository Repository
        {
            get
            { 
                return DynamicRepository; 
            }
        }

        /// <summary>
        /// Handler Exception.
        /// </summary>
        /// <param name="ex">Type exception.</param>
        public void HandleException(Exception ex)
        {
            LogHelper.Log(ex);
        }

        /// <summary>
        /// Get Report Data with Criteria.
        /// </summary>
        /// <typeparam name="T">Type of Object return.</typeparam>
        /// <param name="criteria">Criteria to Query DB.</param>
        /// <returns>Enumerable of Type T.</returns>
        public virtual IEnumerable<T> GetReportData<T>(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");
            try
            {
                if (criteria.IsValid())
                {
                    return DynamicRepository.Fetch<T>(criteria);
                }

                return new List<T>();
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return null;
        }
    }
}
