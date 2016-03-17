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

namespace TicketsTacGui
{
    /// <summary>
    /// Interaction logic for CreateClientPage.xaml
    /// </summary>
    public partial class CreateClientPage : Page
    {
        public CreateClientPage()
        {
            InitializeComponent();

            foreach ( Rank value in Enum.GetValues(typeof(Rank)) )
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Tag = (int)value;
                item.Content = value.ToString();

                comboBoxRank.Items.Add(item);
            }
        }

        private void buttonSubmit_Click(object sender, RoutedEventArgs e)
        {
            List<string> fields = new List<string> { "Username", "Password", "Email", "Rank", "Created" };
            List<string> values = new List<string> { textBoxUsername.Text, textBoxPassword.Text.ToSHA1(), textBoxEmail.Text, ((ComboBoxItem)comboBoxRank.SelectedItem).Tag.ToString(), DB.getTimestamp().ToString() };

            DB.Insert(fields, values, "Users");
            NavigationService.GoBack();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
