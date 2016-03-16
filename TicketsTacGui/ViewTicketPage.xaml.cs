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
    /// Interaction logic for ViewTicketPage.xaml
    /// </summary>
    public partial class ViewTicketPage : Page
    {
        Ticket Ticket = null;

        public ViewTicketPage(int id)
        {
            InitializeComponent();

            Ticket = Ticket.GetFromDb(id);
            labelProjectTitle.Content = Ticket.Name;
            labelTicketText.Content = Ticket.ProblemDescription;
            
            foreach ( Commentaire commentaire in Ticket.AdditionnalNote )
            {
                TextBlock reply = new TextBlock();
                reply.Background = Brushes.AntiqueWhite;

                replies.Children.Add(reply);
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProjectIssuesPage(Ticket.Project.Id));
        }

        private void buttonSubmit_Click(object sender, RoutedEventArgs e)
        {
            List<string> fields = new List<string>();
            List<string> values = new List<string>();

            fields.Add("Ticket_Id");
            fields.Add("Message");
            fields.Add("Created");

            values.Add(Ticket.Id.ToString());
            values.Add(textBox.Text);
            values.Add(DB.getTimestamp().ToString());

            DB.Insert(fields, values, "Ticket_comms");
            NavigationService.Navigate(new ProjectIssuesPage(Ticket.Project.Id));
        }
    }
}
