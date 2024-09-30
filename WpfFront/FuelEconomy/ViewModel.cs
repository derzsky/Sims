using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using LiveChartsCore.ConditionalDraw;
using LiveChartsCore.Defaults;

namespace WpfFront.FuelEconomy
{
    public class ViewModel
    {
        public Conditions Conditions { get; set; } = new();
        public ObservableCollection<ISeries> FillUps { get; set; } = new();
        public ObservableCollection<ISeries> NeededSpeeds { get; set; } = new();
        public ObservableCollection<ISeries> ConsumptionsSeries { get; set; } = new();

        public Axis[] Limit3Axes =
        {
            new Axis
            {
                MinLimit = 3
            }
        };
        public Axis[] Min0Max2Axes =
        {
            new Axis
            {
                MinLimit = 0,
                MaxLimit = 2
            }
        };
        public Axis[] SpeedAxes =
        {
            new Axis
            {
                MinLimit = 70
            }
        };

        public void DrawFillupsAndSpeeds()
        {

            var fillups = CalculateFillups();
            FillUps.Clear();
            foreach (var fillup in fillups)
            {
                FillUps.Add(fillup);
            }

            var speeds = CalculateSpeedsSeries();
            NeededSpeeds.Clear();
            foreach (var speed in speeds)
            {
                NeededSpeeds.Add(speed);
            }

            var consumptions = CalculateConsumptionSeries();
            ConsumptionsSeries.Clear();
            foreach (var consumption in consumptions)
            {
                ConsumptionsSeries.Add(consumption);
            }
        }

        ObservableCollection<ISeries> CalculateFillups()
        {
            var fillups = Conditions.CalculateFillups();

            List<WeightedPoint> rabbitFillupPoints = new();
            rabbitFillupPoints.Add(new WeightedPoint { X = 0, Y = 0 });
            List<WeightedPoint> turtleFillupPoints = new();
            turtleFillupPoints.Add(new WeightedPoint { X = 0, Y = 0 });

            int i = 0;
            foreach (var rabbitFillup in fillups.RabbitFillups)
            {
                var rabbitFillupPoint = new WeightedPoint
                {
                    X = (i + 1) * Conditions.IterationLength,
                };

                if (rabbitFillup)
                {
                    rabbitFillupPoint.Y = 1;
                    rabbitFillupPoints.Add(rabbitFillupPoint);
                }

                i++;
            }

            i = 0;
            foreach (var turtleFillup in fillups.TutrleFillups)
            {
                var turtleFillupPoint = new WeightedPoint
                {
                    X = (i + 1) * Conditions.IterationLength,
                };

                if (turtleFillup)
                {
                    turtleFillupPoint.Y = 1;
                    turtleFillupPoints.Add(turtleFillupPoint);
                }

                i++;
            }

            var series = new ObservableCollection<ISeries>
            {
                new StackedColumnSeries<WeightedPoint>
                {
                    Values = rabbitFillupPoints,
                    Name = "Заправки Зайца",
                    MaxBarWidth = double.MaxValue,
                    IgnoresBarPosition = true
                },
                new StackedColumnSeries<WeightedPoint>
                {
                    Values = turtleFillupPoints,
                    Name = "Заправки Черепахи",
                    MaxBarWidth = double.MaxValue,
                    IgnoresBarPosition = true
                },
            };

            return series;
        }

        IEnumerable<ISeries> CalculateSpeedsSeries()
        {
            var fillups = Conditions.CalculateFillups();

            List<WeightedPoint> rabbitSpeedPoints = CalculateSpeeds(fillups.RabbitFillups);

            List<WeightedPoint> turtleSpeedPoints = CalculateSpeeds(fillups.TutrleFillups);

            var series = new ObservableCollection<ISeries>
            {
                new LineSeries<WeightedPoint>
                {
                    Values = rabbitSpeedPoints,
                    Name = "Крейсер Зайца",
                    Fill = null
                },
                new LineSeries<WeightedPoint>
                {
                    Values = turtleSpeedPoints,
                    Name = "Крейсер Черепахи",
                    Fill = null
                }
            };

            return series;
        }

        public List<WeightedPoint> CalculateSpeeds(List<bool> fillUps)
        {
            List<WeightedPoint> points = new();
            int currentNumberOfFillups = 0;
            for (int i = 0; i < fillUps.Count(); i++)
            {
                var fillup = fillUps[i];

                if (fillup)
                    currentNumberOfFillups++;

                var traveledDistance = Conditions.GetTraveledDistance(i);
                var neededSpeed = Conditions.CalculateNeededSpeed(traveledDistance, currentNumberOfFillups);

                var point = new WeightedPoint
                {
                    X = traveledDistance,
                    Y = neededSpeed
                };

                points.Add(point);
            }

            return points;
        }

        IEnumerable<ISeries> CalculateConsumptionSeries()
        {
            var fillups = Conditions.CalculateFillups();

            List<WeightedPoint> rabbitSpeedPoints = CalculateSpeeds(fillups.RabbitFillups);

            List<WeightedPoint> turtleSpeedPoints = CalculateSpeeds(fillups.TutrleFillups);

            var rabbitConsumptions = CalculateConsumptions(rabbitSpeedPoints);
            var turtleConsumptions = CalculateConsumptions(turtleSpeedPoints);

            var series = new ObservableCollection<ISeries>
            {
                new LineSeries<WeightedPoint>
                {
                    Values = rabbitConsumptions,
                    Name = "Расход Зайца",
                    Fill = null
                },
                new LineSeries<WeightedPoint>
                {
                    Values = turtleConsumptions,
                    Name = "Расход Черепахи",
                    Fill = null
                }
            };

            return series;
        }

        public List<WeightedPoint> CalculateConsumptions(List<WeightedPoint> speedPoints)
        {
            List<WeightedPoint> consumptions = new();
            
            foreach(var speedPoint in speedPoints)
            {
                var consumptionValue = Conditions.CalculateConsumption(speedPoint.Y.Value);

                var point = new WeightedPoint
                {
                    X = speedPoint.X,
                    Y = consumptionValue
                };

                consumptions.Add(point);
            }

            return consumptions;
        }
    }
}
