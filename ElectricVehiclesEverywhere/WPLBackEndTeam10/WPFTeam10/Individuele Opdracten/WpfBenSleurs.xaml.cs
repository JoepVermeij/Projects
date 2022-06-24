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
using System.Data.SqlClient;
using ClassLibTeam10.Settings;
using ClassLibTeam10.IndividueleOpdrachten.SleursBen;
using System.Data;
using ClassLibTeam10.Entities;
using ClassLibTeam10.Data;
using ClassLibTeam10.Data.Framework;
using ClassLibTeam10.Data.Studenten;
using System.Reflection;
using ClassLibTeam10.Mail;
using ClassLibTeam10.Business;

namespace WPFTeam10.Individeule_Opdracten
{
    /// <summary>
    /// Interaction logic for WpfBenSleurs.xaml
    /// </summary>
    public partial class WpfBenSleurs : Window, IDisposable
    {
        private SqlConnection sqlConn;
        System.Windows.Controls.DataGrid dataGrid;
        DataRowView row;
        public WpfBenSleurs()
        {
            InitializeComponent();
            sqlConn = new SqlConnection(Settings.Database.PxlConnectionString);
            sqlConn.Open();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BenSleurs Ben = new BenSleurs();
            TB_Voornaam.Text = Ben.GetFirstName();
            TB_Naam.Text = Ben.GetLastName();
            foreach (Hobby item in Ben.Hobbies)
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
            catch
            {

                throw;
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
            catch
            {
                throw;
            }
        }
        private void B_InsertFramework_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Klant klant = new Klant();
                klant.Voornaam = TB_Voornaam.Text;
                klant.Achternaam = TB_Naam.Text;
                sqlConn.AddObjectToDataBase(klant);
            }
            catch
            {
                throw;
            }

        }
        public void opslaandb()
        {

            var result = System.Windows.MessageBox.Show("wilt u de database aanpassen", "DB adjust mode", MessageBoxButton.YesNo);

            // If the no button was pressed ... 
            if (result == MessageBoxResult.No)
            {
                Close();
            }
            else
            {
                //sqlConn.DeleteAll("klanten");
                foreach (DataRowView dr in DG_Students.ItemsSource)
                {


                    Klant klant = CreateKlant(DG_Students, dr);
                    // update table klanten set klant as klant where klant.KlantId = (int) dr[0];

                    #region oude code
                    // bouw db terug op per klant

                    //  klant.KlantId = Convert.ToInt32(dr[0]);
                    /*
                    if (dr[0] == DBNull.Value)
                    {
                        klant.KlantId = int.MaxValue;
                    }
                    else
                    {
                        klant.KlantId = Convert.ToInt32(dr[0]);
                    }
                    // klant.Voornaam = dr[1].ToString();
                    if (dr[1] == DBNull.Value)
                    {
                        klant.Voornaam = "";
                    }
                    else
                    {
                        klant.Voornaam = dr[1].ToString();
                    }
                    //klant.Achternaam = dr[2].ToString();
                    if (dr[2] == DBNull.Value)
                    {
                        klant.Achternaam = "";
                    }
                    else
                    {
                        klant.Achternaam = dr[2].ToString();
                    }
                    //klant.Email = dr[3].ToString();
                    if (dr[3] == DBNull.Value)
                    {
                        klant.Email = "";
                    }
                    else
                    {
                        klant.Email = dr[3].ToString();
                    }
                    // klant.Telefoonnummer = dr[4].ToString();
                    if (dr[4] == DBNull.Value)
                    {
                        klant.Telefoonnummer = "";
                    }
                    else
                    {
                        klant.Telefoonnummer = dr[4].ToString();
                    }
                    // klant.Geboortedatum = Convert.ToDateTime(dr[5]);
                    if (dr[5] == DBNull.Value)
                    {

                        klant.Geboortedatum = null;
                    }
                    else
                    {
                        klant.Geboortedatum = Convert.ToDateTime(dr[5]);
                    }
                    //klant.RijbewijsA = Convert.ToBoolean(dr[6]);
                    if (dr[6] == DBNull.Value)
                    {
                        klant.RijbewijsA = false;
                    }
                    else
                    {
                        klant.RijbewijsA = Convert.ToBoolean(dr[6]);
                    }
                    //klant.RijbewijsB = Convert.ToBoolean(dr[7]);
                    if (dr[7] == DBNull.Value)
                    {
                        klant.RijbewijsB = false;
                    }
                    else
                    {
                        klant.RijbewijsB = Convert.ToBoolean(dr[7]);
                    }
                    //klant.Adres = dr[8].ToString();
                    if (dr[8] == DBNull.Value)
                    {
                        klant.Adres = "";
                    }
                    else
                    {
                        klant.Adres = dr[8].ToString();
                    }
                    //klant.Huisnummer = dr[9].ToString();
                    if (dr[9] == DBNull.Value)
                    {
                        klant.Huisnummer = 0;
                    }
                    else
                    {
                        klant.Huisnummer = Convert.ToInt32(dr[9]);
                    }
                    //klant.Bus = dr[10].ToString();
                    if (dr[10] == DBNull.Value)
                    {
                        klant.Bus = "";
                    }
                    else
                    {
                        klant.Bus = dr[10].ToString();
                    }
                    //klant.Postcode = Convert.ToInt32(dr[11]);
                    if (dr[11] == DBNull.Value)
                    {
                        klant.Postcode = 10000;
                    }
                    else
                    {
                        klant.Postcode = Convert.ToInt32(dr[11]);
                    }
                    //klant.Wachtwoord = dr[12].ToString();
                    if (dr[12] == DBNull.Value)
                    {
                        klant.Wachtwoord = "";
                    }
                    else
                    {
                        klant.Wachtwoord = dr[12].ToString();
                    }
                    //klant.IBan = dr[13].ToString();
                    if (dr[13] == DBNull.Value)
                    {
                        klant.Iban = "";
                    }
                    else
                    {
                        klant.Iban = dr[13].ToString();
                    }
                    //klant.RekeningHouder = dr[14].ToString();
                    if (dr[14] == DBNull.Value)
                    {
                        klant.RekeningHouder = "";
                    }
                    else
                    {
                        klant.RekeningHouder = dr[14].ToString();
                    }
                    //klant.VervalDatum = Convert.ToDateTime(dr[15]);
                    if (dr[15] == DBNull.Value)
                    {

                        klant.VervalDatum = null;
                    }
                    else
                    {
                        klant.VervalDatum = Convert.ToDateTime(dr[15]);
                    }
                    //klant.Salt = dr[16].ToString();
                    if (dr[16] == DBNull.Value)
                    {
                        klant.Salt = "";
                    }
                    else
                    {
                        klant.Salt = dr[16].ToString();
                    }

                    InsertResult Tr = sqlConn.Insert(klant);
                    */
                    #endregion
                }
            }
        }

