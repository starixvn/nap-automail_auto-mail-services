namespace Fanex.NAPReport.SMALoss.Business.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using Fanex.NAP.Framework.Data;
    using Fanex.Service.Core.Extensions;

    public class ServiceBase
    {
        private readonly IDynamicRepository _dynamicRepository;

        public ServiceBase()
        {
            _dynamicRepository = new DynamicRepository();
        }

        /// <summary>
        /// Get list data with Criteria.
        /// </summary>
        /// <typeparam name="T">The input object type.</typeparam>
        /// <param name="criteria">Criteria to Query DB.</param>
        /// <returns>Enumerable of Type T.</returns>
        public IEnumerable<T> GetEntites<T>(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    return _dynamicRepository.Fetch<T>(criteria);
                }
            }
            catch (Exception exception)
            {
                LogExtensions.ExceptionHandler(exception);
                throw;
            }

            return new List<T>();
        }

        /// <summary>
        /// Gets data.
        /// </summary>
        /// <typeparam name="T">The input object type.</typeparam>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Returns a object T.</returns>
        public T GetEntity<T>(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            try
            {
                if (criteria.IsValid())
                {
                    return _dynamicRepository.Get<T>(criteria);
                }
            }
            catch (Exception exception)
            {
                LogExtensions.ExceptionHandler(exception);
                throw;
            }

            return (T)new object();
        }
    }
}