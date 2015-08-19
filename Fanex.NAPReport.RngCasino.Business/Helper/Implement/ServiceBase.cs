namespace Fanex.NAPReport.RngCasino.Business.Helper.Implement
{
    using System;
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using Fanex.Data.Repository;
    using Fanex.NAPReport.RngCasino.Business.Helper.Interface;
    using Fanex.Service.Core.Extensions;
    using Fanex.Service.Core.MailService;

    public abstract class ServiceBase : TimerFunctionMailServiceBase, IServiceBase
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
            LogExtensions.ExceptionHandler(ex);
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