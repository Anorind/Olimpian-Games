using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public partial class EditOlympiadaWindow : Window
    {
        private readonly Olympiada _olympiad;

        public event Action OlympiadaEdited;

        public EditOlympiadaWindow(Olympiada olympiad)
        {
            InitializeComponent();

            _olympiad = olympiad;

            yearTextBox.Text = olympiad.Year.ToString();
            seasonTextBox.Text = olympiad.Season;
            countryTextBox.Text = olympiad.Country;
            cityTextBox.Text = olympiad.City;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            _olympiad.Year = int.Parse(yearTextBox.Text);
            _olympiad.Season = seasonTextBox.Text;
            _olympiad.Country = countryTextBox.Text;
            _olympiad.City = cityTextBox.Text;

            using (var context = new OlimpianGamesContext())
            {
                context.Entry(_olympiad).State = EntityState.Modified;
                context.SaveChanges();
            }

            OlympiadaEdited?.Invoke();

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
