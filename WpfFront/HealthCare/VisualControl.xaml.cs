using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using LiveChartsCore.ConditionalDraw;

namespace WpfFront.HealthCare
{
    /// <summary>
    /// Interaction logic for VisualControl.xaml
    /// </summary>
    public partial class VisualControl : UserControl
    {
        ViewModel _viewModel;
        public VisualControl()
        {
            InitializeComponent();
            
            _viewModel = DataContext as ViewModel;
        }
    }

    public class ViewModel
    {
        public Market Market1 = new Market
        {
            DoctorSkillMultiplyer = 2,
            PeopleWantToCkeckUp = .5,
            RiskToGetSickness = .0015,
            SelfPaidCare = true,
            SicknessIsAcceptedByMarketThreshold = 0,
            SicknessIsHealedThreshold = .1,
            SicknessProgressionProgability = .5,
            SicknessProgressionStep = .2,
            TolereableSicknessThreshold = .1,
            UrgencyThreshold = .7
        };

        public Market Market2 = new Market
        {
            DoctorSkillMultiplyer = 2,
            PeopleWantToCkeckUp = .5,
            RiskToGetSickness = .0015,
            SicknessIsAcceptedByMarketThreshold = 0.2,
            SicknessIsHealedThreshold = .1,
            SicknessProgressionProgability = .5,
            SicknessProgressionStep = .2,
            TolereableSicknessThreshold = .1,
            UrgencyThreshold = .7
        };
        public List<Illness> Illnesses { get; set; } = new List<Illness>();
        public ObservableCollection<ISeries> GeneratedIllnessesSeries { get; set; }

        public ObservableCollection<int> DoctroSolutionsCounts = new();
        public ObservableCollection<int> DrugstoreSolutionsCounts = new();
        public ObservableCollection<int> IgnoreSolutionsCounts = new();

        public ObservableCollection<int> IllCounts = new();
        public ObservableCollection<int> CheckupCounts = new();
        public ObservableCollection<ISeries> SolutionsCountSeries { get; set; }

        public ObservableCollection<int> WillBeTreatedCount = new();
        public ObservableCollection<int> WillNotBeTreatedCount = new();

        public ObservableCollection<int> WillBeHealedByDoctors = new();

        public Axis[] YAxes01 { get; set; } =
        {
            new Axis
            {
                MinLimit = 0,
                MaxLimit = 1
            }
        };

        public Axis[] YAxes0100 { get; set; } =
        {
            new Axis
            {
                MinLimit = 0,
                MaxLimit = 110
            }
        };

        public Axis[] EmptyAxis { get; set; } =
        {
            new Axis
            {
                MinLimit = -.5,
                MaxLimit = 1.5,
                MinStep = 1
            }
        };

        public RectangularSection[] Sections { get; set; } =
    {
        new RectangularSection
        {
            Label = "Несрочные пациенты, рынок",
            LabelSize = 25,
            LabelPaint = new SolidColorPaint(SKColors.MediumSeaGreen)
            {
                SKTypeface = SKTypeface.FromFamilyName("Roboto", SKFontStyle.Bold)
            },
            Xi = -.5,
            Xj = 100,
            Yi = .7,
            Yj = .0,
            Fill = new SolidColorPaint(SKColors.MediumSeaGreen.WithAlpha(50))
        },
        new RectangularSection
        {
            Label = "Несрочные пациенты, страховка",
            LabelSize = 25,
            LabelPaint = new SolidColorPaint(SKColors.MediumSeaGreen)
            {
                SKTypeface = SKTypeface.FromFamilyName("Roboto", SKFontStyle.Bold)
            },
            Xi = 100,
            Xj = 200.5,
            Yi = .7,
            Yj = .2,
            Fill = new SolidColorPaint(SKColors.MediumSeaGreen.WithAlpha(50))
        },
        new RectangularSection
        {
            Label = "Срочные пациенты",
            LabelSize = 25,
            LabelPaint = new SolidColorPaint(SKColors.OrangeRed)
            {
                SKTypeface = SKTypeface.FromFamilyName("Roboto", SKFontStyle.Bold)
            },
            Yj = .7,
            Fill = new SolidColorPaint(SKColors.Red.WithAlpha(50))
        },
    };

