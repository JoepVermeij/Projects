using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using ClassLibTeam10.Data.Framework;
using ClassLibTeam10.Data.Studenten;
using ClassLibTeam10.Business;
using ClassLibTeam10.Entities;
using ClassLibTeam10.IndividueleOpdrachten.KerkhofsDavy;
using ClassLibTeam10.Settings;

namespace WPFTeam10.Individeule_Opdracten
{
    /// <summary>
    /// Interaction logic for WpfDavyKerkhofs.xaml
    /// </summary>
    public partial class WpfDavyKerkhofs : Window, IDisposable
    {
        private SqlConnection sqlConn;
        public WpfDavyKerkhofs()
        {
            InitializeComponent();
            sqlConn = new SqlConnection(Settings.Database.ProjectConnectionString);
            sqlConn.Open();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DavyKerkhofs Davy = new DavyKerkhofs();
            TB_Voornaam.Text = Davy.GetFirstName();
            TB_Naam.Text = Davy.GetLastName();
            foreach (Hobby item in Davy.Hobbies)
            {
                LB_Hobbies.Items.Add(item);
            }
        }
        private void B_SelectTestSql_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                SqlConnection conn = new SqlConnection(Settings.Database.IndividualConnectionString);
                string query = "select * from students";
                SqlCommand sql = new SqlCommand(query, conn);
                conn.Open();
                //sql.CommandText = query;
                //sql.Connection = conn;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = sql;

                DataSet ds = new DataSet();
                adapter.Fill(ds);
                conn.Close();
                DataTable dt = ds.Tables[0];
                DataView dv = dt.DefaultView;
                DG_Students.ItemsSource = ds.Tables[0].DefaultView;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void Insert()
        {
            try
            {
                SqlConnection conn = new SqlConnection(Settings.Database.IndividualConnectionString);
                string query = $"Insert into students (firstname, lastname) Values('{TB_Voornaam.Text}','{TB_Naam.Text}')";
                SqlCommand sql = new SqlCommand(query, conn);
                conn.Open();
                sql.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void B_Insert_Click(object sender, RoutedEventArgs e)
        {
            Insert();
        }

        private void B_SelectFramework_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DG_Students.ItemsSource = sqlConn.GetStudenten().DataTable.DefaultView;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void B_InsertFramework_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Student student = new Student();
                student.FirstName = TB_Voornaam.Text;
                student.LastName = TB_Naam.Text;
                sqlConn.AddObjectToDataBase(student);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public void Dispose()
        {
            sqlConn.Close();
        }
    }
}
