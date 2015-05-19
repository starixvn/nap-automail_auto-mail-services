namespace Fanex.HorseTransaction3rdServices.Configurations
{
    using System.Configuration;

    public class RacingSection : ConfigurationSection
    {
        public static RacingSection Instance
        {
            get
            {
                return ConfigurationManager.GetSection("RacingSection") as RacingSection;
            }
        }

        [ConfigurationProperty("RacingConfigs", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(RacingCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public RacingCollection RacingConfigs
        {
            get
            {
                return (RacingCollection)base["RacingConfigs"];
            }
        }
    }
}
