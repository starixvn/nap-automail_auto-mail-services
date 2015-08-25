namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Business.Licensee
{
    using System.Collections.Generic;
    using Fanex.NAPReport.NegativeBalanceByMatch.Business.Entities;

    public interface ILicenseeService
    {
        IList<string> GetLicensees();

        IList<LicenseeFunction> GetLicenseeFunctions();
    }
}