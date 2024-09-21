using System.Data;

namespace Front.Healthcare
{
    public class NumberGenerator
    {
        //при тестировании сюда можно поместить заранее известные значения вместо случайных
        private Queue<double> _doubles = new();

        public double RandomDouble
        {
            get
            {
                if (_doubles.Any())
                    return _doubles.Dequeue();

                throw new NotImplementedException();
                return Random.Shared.NextDouble();
            }
        }

        public NumberGenerator(Queue<double> doubles = null)
        {
            if (doubles == null)
                return;

            _doubles = doubles;
        }
    }

    public class Market
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

        public NumberGenerator Generator { get; set; }
        public List<Sickness> Line { get; set; } = new();
        public List<Sickness> TotalSicknesses { get; set; } = new();
    }

    public class Doctor
    {
        private Market _market;

        public Doctor(Market market)
        {
            _market = market;
        }

        public void Update()
        {
            var orderedLine = _market.Line.OrderBy(s => s.IsUrgent).ToList();

            foreach (var sickness in orderedLine)
            {
                sickness.Heal(_market.DoctorSkillMultiplyer * _market.SicknessProgressionStep);
            }
        }
    }

    public class Person
    {
        private Market _market;
        public List<Sickness> Sicknesses { get; set; } = new();

        public Person(Market market)
        {
            _market = market;
        }

        public void Update()
        {
            GetNewSickness();

            for (int i = Sicknesses.Count - 1; i >= 0; i--)
            {
                var sickness = Sicknesses[i];
                sickness.Update();

                HandleSickness(sickness);
            }

            if (Sicknesses.Any())
                return;

            CreateCheckupSickness();
        }

        public void GetNewSickness()
        {
            var randomNumber = _market.Generator.RandomDouble;
            if (randomNumber < _market.RiskToGetSickness)
                return;

            var sicknessStrength = _market.Generator.RandomDouble;
            var sicknessAcceptanceByMarket = _market.Generator.RandomDouble;

            if (!Sicknesses.Any(s => s.IsCheckup))
            {
                var sickness = new Sickness(sicknessStrength, _market, sicknessAcceptanceByMarket);
                Sicknesses.Add(sickness);
                return;
            }

            var currentChecupSickness = Sicknesses.First(s => s.IsCheckup);
            currentChecupSickness.IsCheckup = false;
            currentChecupSickness.Strength = sicknessStrength;
            currentChecupSickness.AcceptanceByMarket = sicknessAcceptanceByMarket;
        }

        public void CreateCheckupSickness()
        {
            Sickness checkupSickness;
            //если оплачено, то записывается
            if (!_market.SelfPaidCare)
            {
                checkupSickness = new Sickness(_market, true);
                Sicknesses.Add(checkupSickness);
                return;
            }

            //а если за свои, то сначал думает
            var wantsToCheckup = _market.Generator.RandomDouble;
            if (wantsToCheckup < _market.PeopleWantToCkeckUp)
                return;

            checkupSickness = new Sickness(_market, true);
            Sicknesses.Add(checkupSickness);
            _market.Line.Add(checkupSickness);
        }

        public void HandleSickness(Sickness sickness)
        {
            //если чекап, то это не лечится
            if (!sickness.IsCheckup
                    && sickness.Strength < _market.SicknessIsHealedThreshold
                    && _market.SelfPaidCare)
            {
                HealSicknessCompletely(sickness);
                return;
            }

            if (sickness.IsUrgent)
            {
                if (!_market.Line.Contains(sickness))
                    _market.Line.Add(sickness);

                return;
            }

            if (!_market.SelfPaidCare)
            {
                if (!_market.Line.Contains(sickness))
                    _market.Line.Add(sickness);

                return;
            }

            //если захотел
            var wantsToDealItThemselves = _market.Generator.RandomDouble;
            if (wantsToDealItThemselves > _market.TolereableSicknessThreshold)
            {
                //купил таблетку
                sickness.Heal(_market.SicknessProgressionStep);
                return;
            }

            if (!_market.Line.Contains(sickness))
                _market.Line.Add(sickness);
        }

        public void HealSicknessCompletely(Sickness sickness)
        {
            if (_market.Line.Contains(sickness))
                _market.Line.Remove(sickness);

            Sicknesses.Remove(sickness);
        }
    }

    public class Sickness
    {
        private Market _market;
        public double Strength { get; set; }
        public double AcceptanceByMarket { get; set; }
        public bool IsUrgent { get { return Strength > _market.UrgencyThreshold; } }

        public bool IsCheckup;

        public Sickness(double strength, Market market, double acceptanceByMarket)
        {
            _market = market;
            AcceptanceByMarket = acceptanceByMarket;
            Strength = strength;
            _market.TotalSicknesses.Add(this);
        }

        public Sickness(Market market, bool checkup)
        {
            _market = market;
            IsCheckup = checkup;
            Strength = 0;
            AcceptanceByMarket = 1;
            _market.TotalSicknesses.Add(this);
        }

        public void Heal(double amount)
        {
            if (Strength > amount)
                Strength -= amount;
            else
                Strength = 0;
        }

        public void Update()
        {
            if (IsUrgent)
                return;

            var progressionProbability = _market.Generator.RandomDouble;
            if (progressionProbability >= _market.SicknessProgressionProgability)
                Strength += _market.SicknessProgressionStep;
            else if (Strength >= _market.SicknessProgressionStep)
                Strength -= _market.SicknessProgressionStep;
        }
    }
}
