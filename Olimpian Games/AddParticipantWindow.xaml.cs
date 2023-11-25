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

namespace Olimpian_Games
{
    public partial class AddParticipantWindow : Window
    {
        public event Action ParticipantAdded;

        public AddParticipantWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var participant = new Participant
            {
                FirstName = firstNameTextBox.Text,
                LastName = lastNameTextBox.Text,
                Country = countryTextBox.Text,
                BirthDate = birthDatePicker.SelectedDate.HasValue ? birthDatePicker.SelectedDate.Value : DateTime.MinValue
            };

            using (var context = new OlimpianGamesContext())
            {
                var sport = context.Sports.FirstOrDefault(s => s.Name == sportTextBox.Text);
                if (sport == null)
                {
                    sport = new Sport { Name = sportTextBox.Text };
                    context.Sports.Add(sport);
                }

                participant.SportId = sport.SportId;
                participant.Medal = ((ComboBoxItem)medalComboBox.SelectedItem).Content.ToString();

                int year = int.Parse(olympiadYearTextBox.Text);
                var olympiad = context.Olympiads.FirstOrDefault(o => o.Year == year);
                if (olympiad == null)
                {
                    var addOlympiadaWindow = new AddOlympiadaWindow();
                    var result = addOlympiadaWindow.ShowDialog();
                    if (result.HasValue && result.Value)
                    {
                        olympiad = context.Olympiads.FirstOrDefault(o => o.Year == year);
                        if (olympiad == null)
                        {
                            MessageBox.Show("Олімпіада все ще не знайдена");
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                participant.OlympiadId = olympiad.OlympiadId;

                context.Participants.Add(participant);
                context.SaveChanges();

                olympiad.ParticipantsCount = context.Participants.Count(p => p.OlympiadId == olympiad.OlympiadId);
                context.SaveChanges();

                sport.ParticipantsCount = context.Participants.Count(p => p.SportId == sport.SportId);
                context.SaveChanges();
            }

            ParticipantAdded?.Invoke();

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }


}
