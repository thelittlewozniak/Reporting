using Reporting.model;
using Reporting.services.GoogleMapsMatrixService;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using NsExcel = Microsoft.Office.Interop.Excel;

namespace Reporting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string HomeAdressString { get; set; }
        public string WorkAdressString { get; set; }
        public List<Trip> Report { get; set; } = new List<Trip>();
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
            this.Trips.ItemsSource = null;
            this.Trips.ItemsSource = Report;
        }
        public void ListToExcel(List<Trip> list)
        {
            NsExcel.ApplicationClass excapp = new Microsoft.Office.Interop.Excel.ApplicationClass();

            excapp.Visible = true;

            var workbook = excapp.Workbooks.Add(NsExcel.XlWBATemplate.xlWBATWorksheet);
        
            var sheet = (NsExcel.Worksheet)workbook.Sheets[1]; //indexing starts from 1

            var range = sheet.get_Range("A1", "A1");
            range.Value2 = "test";

            //now the list
            string cellName;
            string cellNameB;
            string cellNameC;
            string cellNameD;
            string cellNameE;
            int counter = 1;
            foreach (var item in list)
            {
                cellName = "A" + counter.ToString();
                cellNameB = "B" + counter.ToString();
                cellNameC = "C" + counter.ToString();
                cellNameD = "D" + counter.ToString();
                cellNameE = "E" + counter.ToString();
                var range1 = sheet.get_Range(cellName, cellName);
                var range2 = sheet.get_Range(cellNameB, cellNameB);
                var range3 = sheet.get_Range(cellNameC, cellNameC);
                var range4 = sheet.get_Range(cellNameD, cellNameD);
                var range5 = sheet.get_Range(cellNameE, cellNameE);
                range1.Value2 = item.DateTrip.ToString();
                range2.Value2 = item.Code.ToString();
                range3.Value2 = item.From.ToString();
                range4.Value2 = item.To.ToString();
                range5.Value2 = item.Distance.ToString();
                ++counter;
            }
        }

        private void ExportInExcel_Click(object sender, RoutedEventArgs e)
        {
            this.ListToExcel(this.Report);
        }
    }
}
