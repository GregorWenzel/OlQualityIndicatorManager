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
using Telerik.Windows.Controls;

namespace OlQualityIndicatorManager.Grid.Views
{
    /// <summary>
    /// Interaction logic for GuidelineSelectionView.xaml
    /// </summary>
    public partial class GuidelineSelectionView : UserControl
    {
        public GuidelineSelectionView()
        {
            InitializeComponent();
        }

        private void RadGridView_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as RadGridView).SelectAll();
        }
    }
}