        #region
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = System.Windows.MessageBox.Show("wilt u de database aanpassen", "DB adjust mode", MessageBoxButton.YesNo);

            // If the no button was pressed ... 
            if (result == MessageBoxResult.No)
            {
                Close();
            }
            else
            {
                opslaandb();
            }
            #endregion



        }
        private static Klant DbNullControle(Klant klant, string columnName, string col, int IfnullGiveThis, DataRowView dr, int columnNummer)
        {

            if (dr[columnNummer] == DBNull.Value)
            {
                klant.KlantId = IfnullGiveThis;
            }
            else
            {
                klant.KlantId = Convert.ToInt32(dr[columnNummer]);
            }
            return klant;
        }
        private static Klant CreateKlant(DataGrid dataGrid, DataRowView row)
        {
            var arr = dataGrid.Columns.ToArray();
            Klant klant = new Klant();
            PropertyInfo[] properties = klant.GetType().GetProperties();
            //loops over all properties of klant
            foreach (PropertyInfo item in properties)
            {
                int index = -1;
                string propName = item.Name.ToLower();
                //loops over all columns in datagrid
                for (int i = 0; i < arr.Length; i++)
                {

                    if (arr[i].Header.ToString().ToLower() == propName)
                    {
                        index = i;
                        break;
                    }
                    if (row[i] == DBNull.Value)
                    {
                        row[i] = null;
                    }
                }
                if (index != -1)
                {
                    var a = row[index].GetType();
                    var b = a.ToString();
                    if (row[index] == DBNull.Value)
                    {
                        row[index] = null;
                    }
                    item.SetValue(klant, row[index]);
                }
            }
            return klant;
        }

        public void Dispose()
        {
            sqlConn.Close();
        }

        private void DG_Students_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            dataGrid = sender as DataGrid;
            row = dataGrid.SelectedItem as DataRowView;
        }

        private void DG_Students_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (row != null)
            {
                Klant klant = new Klant();
                klant.KlantId = Convert.ToInt32(row[0]);
                klant.Achternaam = row[2].ToString();
                sqlConn.UpdateByObject(klant);
            }

        }

        private void B_UpdateFramework_Click(object sender, RoutedEventArgs e)
        {
            opslaandb();
        }

        private void ButtonTestMail(object sender, RoutedEventArgs e)
        {
            try
            {
                var mail = new PxlMail("Kristof.evaert@gmail.com");
                mail.Body = "Dit is een test met html <ul><li>hallo</li></ul>";
                mail.Subject = "testsubject";
                mail.SendMail();
            }
            catch (Exception ex)
            {

                throw;
            }


        }
    }
}
