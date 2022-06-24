using ClassLibTeam10.Settings;
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
using System.Windows.Shapes;

namespace WPFTeam10.Project
{
    /// <summary>
    /// Interaction logic for WpfSettings.xaml
    /// </summary>
    public partial class WpfSettings : Window
    {
        public WpfSettings()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TxtSqlServer.Text = Settings.Database.Server;
            TxtProjDB.Text = Settings.Database.ProjectDb;
            TxtindDB.Text = Settings.Database.IndividualConnectionString;
            TxtProjConnString.Text = Settings.Database.ProjectConnectionString;
            TxtIndConnString.Text = Settings.Database.IndividualConnectionString;
        }
    }
}
