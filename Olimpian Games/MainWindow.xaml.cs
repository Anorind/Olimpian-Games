using System.Data.Entity;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Text;
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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            AddData();
            InitializeComponent();
        }
        public void AddData()
        {
            using (var context = new OlimpianGamesContext())
            {
                var olympiad1 = new Olympiada { Year = 2008, Season = "Summer", Country = "China", City = "Beijing", ParticipantsCount = 0 };
                var olympiad2 = new Olympiada { Year = 2012, Season = "Summer", Country = "United Kingdom", City = "London", ParticipantsCount = 0 };

                context.Olympiads.Add(olympiad1);
                context.Olympiads.Add(olympiad2);

                context.SaveChanges();

                var sport1 = new Sport { Name = "Swimming", ParticipantsCount = 0 };
                var sport2 = new Sport { Name = "Athletics", ParticipantsCount = 0 };
                var sport3 = new Sport { Name = "Gymnastics", ParticipantsCount = 0 };

                context.Sports.Add(sport1);
                context.Sports.Add(sport2);
                context.Sports.Add(sport3);

                context.SaveChanges();

                for (int i = 1; i <= 20; i++)
                {
                    var participant = new Participant
                    {
                        FirstName = $"Participant{i}",
                        LastName = $"LastName{i}",
                        Country = i % 2 == 0 ? "USA" : "Ukraine",
                        BirthDate = new DateTime(1980 + i, 1, 1),
                        SportId = i % 3 == 0 ? sport1.SportId : (i % 3 == 1 ? sport2.SportId : sport3.SportId),
                        OlympiadId = i % 2 == 0 ? olympiad1.OlympiadId : olympiad2.OlympiadId,
                        Medal = i % 4 == 0 ? "Gold" : (i % 4 == 1 ? "Silver" : (i % 4 == 2 ? "Bronze" : null))
                    };

                    context.Participants.Add(participant);
                }

                context.SaveChanges();

                sport1.ParticipantsCount = context.Participants.Count(p => p.SportId == sport1.SportId);
                sport2.ParticipantsCount = context.Participants.Count(p => p.SportId == sport2.SportId);
                sport3.ParticipantsCount = context.Participants.Count(p => p.SportId == sport3.SportId);

                olympiad1.ParticipantsCount = context.Participants.Count(p => p.OlympiadId == olympiad1.OlympiadId);
                olympiad2.ParticipantsCount = context.Participants.Count(p => p.OlympiadId == olympiad2.OlympiadId);

                context.SaveChanges();
            }
        }

        private void tablesComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            using (var context = new OlimpianGamesContext())
            {
                var tableNames = context.GetType().GetProperties()
                    .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                    .Select(p => p.Name);

                tablesComboBox.ItemsSource = tableNames;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedTable = tablesComboBox.SelectedItem.ToString();
            Type selectedType = null;

            if (selectedTable == "Participants")
            {
                var addWindow = new AddParticipantWindow();
                addWindow.ParticipantAdded += UpdateDataForParticipants;
                addWindow.Show();
            }
            else if (selectedTable == "Olympiads")
            {
                var addWindow = new AddOlympiadaWindow();
                addWindow.OlimpiadaAdded += UpdateDataForOlympiada;
                addWindow.Show();

            }
            else if (selectedTable == "Sports")
            {
                var addWindow = new AddSportWindow();
                addWindow.SportAdded += UpdateDataForSport;
                addWindow.Show();
            }
            else
            {
                MessageBox.Show("Виберіть таблицю");
            }
        }

        private void UpdateDataForOlympiada()
        {
            using (var context = new OlimpianGamesContext())
            {
                dataGrid.ItemsSource = context.Olympiads.ToList();
            }
        }
        private void UpdateDataForParticipants()
        {
            using (var context = new OlimpianGamesContext())
            {
                dataGrid.ItemsSource = context.Participants.ToList();
            }
        }
        private void UpdateDataForSport()
        {
            using (var context = new OlimpianGamesContext())
            {
                dataGrid.ItemsSource = context.Sports.ToList();
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new OlimpianGamesContext())
            {
                var selected = dataGrid.SelectedItem;

                if (selected is Participant participant)
                {

                    var participantInDb = context.Participants.Find(participant.ParticipantId);

                    if (participantInDb != null)
                    {
                        context.Participants.Remove(participantInDb);
                        context.SaveChanges();


                        var sportForParticipant = context.Sports.Find(participant.SportId);
                        sportForParticipant.ParticipantsCount = context.Participants.Count(p => p.SportId == sportForParticipant.SportId);

                        var olympiad = context.Olympiads.Find(participant.OlympiadId);
                        olympiad.ParticipantsCount = context.Participants.Count(p => p.OlympiadId == olympiad.OlympiadId);
                    }
                    using (var context1 = new OlimpianGamesContext())
                    {
                        dataGrid.ItemsSource = context1.Participants.ToList();
                    }
                }
                else if (selected is Sport sport)
                {

                    var participantsInSport = context.Participants.Where(p => p.SportId == sport.SportId).ToList();
                    foreach (var participantInSport in participantsInSport)
                    {
                        context.Participants.Remove(participantInSport);
                    }

                    context.SaveChanges();


                    var olympiads = context.Olympiads.ToList();
                    foreach (var olympiad in olympiads)
                    {
                        olympiad.ParticipantsCount = context.Participants.Count(p => p.OlympiadId == olympiad.OlympiadId);
                    }


                    var sportInDb = context.Sports.Find(sport.SportId);
                    if (sportInDb != null)
                    {
                        context.Sports.Remove(sportInDb);
                    }

                    context.SaveChanges();

                    using (var context1 = new OlimpianGamesContext())
                    {
                        dataGrid.ItemsSource = context1.Sports.ToList();
                    }
                }
                else if (selected is Olympiada olympiad)
                {

                    var olympiadInDb = context.Olympiads.Find(olympiad.OlympiadId);

                    if (olympiadInDb != null)
                    {

                        var participantsInOlympiad = context.Participants.Where(p => p.OlympiadId == olympiadInDb.OlympiadId).ToList();
                        foreach (var participantInOlympiad in participantsInOlympiad)
                        {
                            context.Participants.Remove(participantInOlympiad);
                        }

                        context.SaveChanges();


                        var sports = context.Sports.ToList();
                        foreach (var sportInOlimpiada in sports)
                        {
                            sportInOlimpiada.ParticipantsCount = context.Participants.Count(p => p.SportId == sportInOlimpiada.SportId);
                        }

                        context.Olympiads.Remove(olympiadInDb);
                        context.SaveChanges();

                        using (var context1 = new OlimpianGamesContext())
                        {
                            dataGrid.ItemsSource = context1.Olympiads.ToList();
                        }
                    }
                }
                context.SaveChanges();
            }
        }





        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is Olympiada selectedOlympiad)
            {
                var editWindow = new EditOlympiadaWindow(selectedOlympiad);
                editWindow.OlympiadaEdited += () => RefreshDataGrid();
                editWindow.Show();
            }
            if (dataGrid.SelectedItem is Sport selectedSport)
            {
                var editWindow = new EditSportWindow(selectedSport);
                editWindow.SportEdited += () => RefreshDataGrid();
                editWindow.Show();
            }
            if (dataGrid.SelectedItem is Participant selectedParticipant)
            {
                var editWindow = new EditParticipantWindow(selectedParticipant);
                editWindow.ParticipantEdited += () => RefreshDataGrid();
                editWindow.Show();
            }
        }
        private void RefreshDataGrid()
        {
            using (var context = new OlimpianGamesContext())
            {
                switch (tablesComboBox.SelectedItem.ToString())
                {
                    case "Participants":
                        dataGrid.ItemsSource = context.Participants.ToList();
                        break;
                    case "Olympiads":
                        dataGrid.ItemsSource = context.Olympiads.ToList();
                        break;
                    case "Sports":
                        dataGrid.ItemsSource = context.Sports.ToList();
                        break;
                    default:
                        dataGrid.ItemsSource = null;
                        break;
                }
            }
        }


        private void StatsButton_Click(object sender, RoutedEventArgs e)
        {
            var statsWindow = new StatsWindow();
            statsWindow.QueryRequested += ExecuteQuery;
            statsWindow.Show();
        }
        private void ExecuteQuery(string queryId, string country = null)
        {
            using (var context = new OlimpianGamesContext())
            {
                switch (queryId)
                {
                    case "MedalTable":
                        var medalTable = context.Participants
                            .GroupBy(p => p.Country)
                            .Select(g => new { Country = g.Key, Medals = g.Count(p => p.Medal != null) })
                            .OrderByDescending(g => g.Medals)
                            .ToList();
                        dataGrid.ItemsSource = medalTable;
                        break;

                    case "Medalists":
                        var medalists = context.Participants
                            .Where(p => p.Medal != null)
                            .Select(p => new { p.FirstName, p.LastName, Sport = p.Sport.Name, p.Medal })
                            .ToList();
                        dataGrid.ItemsSource = medalists;
                        break;

                    case "MostGoldMedals":
                        var mostGoldMedals = context.Participants
                            .Where(p => p.Medal == "Gold")
                            .GroupBy(p => p.Country)
                            .Select(g => new { Country = g.Key, GoldMedals = g.Count() })
                            .OrderByDescending(g => g.GoldMedals)
                            .FirstOrDefault();
                        dataGrid.ItemsSource = mostGoldMedals != null ? new[] { mostGoldMedals } : null;
                        break;

                    case "MostGoldMedalsInSport":
                        var mostGoldMedalsInSport = context.Participants
                            .Where(p => p.Medal == "Gold")
                            .GroupBy(p => p.Sport.Name)
                            .Select(g => new { Sport = g.Key, GoldMedals = g.Count() })
                            .OrderByDescending(g => g.GoldMedals)
                            .FirstOrDefault();
                        dataGrid.ItemsSource = mostGoldMedalsInSport != null ? new[] { mostGoldMedalsInSport } : null;
                        break;

                    case "MostHostedOlympiads":
                        var mostHostedOlympiads = context.Olympiads
                            .GroupBy(o => o.Country)
                            .Select(g => new { Country = g.Key, TimesHosted = g.Count() })
                            .OrderByDescending(g => g.TimesHosted)
                            .FirstOrDefault();
                        dataGrid.ItemsSource = mostHostedOlympiads != null ? new[] { mostHostedOlympiads } : null;
                        break;

                    case "TeamComposition":
                        
                        var teamComposition = context.Participants
                            .Where(p => p.Country == country)
                            .Select(p => new { p.FirstName, p.LastName, Sport = p.Sport.Name })
                            .ToList();
                        dataGrid.ItemsSource = teamComposition;
                        break;

                    case "CountryStats":
                       
                        var countryStats = context.Participants
                            .Where(p => p.Country == country)
                            .GroupBy(p => p.Olympiad.Year)
                            .Select(g => new { Year = g.Key, Medals = g.Count(p => p.Medal != null) })
                            .OrderBy(g => g.Year)
                            .ToList();
                        dataGrid.ItemsSource = countryStats;
                        break;

                    default:
                        dataGrid.ItemsSource = null;
                        break;
                }
            }
        }


        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void tablesComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            using (var context = new OlimpianGamesContext())
            {
                switch (tablesComboBox.SelectedItem.ToString())
                {
                    case "Participants":
                        dataGrid.ItemsSource = context.Participants.ToList();
                        break;
                    case "Olympiads":
                        dataGrid.ItemsSource = context.Olympiads.ToList();
                        break;
                    case "Sports":
                        dataGrid.ItemsSource = context.Sports.ToList();
                        break;
                    default:
                        dataGrid.ItemsSource = null;
                        break;
                }
            }
        }
    }
}