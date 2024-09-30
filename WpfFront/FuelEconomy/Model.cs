
namespace WpfFront.FuelEconomy
{
    public class Conditions
    {
        public double SpeedIncrement = 10;
        public double FinalSpeed { get; set; } = 90;
        public double FillUpTakesMinutes { get; set; } = 20;
        public double IterationLength { get; set; } = 25;
        public double RabbitRange { get; set; } = 200;
        public double TurtleRange { get; set; } = 500;
        public double MaxMileage { get; set; } = 1200;
        public double Consumption90 { get; set; } = 4.5;
        public double Consumption120 { get; set; } = 5.7;
        private double IterationIncrement
        {
            get
            {
                var givenSpeedsDiff = 120d - 90;
                var incrementIterations = givenSpeedsDiff / SpeedIncrement;
                var consumptionIncrease = Consumption120 / Consumption90;
                var iterationIncrement = Math.Pow(consumptionIncrease, 1 / incrementIterations);

                return iterationIncrement;
            }
        }

        public (List<bool> RabbitFillups, List<bool> TutrleFillups) CalculateFillups()
        {
            var rabbitFillups = GetFillups(RabbitRange, IterationLength, MaxMileage);
            var turtleFillups = GetFillups(TurtleRange, IterationLength, MaxMileage);

            return (rabbitFillups, turtleFillups);
        }

        public List<bool> GetFillups(double maxRange, double step, double mileage)
        {
            var currentRange = maxRange;

            var fillups = new List<bool>();

            for (double i = 0; i < mileage; i += step)
            {
                currentRange -= step;
                if (currentRange <= 0)
                {
                    currentRange = maxRange;
                    fillups.Add(true);
                    continue;
                }

                fillups.Add(false);
            }

            return fillups;
        }

        public List<double> ExtrapolateFuelConsumption(double consumption90, double consumption120)
        {
            List<double> consumptions = new();

            return consumptions;
        }

        public double GetTraveledDistance(int i)
        {
            return (i + 1) * IterationLength;
        }
        public double CalculateNeededSpeed(double traveledDistance, double currentNumberOfFillups)
        {
            var targetTime = traveledDistance / FinalSpeed;

            var wastedTime = currentNumberOfFillups * FillUpTakesMinutes;
            var timeLeft = targetTime - wastedTime / 60;
            var neededSpeed = traveledDistance / timeLeft;

            return neededSpeed;
        }

        public double CalculateConsumption(double speed)
        {
            if (speed == 90)
                return Consumption90;

            if (speed == 120)
                return Consumption120;

            double speedDifference, steps, consumption;
            if (speed < 90)
            {
                speedDifference = 90 - speed;
                steps = speedDifference / SpeedIncrement;
                consumption = Consumption90 / Math.Pow(IterationIncrement, steps);

                return consumption;
            }

            speedDifference = speed - 90;
            steps = speedDifference / SpeedIncrement;
            consumption = Consumption90 * Math.Pow(IterationIncrement, steps);

            return consumption;
        }
    }
}
