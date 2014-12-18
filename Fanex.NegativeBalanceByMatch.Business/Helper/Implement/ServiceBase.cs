using Fanex.Common.Helper;
using System;
using System.Collections.Generic;
using Fanex.NAP.Framework.Data;
using Fanex.NegativeBalanceByMatch.Business.Helper.Interface;

namespace Fanex.NegativeBalanceByMatch.Business.Helper.Implement
{
    public class ServiceBase: IServiceBase
    {
        protected readonly IDynamicRepository DynamicRepository;

        public ServiceBase(IDynamicRepository repository = null)
        {
            DynamicRepository = repository ?? new DynamicRepository();
        }

        /// <summary>
        ///  Get dynamic repository
        /// </summary>
        protected IDynamicRepository Repository
        {
            get { return DynamicRepository; }
        }

        /// <summary>
        /// Handler Exception
        /// </summary>
        /// <param name="ex"></param>
        public void HandleException(Exception ex)
        {
            //LogService.Error(ex.Message, ex);
            LogHandler.Log(ex);
        }

        /// <summary>
        /// Get Report Data with Criteria
        /// </summary>
        /// <typeparam name="T">Type of Object return</typeparam>
        /// <param name="criteria">Criteria to Query DB</param>
        /// <returns>Enumerable of Type T</returns>
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
                //HandleException(ex);
                return null;
            }
        }
    }
}
