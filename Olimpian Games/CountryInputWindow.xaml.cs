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
    public partial class CountryInputWindow : Window
    {
        public string Country { get; private set; }

        public CountryInputWindow()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Country = countryTextBox.Text;
            this.DialogResult = true;
            this.Close();
        }
    }
}
