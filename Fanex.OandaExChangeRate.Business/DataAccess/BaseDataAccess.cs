namespace Fanex.OandaExChangeRate.Business.DataAccess
{
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Data;

    public class BaseDataAccess
    {
        private Database _db;
        protected enum DatabaseName
        {
            Archive =0, Main = 1
        }
        protected Database Database
        {
            get
            {
                _db = EnterpriseLibraryContainer.Current.GetInstance<Database>(DatabaseName.Archive.ToString());
                return _db;
            }
        }

        protected Database GetDatabase(DatabaseName databaseName)
        {
            if (string.IsNullOrEmpty(databaseName.ToString()))
            {
                return this.Database;
            }

            return EnterpriseLibraryContainer.Current.GetInstance<Database>(databaseName.ToString());
        }
    }
}
