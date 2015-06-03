namespace Fanex.NAPReport.NegativeBalanceByMatch.Business.Entities
{
    public class LicenseeFunction
    {
        public string LicenseeName { get; set; }

        public Function Function { get; set; }
    }

    public class Function
    {
        public int IdVipGroup { get; set; }

		public int IdNormalGroup { get; set; }

        public string Name { get; set; }
    }
}
