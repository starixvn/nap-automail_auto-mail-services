namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Helper
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using Fanex.NAPReport.NegativeBalanceByMatch.Business.Entities;

    public class LicenseeFunctionParser
    {
        private readonly string _fileNamePath;

        public LicenseeFunctionParser(string fileNamePath)
        {
            _fileNamePath = fileNamePath;
            LicenseeFunctions = new List<LicenseeFunction>();
            Parse();
        }

        public IList<LicenseeFunction> LicenseeFunctions { get; set; }

        private void Parse()
        {
            XElement licenseeFunctions = XElement.Load(_fileNamePath);

            foreach (XElement licenseeFunction in licenseeFunctions.Elements())
            {
                LicenseeFunctions.Add(
                    new LicenseeFunction()
                    {
                        LicenseeName = licenseeFunction.Attribute("licenseeName").Value,
                        Function = new Function
                        {
                            IdVipGroup = int.Parse(licenseeFunction.Attribute("functionIdVipGroup").Value),
                            IdNormalGroup = int.Parse(licenseeFunction.Attribute("functionIdNormalGroup").Value),
                            Name = licenseeFunction.Attribute("functionName").Value
                        }
                    });
            }
        }
    }
}