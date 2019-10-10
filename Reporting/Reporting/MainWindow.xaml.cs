using Reporting.model;
using Reporting.services;
using Reporting.services.GoogleMapsMatrixService;
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

namespace Reporting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string HomeAdressString { get; set; }
        public string WorkAdressString { get; set; }
        public List<Trip> Report { get; set; }
        private GoogleMapsMatrixService googleMapsMatrixService;
        public MainWindow()
        {
            InitializeComponent();
            googleMapsMatrixService = new GoogleMapsMatrixService("");
        }

        private void HomeAdressSet_Click(object sender, RoutedEventArgs e)
        {
            if(this.HomeAdress.Text != "Put your home adress here")
            {
                if (this.HomeAdressString == null)
                {
                    this.HomeAdressString = this.HomeAdress.Text;
                    this.HomeAdress.IsEnabled = false;
                    this.HomeAdressSet.IsEnabled = false;
                }
            }
            if (this.HomeAdressString != null && this.WorkAdressString != null)
                this.UpdateKilometersBetweenWorkAndHome();
        }
        private async void UpdateKilometersBetweenWorkAndHome()
        {
            var kilometers = await this.googleMapsMatrixService.GetKilometers(this.HomeAdressString, this.WorkAdressString);
            this.KilometersHomeWork.Text = kilometers + " kilometers";
        }

        private void WorkAdressSet_Click(object sender, RoutedEventArgs e)
        {
            if (this.WorkAdress.Text != "Put your work adress here")
            {
                if (this.WorkAdressString == null)
                {
                    this.WorkAdressString = this.WorkAdress.Text;
                    this.WorkAdress.IsEnabled = false;
                    this.WorkAdressSet.IsEnabled = false;
                }
            }
            if (this.HomeAdressString != null && this.WorkAdressString != null)
                this.UpdateKilometersBetweenWorkAndHome();
        }

        private void HomeAdressOrigin_Click(object sender, RoutedEventArgs e)
        {
            this.Origin.Text = this.HomeAdressString;
        }

        private void WorkAdressOrigin_Click(object sender, RoutedEventArgs e)
        {
            this.Origin.Text = this.WorkAdressString;
        }

        private void HomeAdressDestination_Click(object sender, RoutedEventArgs e)
        {
            this.Destination.Text = this.HomeAdressString;
        }

        private void WorkAdressDestination_Click(object sender, RoutedEventArgs e)
        {
            this.Destination.Text = this.WorkAdressString;
        }

        private async void AddToYourReport_Click(object sender, RoutedEventArgs e)
        {
            var trip = new Trip
            {
                From = this.Origin.Text,
                To = this.Destination.Text,
                Code = ((ListBoxItem)this.MovementCode.SelectedItem).Content.ToString(),
                DateTrip = this.DateOfTrip.SelectedDate,
                Distance = await this.googleMapsMatrixService.GetKilometers(this.Origin.Text, this.Destination.Text)
            };
            Report.Add(trip);
        }
    }
}
