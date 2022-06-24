using ClassLibTeam10.Data.Framework;
using ClassLibTeam10.Entities;
using ClassLibTeam10.Settings;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
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

namespace WPFTeam10
{
    /// <summary>
    /// Interaction logic for LogInScreen.xaml
    /// </summary>
    public partial class LogInScreen : Window, IDisposable
    {
        private SqlConnection sqlConn;
        public LogInScreen()
        {
            InitializeComponent();
            EmailTextBox.Focus();
            sqlConn = new SqlConnection(Settings.Database.PxlConnectionString);
            sqlConn.Open();
        }
        public void LogInFunctie()
        {
Login login = new Login(EmailTextBox.Text, WachtwoordTextBox.Password);
            if (HandleLogins.TryLogin(sqlConn, login))
            {
                if (HandleAdmin.IsAdmin(sqlConn, EmailTextBox.Text))
                {
                    StartUpWindow startUpWindow = new StartUpWindow();
                    startUpWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("U bent geen admin");
                }


            }
            else
            {
                MessageBox.Show("combinatie is fout");
                WachtwoordTextBox.Password = string.Empty;
            }
        }

        private void LogInButtonClick(object sender, RoutedEventArgs e)
        {
            LogInFunctie();
        }



        public void Dispose()
        {
            sqlConn.Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LogInFunctie();
            }
        }
    }
}
