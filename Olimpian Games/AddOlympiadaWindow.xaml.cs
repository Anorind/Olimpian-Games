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
    public partial class AddOlympiadaWindow : Window
    {
        public event Action OlimpiadaAdded;
        public AddOlympiadaWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var olympiad = new Olympiada
            {
                Year = int.Parse(yearTextBox.Text),
                Season = seasonTextBox.Text,
                Country = countryTextBox.Text,
                City = cityTextBox.Text
            };

            using (var context = new OlimpianGamesContext())
            {
                context.Olympiads.Add(olympiad);
                context.SaveChanges();
            }
            OlimpiadaAdded?.Invoke();

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
