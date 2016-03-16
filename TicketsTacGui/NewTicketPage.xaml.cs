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

            label_name_name.Content = this.Project.Nom;
        }

        private void textBox_ticket_name_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void buttonCreateTicket_Click(object sender, RoutedEventArgs e)
        {
            var auteur = User.currentUser;
            List<User> UserAssign = new List<User>();
            List<String> field = new List<string>();
            List<String> value = new List<string>();
            List<string> fieldCreationTicket = new List<string>();
            List<string> valueCreationTicket = new List<string>();

            fieldCreationTicket.Add("Name");
            fieldCreationTicket.Add("projet");
            fieldCreationTicket.Add("problem_description");
            fieldCreationTicket.Add("state");
            fieldCreationTicket.Add("auteur");



            valueCreationTicket.Add(textBox_ticket_name.Text);
            valueCreationTicket.Add(this.Project.Id.ToString());
            valueCreationTicket.Add(textBlock_ticket_description.Text);
            valueCreationTicket.Add("4");
            valueCreationTicket.Add(auteur.Id.ToString());

            int id = DB.Insert(fieldCreationTicket, valueCreationTicket, "Tickets");


            field.Add("Ticket_Id");
            field.Add("User_Id");

            
            for (int i = 0; i < comboBox_assignee.SelectedItems.Count; i++)
            {
                UserAssign.Add(User.Get(int.Parse(comboBox_assignee.SelectedItems.ElementAt(i).ToString())));
                value.Add(id.ToString());
                value.Add(UserAssign.ToString());

            }

            Ticket created = new Ticket(id, textBox_ticket_name.Text, textBlock_ticket_description.Text, this.Project, auteur);

            DB.Insert("UserAssign", "Ticket_assignee");
            //User u = new User(DB.Get(int.Parse(((ComboBoxItem)comboBox_assignee.SelectedItem).Tag.ToString()), "Users"));

            NavigationService.Navigate(new ProjectsListPage());
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProjectsListPage());
        }
    }
}
