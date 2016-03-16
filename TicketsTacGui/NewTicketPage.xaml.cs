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
            List<string> fieldsAssignees = new List<string> { "Ticket_Id", "Assignee_Id" }; //On déclare valuesAssignees plus tard étant donné que la liste varie en fonction de ElementAt(i)
            List<string> fieldsCreationTicket = new List<string> { "Name", "Description", "Projet_Id", "State", "Auteur_Id" };
            List<string> valuesCreationTicket = new List<string> { textBox_ticket_name.Text, textBox_ticket_description.Text, Project.Id.ToString(), "4", auteur.Id.ToString()};

            int ticketId = DB.Insert(fieldsCreationTicket, valuesCreationTicket, "Tickets");
            Ticket ticket = Ticket.GetFromDb(ticketId);

            for (int i = 0; i < comboBox_assignee.SelectedItems.Count; i++)
            {
                List<string> valuesAssignees = new List<string>();

                string key = comboBox_assignee.SelectedItems.ElementAt(i).Key.ToString();
                int value = int.Parse(comboBox_assignee.SelectedItems.ElementAt(i).Value.ToString());

                User assignmentTarget = User.Get(value);

                valuesAssignees.Add(ticket.Id.ToString());
                valuesAssignees.Add(assignmentTarget.Id.ToString());
                DB.Insert(fieldsAssignees, valuesAssignees, "Ticket_assignee");
            }

            NavigationService.Navigate(new ProjectIssuesPage(Project.Id));
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProjectIssuesPage(Project.Id));
        }
    }
}
