namespace Fanex.NAPReport.WinLossByProducts.Business.Criteria
{
    using Fanex.Data.Repository;

    public class GetDWStatusByCubeCriteria : CriteriaBase
    {
        public string Cube
        {
            get;
            set;
        }

        public override string GetSettingKey()
        {
            return "BI_GetDWStatusByCube";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
