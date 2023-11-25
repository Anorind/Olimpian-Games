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

namespace Olimpian_Games
{
    public partial class StatsWindow : Window
    {
        public event Action<string, string> QueryRequested;

        public StatsWindow()
        {
            InitializeComponent();
        }

        private void MedalTableButton_Click(object sender, RoutedEventArgs e)
        {
            QueryRequested?.Invoke("MedalTable", null);
            this.Close();
        }

        private void MedalistsButton_Click(object sender, RoutedEventArgs e)
        {
            QueryRequested?.Invoke("Medalists", null);
            this.Close();
        }

        private void MostGoldMedalsButton_Click(object sender, RoutedEventArgs e)
        {
            QueryRequested?.Invoke("MostGoldMedals", null);
            this.Close();
        }

        private void MostGoldMedalsInSportButton_Click(object sender, RoutedEventArgs e)
        {
            QueryRequested?.Invoke("MostGoldMedalsInSport", null);
            this.Close();
        }

        private void MostHostedOlympiadsButton_Click(object sender, RoutedEventArgs e)
        {
            QueryRequested?.Invoke("MostHostedOlympiads", null);
            this.Close();
        }

        private void TeamCompositionButton_Click(object sender, RoutedEventArgs e)
        {
            var countryInputWindow = new CountryInputWindow();
            var result = countryInputWindow.ShowDialog();
            if (result.HasValue && result.Value)
            {
                QueryRequested?.Invoke("TeamComposition", countryInputWindow.Country);
            }
            this.Close();
        }

        private void CountryStatsButton_Click(object sender, RoutedEventArgs e)
        {
            var countryInputWindow = new CountryInputWindow();
            var result = countryInputWindow.ShowDialog();
            if (result.HasValue && result.Value)
            {
                QueryRequested?.Invoke("CountryStats", countryInputWindow.Country);
            }
            this.Close();
        }
    }

}
