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
    public partial class EditParticipantWindow : Window
    {
        private readonly Participant _participant;

        public event Action ParticipantEdited;

        public EditParticipantWindow(Participant participant)
        {
            InitializeComponent();

            _participant = participant;

            firstNameTextBox.Text = participant.FirstName;
            lastNameTextBox.Text = participant.LastName;
            countryTextBox.Text = participant.Country;
            birthDatePicker.SelectedDate = participant.BirthDate;
            sportTextBox.Text = participant.Sport?.Name;
            medalComboBox.SelectedItem = participant.Medal;
            olympiadYearTextBox.Text = participant.Olympiad?.Year.ToString();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            _participant.FirstName = firstNameTextBox.Text;
            _participant.LastName = lastNameTextBox.Text;
            _participant.Country = countryTextBox.Text;
            _participant.BirthDate = birthDatePicker.SelectedDate.HasValue ? birthDatePicker.SelectedDate.Value : DateTime.MinValue;

            using (var context = new OlimpianGamesContext())
            {
                var sport = context.Sports.FirstOrDefault(s => s.Name == sportTextBox.Text);
                if (sport == null)
                {
                    sport = new Sport { Name = sportTextBox.Text };
                    context.Sports.Add(sport);
                }

                _participant.SportId = sport.SportId;
                _participant.Medal = ((ComboBoxItem)medalComboBox.SelectedItem).Content.ToString();

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

                _participant.OlympiadId = olympiad.OlympiadId;

                context.Entry(_participant).State = EntityState.Modified;
                context.SaveChanges();

                olympiad.ParticipantsCount = context.Participants.Count(p => p.OlympiadId == olympiad.OlympiadId);
                context.SaveChanges();

                sport.ParticipantsCount = context.Participants.Count(p => p.SportId == sport.SportId);
                context.SaveChanges();
            }

            ParticipantEdited?.Invoke();

            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }


}
