using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfFront.FuelEconomy
{
    public class Conditions
    {
        public double FillUpTakesMinutes { get; set; } = 20;
        public double IterationLength { get; set; } = 25;
        public double RabbitRange { get; set; } = 200;
        public double TurtleRange { get; set; } = 500;
        public double MaxMileage { get; set; } = 1200;

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
    }
    public class Contender
    {

    }
}
