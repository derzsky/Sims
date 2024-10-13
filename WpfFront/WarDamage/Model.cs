namespace WpfFront.WarDamage
{
    public class Government
    {
        public double GunPower { get; set; }
        public double EconomicPower { get; set; }
        public double ManPower { get; set; }

        public double MaxGunPowerSupply { get; set; }
        public double MaxManPowerSupply { get; set; }

        public double GunPowerThreshold { get; set; }
        public double EconomicPowerThreshold { get; set; }
        public double ManPowerThreshold { get; set; }

        public double ManLoss { get; set; }

        public bool WantsToWar
        {
            get
            {
                return GunPower >= GunPowerThreshold
                    && EconomicPower >= EconomicPowerThreshold
                    && ManPower >= ManPowerThreshold;
            }
        }
        

    }
}
