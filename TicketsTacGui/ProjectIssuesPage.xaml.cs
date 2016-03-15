using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Logique d'interaction pour ProjectIssuesPage.xaml
    /// </summary>
    public partial class ProjectIssuesPage : Page
    {
        private Projet Project { get; set; }

        public ProjectIssuesPage(int id)
        {
            this.Project = Projet.GetProjetFromBDD(id);

            InitializeComponent();

            Task.Run(() =>
            {
                List<Ticket> tickets = this.Project.GetAllTickets();

                foreach (Ticket ticket in tickets)
                {
                    stackPanel_issues.Dispatcher.Invoke(() =>
                    {
                        Button ticketButton = new Button();
                        ticketButton.Content = ticket.ProblemDescription;

                        stackPanel_issues.Children.Add(ticketButton);
                    });
                }
            });
        }
    }
}
