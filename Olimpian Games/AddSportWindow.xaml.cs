using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Olimpian_Games
{
    public partial class AddSportWindow : Window
    {
        public event Action SportAdded;
        public AddSportWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var sport = new Sport
            {
                Name = NameSportTextBox.Text,
            };
            using (var context = new OlimpianGamesContext())
            {              
                context.Sports.Add(sport);
                context.SaveChanges();
            }
            SportAdded?.Invoke();

            this.Close();
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
