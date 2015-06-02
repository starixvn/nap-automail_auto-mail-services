namespace Fanex.NAPReport.BalanceMonthlyReport.Business.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Common.Helper;

    /// <summary>
    /// Query Service class.
    /// </summary>
    public class QueryService : ServiceBase
    {
        private readonly IDynamicRepository _repository;

        public QueryService(IDynamicRepository repository = null)
        {
            _repository = repository ?? new DynamicRepository();
        }

        /// <summary>
        /// Get the list of founded entities.
        /// </summary>
        /// <typeparam name="T">General type.</typeparam>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>The list of founded entities.</returns>
        public virtual IEnumerable<T> GetData<T>(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            if (criteria.IsValid())
            {
                return _repository.Fetch<T>(criteria);
            }

            return new List<T>();
        }

        /// <summary>
        /// Get the list of founded entities and total page.
        /// </summary>
        /// <typeparam name="T">General type.</typeparam>
        /// <param name="criteria">Input criteria.</param>
        /// <param name="totalPages">Total page.</param>
        /// <returns>The list of founded entities and total page.</returns>
        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", Justification = "Accept type reference")]
        public IEnumerable<T> GetData<T>(ICriteria criteria, ref int totalPages)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            if (criteria.IsValid())
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var param = criteria.GetSpParams();
                    var list = objectDb.Query<T>(param);
                    totalPages = Convert.ToInt32(param["totalpages"]);
                    return list;
                }
            }

            return new List<T>();
        }

        /// <summary>
        /// Get the list of found entities and output values.
        /// </summary>
        /// <typeparam name="T">General type.</typeparam>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>The list of found entities and output values.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Temporarily, accept it")]
        public IEnumerable<T> GetDataOutput<T>(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                IEnumerable<T> list = new List<T>();

                try
                {
                    var param = criteria.GetSpParams();
                    list = objectDb.Query<T>(param);

                    var outputParams = objectDb.GetOuputParams();
                    if (outputParams != null)
                    {
                        var properties = criteria.GetType().GetProperties();

                        foreach (string outputParam in outputParams.Keys)
                        {
                            foreach (var property in properties)
                            {
                                if (property.CanWrite && property.Name.ToLower() == outputParam)
                                {
                                    property.SetValue(criteria, param[outputParam], null);
                                }
                            }
                        }
                    }
                }
                catch
                {
                }

                return list;
            }
        }

        /// <summary>
        /// Get entity details.
        /// </summary>
        /// <typeparam name="T">General type.</typeparam>
        /// <param name="criteria">Input criteria.</param>
        /// <returns>Entity details.</returns>
        public T GetEntity<T>(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            if (criteria.IsValid())
            {
                using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
                {
                    var param = criteria.GetSpParams();
                    var result = objectDb.QueryEntity<T>(param);
                    return result;
                }
            }

            return (T)new object();
        }

        /// <summary>
        /// Get output values.
        /// </summary>
        /// <param name="criteria">Input criteria.</param>
        public void ExecuteOutput(ICriteria criteria)
        {
            Check.Argument.IsNotNull(criteria, "criteria");

            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();
                objectDb.ExecuteNonQuery(param);

                var outputParams = objectDb.GetOuputParams();
                var properties = criteria.GetType().GetProperties();
                foreach (string outputParam in outputParams.Keys)
                {
                    foreach (var property in properties)
                    {
                        if (property.CanWrite && property.Name.ToLower() == outputParam)
                        {
                            property.SetValue(criteria, param[outputParam] == DBNull.Value ? null : param[outputParam], null);
                        }
                    }
                }
            }
        }
    }
}