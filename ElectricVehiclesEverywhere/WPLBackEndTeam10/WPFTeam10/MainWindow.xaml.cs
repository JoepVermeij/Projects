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
using WPFTeam10.Individeule_Opdracten;
using WPFTeam10.Individuele_Opdracten;
using WPFTeam10.Project;

namespace WPFTeam10
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MnuSettings_Click(object sender, RoutedEventArgs e)
        {
            var wpf = new WpfSettings();
            wpf.ShowDialog();
        }

        private void MnuEvaertKristof_Click(object sender, RoutedEventArgs e)
        {
            var wpf = new WpfKristofvaert();
            wpf.ShowDialog();
        }

        private void MnuJoepVermeij_Click(object sender, RoutedEventArgs e)
        {
            var wpf = new WpfJoepVermeij();
            wpf.ShowDialog();
        }

        private void MnuDavyKerkhofs_Click(object sender, RoutedEventArgs e)
        {
            var wpf = new WpfDavyKerkhofs();
            wpf.ShowDialog();
        }

        private void MnuBenSleurs_Click(object sender, RoutedEventArgs e)
        {
            var wpf = new WpfBenSleurs();
            wpf.ShowDialog();
        }

        private void MnuBenjaminVanhees_Click(object sender, RoutedEventArgs e)
        {
            var wpf = new WpfBenjaminVanhees();
            wpf.ShowDialog();
        }
    }
}
