using Accessibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WpfFront.HealthCare
{    public class Market
    {
        public double RiskToGetSickness { get; set; }
        public double SicknessProgressionProgability { get; set; }
        public double SicknessProgressionStep { get; set; }
        public double TolereableSicknessThreshold { get; set; }
        public double SicknessIsAcceptedByMarketThreshold { get; set; }
        public double SicknessIsHealedThreshold { get; set; }
        public double UrgencyThreshold { get; set; }
        public bool SelfPaidCare { get; set; }
        public double PeopleWantToCkeckUp { get; set; }
        public double DoctorSkillMultiplyer { get; set; }
    }

    public class Illness
    {
        private double _wantsToDealItThemselves;
        private double _wantsToCheckup1;
        private double _wantsToCheckup2;

        public double Strength { get; set; }

        public Illness()
        {
            _wantsToDealItThemselves = Random.Shared.NextDouble();
            _wantsToCheckup1 = Random.Shared.NextDouble();
            _wantsToCheckup2 = Random.Shared.NextDouble();
        }

        public Solution GetSolution(Market market)
        {
            if (IsCheckup(market) || IsUrgent(market))
                return Solution.Doctor;

            if (Strength < market.SicknessIsHealedThreshold)
                if (market.SelfPaidCare || _wantsToDealItThemselves > .5)
                    return Solution.Ignore;

            if (market.SelfPaidCare && _wantsToDealItThemselves > market.TolereableSicknessThreshold)
                return Solution.Drugstore;

            return Solution.Doctor;
        }

        public bool IsCheckup(Market market)
        {
            var notIll = Strength == 0;
            var wantsToCheckup = WantsToCheckup(market);
            return notIll && wantsToCheckup;
        }
        public bool IsUrgent(Market market)
        {
            return Strength >= market.UrgencyThreshold;
        }
        private bool WantsToCheckup(Market market)
        {
            double finalDecision;
            //за свои не так охотно
            if (market.SelfPaidCare)
            {
                finalDecision = Math.Min(_wantsToCheckup1, _wantsToCheckup2);
            }
            else
            {
                finalDecision = Math.Max(_wantsToCheckup1, _wantsToCheckup2);
            }

            return finalDecision > .5;
        }

        public enum Solution
        {
            Doctor,
            Drugstore,
            Ignore
        }
    }
}
