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

        private double _minWRspeed = 70, _maxWRspeed = 150;
        private List<double> _consumptions { get; set; } = new();
        public double FinalSpeed { get; set; } = 90;
        public Conditions Conditions { get; set; } = new();
        public IEnumerable<ISeries> WindResistanceSeries { get; set; }
        public ObservableCollection<ISeries> FillUps { get; set; } = new();

        public ObservableCollection<ISeries> NeededSpeeds { get; set; } = new();

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
        public ViewModel()
        {
            WindResistanceSeries = FillWrChart(_minWRspeed, _maxWRspeed);
        }

        public void DrawFillupsAndSpeeds()
        {
            var fillups = CalculateFillups();
            FillUps.Clear();
            foreach (var fillup in fillups)
            {
                FillUps.Add(fillup);
            }

            var speeds = CalculateSpeeds();
            NeededSpeeds.Clear();
            foreach(var speed in speeds)
            {
                NeededSpeeds.Add(speed);
            }
        }

        IEnumerable<ISeries> FillWrChart(double minWRspeed, double maxWRspeed)
        {
            List<WeightedPoint> coeffs = new();
            List<WeightedPoint> consumptions = new();

            for (double i = minWRspeed; i <= maxWRspeed;)
            {
                //упрощение, лишь бы зависимость была квадратичная
                var windDrag = Math.Pow(i, 2) / 1000;
                coeffs.Add(new WeightedPoint
                {
                    X = i,
                    Y = windDrag
                });

                //произвольная формула
                var consumptionValue = 3.5 + windDrag / 6;

                consumptions.Add(new WeightedPoint
                {
                    X = i,
                    Y = consumptionValue
                });

                i += 10;
            }

            var resultSeries = new ISeries[]
            {
                new LineSeries<WeightedPoint>
                {
                    Values = coeffs,
                    Name = "Сопротивление ветра"
                },
                new LineSeries<WeightedPoint>
                {
                    Values = consumptions,
                    Name = "Расход топлива"
                },
            };

            return resultSeries;
        }

        ObservableCollection<ISeries> CalculateFillups()
        {
            var fillups = Conditions.CalculateFillups();

            List<WeightedPoint> rabbitFillupPoints = new();
            rabbitFillupPoints.Add(new WeightedPoint { X = 0, Y = 0});
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

        IEnumerable<ISeries> CalculateSpeeds()
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

        List<WeightedPoint> CalculateSpeeds(List<bool> fillUps)
        {
            List<WeightedPoint> points = new();
            int currentNumberOfFillups = 0;
            for(int i = 0; i < fillUps.Count(); i++)
            {
                var fillup = fillUps[i];

                if (fillup)
                    currentNumberOfFillups++;

                var traveledDistance = (i + 1) * Conditions.IterationLength;
                var targetTime = traveledDistance / FinalSpeed;

                var wastedTime = currentNumberOfFillups * Conditions.FillUpTakesMinutes;
                var timeLeft = targetTime - wastedTime / 60;
                var neededSpeed = traveledDistance / timeLeft;

                var point = new WeightedPoint
                {
                    X = traveledDistance,
                    Y = neededSpeed
                };

                points.Add(point);
            }

            return points;
        }
    }
}
