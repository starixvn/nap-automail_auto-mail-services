namespace Fanex.NAPReport.SMALoss.Business.Entities
{
    /// <summary>
    /// Loss level model.
    /// </summary>
    public class SMALossLevel
    {
        public byte LevelID { get; set; }

        public byte ActionID { get; set; }

        public int Value { get; set; }
    }
}