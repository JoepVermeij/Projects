using ClassLibTeam10.Business;
using ClassLibTeam10.Data;
using ClassLibTeam10.Data.Bestellingen;
using ClassLibTeam10.Data.Framework;
using ClassLibTeam10.Entities;
using ClassLibTeam10.Entities.DbEntities;
using ClassLibTeam10.Settings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for StartUpWindow.xaml
    /// </summary>
    public partial class StartUpWindow : Window, IDisposable
    {
        private SqlConnection sqlConn;
        DataSet team10;
        SqlDataAdapter adapter;

        public StartUpWindow()
        {
            InitializeComponent();
            sqlConn = new SqlConnection(Settings.Database.PxlConnectionString);
            sqlConn.Open();
            team10 = new DataSet();

        }

        private void GetDbKlantenButton_Click(object sender, RoutedEventArgs e)
        {
            GetDbProducten.Visibility = Visibility.Collapsed;
            GetDbBestellingen.Visibility = Visibility.Collapsed;
            UpdateDbProducten.Visibility = Visibility.Collapsed;
            UpdateDbBestellingen.Visibility = Visibility.Collapsed;
            UpdateDbKlanten.Visibility = Visibility.Visible;
            GetDbKlanten.Visibility = Visibility.Collapsed;
            if (team10.Tables["klanten"] != null)
            {
                team10.Tables["klanten"].Clear();
            }

            adapter = new SqlDataAdapter("Select * from klanten", sqlConn);

            adapter.Fill(team10, "klanten");
            DG_Data.ItemsSource = team10.Tables["klanten"].DefaultView;

        }

        private void GetDbProductenButton_Click(object sender, RoutedEventArgs e)
        {

            GetDbKlanten.Visibility = Visibility.Collapsed;
            GetDbBestellingen.Visibility = Visibility.Collapsed;
            UpdateDbKlanten.Visibility = Visibility.Collapsed;
            UpdateDbBestellingen.Visibility = Visibility.Collapsed;
            UpdateDbProducten.Visibility = Visibility.Visible;
            GetDbProducten.Visibility = Visibility.Collapsed;
            if (team10.Tables["producten"] != null)
            {
                team10.Tables["producten"].Clear();
            }
            adapter = new SqlDataAdapter("Select * from producten", sqlConn);
            adapter.Fill(team10, "producten");
            DG_Data.ItemsSource = team10.Tables["producten"].DefaultView;
        }

        private void GetDbBestellingenButton_Click(object sender, RoutedEventArgs e)
        {
            GetDbProducten.Visibility = Visibility.Collapsed;
            GetDbKlanten.Visibility = Visibility.Collapsed;
            UpdateDbProducten.Visibility = Visibility.Collapsed;
            UpdateDbKlanten.Visibility = Visibility.Collapsed;
            UpdateDbBestellingen.Visibility = Visibility.Visible;
            GetDbBestellingen.Visibility = Visibility.Collapsed;
            if (team10.Tables["bestellingen"] != null)
            {
                team10.Tables["bestellingen"].Clear();
            }
            adapter = new SqlDataAdapter("Select * from bestellingen", sqlConn);
            adapter.Fill(team10, "bestellingen");
            DG_Data.ItemsSource = team10.Tables["bestellingen"].DefaultView;
        }

        public void Dispose()
        {
            sqlConn.Close();
        }

        private void UpdateKlantenButton_Click(object sender, RoutedEventArgs e)
        {

            GetDbProducten.Visibility = Visibility.Visible;
            GetDbKlanten.Visibility = Visibility.Visible;
            GetDbBestellingen.Visibility = Visibility.Visible;
            UpdateDbProducten.Visibility = Visibility.Collapsed;
            UpdateDbKlanten.Visibility = Visibility.Collapsed;
            UpdateDbBestellingen.Visibility = Visibility.Collapsed;
            foreach (DataRow row in team10.Tables["klanten"].Rows)
            {

                if (row.RowState != DataRowState.Unchanged)
                {
                    if (row.RowState == DataRowState.Modified)
                    {
                        Klant klant = HandleProfiel.CreateKlant(row);
                        HandleProfiel.UpdateFullKlant(sqlConn, klant);
                    }
                    else if (row.RowState == DataRowState.Added)
                    {
                        Klant klant = HandleProfiel.CreateKlant(row);
                        sqlConn.AddObjectToDataBase(klant);
                    }
                    else if (row.RowState == DataRowState.Deleted)
                    {
                        Klant klant = new Klant();
                        klant.KlantId = Convert.ToInt32(row[0, DataRowVersion.Original]);
                        sqlConn.DeleteByObject(klant);
                    }
                }

            }
        }
        private void UpdateProductenButton_Click(object sender, RoutedEventArgs e)
        {
            GetDbProducten.Visibility = Visibility.Visible;
            GetDbKlanten.Visibility = Visibility.Visible;
            GetDbBestellingen.Visibility = Visibility.Visible;
            UpdateDbProducten.Visibility = Visibility.Collapsed;
            UpdateDbKlanten.Visibility = Visibility.Collapsed;
            UpdateDbBestellingen.Visibility = Visibility.Collapsed;
            foreach (DataRow row in team10.Tables["producten"].Rows)
            {

                if (row.RowState != DataRowState.Unchanged)
                {
                    if (row.RowState == DataRowState.Modified)
                    {
                        Product product = HandleProducts.CreateFullProduct(row);
                        sqlConn.UpdateByObject(product);
                    }
                    else if (row.RowState == DataRowState.Added)
                    {
                        Product product = HandleProducts.CreateFullProduct(row);
                        sqlConn.AddObjectToDataBase(product);
                    }
                    else if (row.RowState == DataRowState.Deleted)
                    {
                        Product product = new Product();
                        product.ProductId = Convert.ToInt32(row[0, DataRowVersion.Original]);
                        sqlConn.DeleteByObject(product);
                    }
                }

            }

        }

        private void UpdateBestellingenButton_Click(object sender, RoutedEventArgs e)
        {
            GetDbProducten.Visibility = Visibility.Visible;
            GetDbKlanten.Visibility = Visibility.Visible;
            GetDbBestellingen.Visibility = Visibility.Visible;
            UpdateDbProducten.Visibility = Visibility.Collapsed;
            UpdateDbKlanten.Visibility = Visibility.Collapsed;
            UpdateDbBestellingen.Visibility = Visibility.Collapsed;
            foreach (DataRow row in team10.Tables["bestellingen"].Rows)
            {

                if (row.RowState != DataRowState.Unchanged)
                {
                    if (row.RowState == DataRowState.Modified)
                    {
                        Bestelling bestelling = HandleBestellingen.CreateFullBestelling(row);
                        sqlConn.UpdateByObject(bestelling);
                    }
                    else if (row.RowState == DataRowState.Added)
                    {
                        Bestelling bestelling = HandleBestellingen.CreateFullBestelling(row);
                        sqlConn.AddObjectToDataBase(bestelling);
                    }
                    else if (row.RowState == DataRowState.Deleted)
                    {
                        Bestelling bestelling = new Bestelling();
                        bestelling.BestelId = Convert.ToInt32(row[0, DataRowVersion.Original]);
                        sqlConn.DeleteByObject(bestelling);
                    }
                }

            }
        }
    }
}
