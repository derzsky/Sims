using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfFront.FuelEconomy
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.DrawFillupsAndSpeeds();
        }
    }
}
