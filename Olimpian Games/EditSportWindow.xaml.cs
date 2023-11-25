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
    public partial class EditSportWindow : Window
    {
        private readonly Sport _sport;

        public event Action SportEdited;

        public EditSportWindow(Sport sport)
        {
            InitializeComponent();

            _sport = sport;

            NameSportTextBox.Text = sport.Name;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            _sport.Name = NameSportTextBox.Text;

            using (var context = new OlimpianGamesContext())
            {
                context.Entry(_sport).State = EntityState.Modified;
                context.SaveChanges();
            }

            SportEdited?.Invoke();

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
