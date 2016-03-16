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
    /// Logique d'interaction pour NewTicketPage.xaml
    /// </summary>
    public partial class NewTicketPage : Page
    {
        private Projet Project { get; set; }
        public NewTicketPage(int id)
        {
            this.Project = Projet.GetProjetFromBDD(id);
            InitializeComponent();
        }

        private void textBox_ticket_name_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void buttonCreateTicket_Click(object sender, RoutedEventArgs e)
        {
            
            //User u = new User(DB.Get(int.Parse(((ComboBoxItem)comboBox_assignee.SelectedItem).Tag.ToString()), "Users"));

            Ticket created = new Ticket(textBox_ticket_name.Text, textBlock_ticket_description.Text, this.Project, );
            NavigationService.Navigate(new ProjectsListPage());
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProjectsListPage());
        }
    }
}