        public int PeopleCount = 100;
        public ViewModel()
        {
            for (int i = 0; i < PeopleCount; i++)
            {
                var isSick = Random.Shared.NextDouble();
                var wantsToCkeckup = Random.Shared.NextDouble();
                double strength = 0;
                if (isSick > .5)
                    strength = Random.Shared.NextDouble();

                var illness = new Illness
                {
                    Strength = strength
                };
                Illnesses.Add(illness);
            }

            //два раза, чтобы показать на врехнем графике
            var illnessesToRender = Illnesses.ToList();
            illnessesToRender.AddRange(Illnesses);

            GeneratedIllnessesSeries = new ObservableCollection<ISeries>
            {
                new ColumnSeries<double>
                {
                    Values = illnessesToRender.Select(i => i.Strength).ToArray()
                }
                .OnPointMeasured(p =>
                {
                    if(p.Visual is null)
                        return;

                    if(p.PrimaryValue >= Market1.UrgencyThreshold)
                        p.Visual.Fill = new SolidColorPaint(SKColors.OrangeRed);

                    if(p.PrimaryValue == 0)
                        p.Visual.Fill = new SolidColorPaint(SKColors.MediumSeaGreen);
                })
            };

            DoctroSolutionsCounts.Add(Illnesses.Count(i => i.GetSolution(Market1) == Illness.Solution.Doctor));
            DoctroSolutionsCounts.Add(Illnesses.Count(i => i.GetSolution(Market2) == Illness.Solution.Doctor));

            DrugstoreSolutionsCounts.Add(Illnesses.Count(i => i.GetSolution(Market1) == Illness.Solution.Drugstore));
            DrugstoreSolutionsCounts.Add(Illnesses.Count(i => i.GetSolution(Market2) == Illness.Solution.Drugstore));

            IgnoreSolutionsCounts.Add(Illnesses.Count(i => i.GetSolution(Market1) == Illness.Solution.Ignore));
            IgnoreSolutionsCounts.Add(Illnesses.Count(i => i.GetSolution(Market2) == Illness.Solution.Ignore));

            IllCounts.Add(Illnesses.Count(i => i.GetSolution(Market1) == Illness.Solution.Doctor && !i.IsCheckup(Market1)));
            IllCounts.Add(Illnesses.Count(i => i.GetSolution(Market2) == Illness.Solution.Doctor && !i.IsCheckup(Market2)));

            CheckupCounts.Add(Illnesses.Count(i => i.GetSolution(Market1) == Illness.Solution.Doctor && i.IsCheckup(Market1)));
            CheckupCounts.Add(Illnesses.Count(i => i.GetSolution(Market2) == Illness.Solution.Doctor && i.IsCheckup(Market2)));

            SolutionsCountSeries = new ObservableCollection<ISeries>
            {
                new StackedColumnSeries<int>
                {
                    Values = DoctroSolutionsCounts,
                    Name = "Записаны к врачу",
                    StackGroup = 0
                },
                new StackedColumnSeries<int>
                {
                    Values = DrugstoreSolutionsCounts,
                    Name = "Не ждут, идут в аптеку",
                    StackGroup = 0
                },
                new StackedColumnSeries<int>
                {
                    Values = IgnoreSolutionsCounts,
                    Name = "Решили, что не болит",
                    StackGroup = 0
                },
                new StackedColumnSeries<int>
                {
                    Values = IllCounts,
                    Name = "Пациенты",
                    StackGroup = 1
                },
                new StackedColumnSeries<int>
                {
                    Values = CheckupCounts,
                    Name = "Провериться",
                    StackGroup = 1
                },

            };

            var lineToDoctors1 = Illnesses.Where(i => i.GetSolution(Market1) == Illness.Solution.Doctor);
            var lineToDoctors2 = Illnesses.Where(i => i.GetSolution(Market2) == Illness.Solution.Doctor);

            WillBeTreatedCount.Add(
                lineToDoctors1.Count(i =>
                    i.IsCheckup(Market1)
                    || i.Strength >= Market1.SicknessIsAcceptedByMarketThreshold));
            WillBeTreatedCount.Add(
                lineToDoctors2.Count(i =>
                    i.IsCheckup(Market2)
                    || i.Strength >= Market2.SicknessIsAcceptedByMarketThreshold));

            WillNotBeTreatedCount.Add(
                lineToDoctors1.Count(i =>
                    !i.IsCheckup(Market1)
                    && i.Strength < Market1.SicknessIsAcceptedByMarketThreshold));
            WillNotBeTreatedCount.Add(
                lineToDoctors2.Count(i =>
                    !i.IsCheckup(Market2)
                    && i.Strength < Market2.SicknessIsAcceptedByMarketThreshold));

            SolutionsCountSeries.Add(
                new StackedColumnSeries<int>
                {
                    Values = WillBeTreatedCount,
                    Name = "Будут обслужены врачами",
                    StackGroup = 2
                });
            SolutionsCountSeries.Add(
                new StackedColumnSeries<int>
                {
                    Values = WillNotBeTreatedCount,
                    Name = "Не будут обслужены врачами",
                    StackGroup = 2
                });

            WillBeHealedByDoctors.Add(Illnesses.Count(i =>
                    !i.IsCheckup(Market1)
                    && i.GetSolution(Market1) == Illness.Solution.Doctor
                    && i.Strength >= Market1.SicknessIsAcceptedByMarketThreshold
                    || i.GetSolution(Market1) == Illness.Solution.Drugstore));

            WillBeHealedByDoctors.Add(Illnesses.Count(i =>
                !i.IsCheckup(Market2)
                && i.GetSolution(Market2) == Illness.Solution.Doctor
                && i.Strength >= Market2.SicknessIsAcceptedByMarketThreshold
                || i.GetSolution(Market2) == Illness.Solution.Drugstore));

            SolutionsCountSeries.Add(
                new StackedColumnSeries<int>
                {
                    Values = WillBeHealedByDoctors,
                    Name = "Будут вылечены",
                    StackGroup = 3
                });
        }
    }
}
