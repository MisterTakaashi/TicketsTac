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
            //labelTicketText.Content = Ticket.ProblemDescription;

            //.Text = Ticket.Auteur.Username;
            // TODO : Date création du ticket
            //textBlockDecriptionDate.Text = 
            textBlockDecriptionMessage.Text = Ticket.ProblemDescription;


            foreach ( Commentaire commentaire in Ticket.AdditionnalNotes )
            {
                /*TextBlock reply = new TextBlock();
                reply.Background = Brushes.AntiqueWhite;*/

                Border borderReply = new Border();
                borderReply.Background = Brushes.White;
                borderReply.BorderThickness = new Thickness(20, 20, 20, 0);
                borderReply.CornerRadius = new CornerRadius(3);
                borderReply.Padding = new Thickness(18, 13, 18, 13);

                StackPanel stackPanelFirst = new StackPanel();

                StackPanel stackPanelHori = new StackPanel();
                stackPanelHori.Orientation = Orientation.Horizontal;

                TextBlock textBlockAuthor = new TextBlock();
                textBlockAuthor.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF787878"));
                textBlockAuthor.FontWeight = FontWeights.DemiBold;
                textBlockAuthor.Text = commentaire.Creator.Username.ToUpper() + "    ";

                TextBlock textBlockDate = new TextBlock();
                textBlockDate.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF787878"));
                DateTime dateCreate = DB.UnixTimeStampToDateTime(commentaire.Created);
                textBlockDate.Text = "    " + dateCreate.Day + "/" + dateCreate.Month.ToString() + "/" + dateCreate.Year.ToString() + " " + dateCreate.Hour.ToString() + ":" + dateCreate.Minute.ToString();

                TextBlock textBlockMessage = new TextBlock();
                textBlockMessage.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5b6870"));
                textBlockMessage.FontSize = 16;
                textBlockMessage.FontWeight = FontWeights.DemiBold;
                textBlockMessage.Text = commentaire.Message;

                stackPanelHori.Children.Add(textBlockAuthor);
                stackPanelHori.Children.Add(textBlockDate);

                stackPanelFirst.Children.Add(stackPanelHori);
                stackPanelFirst.Children.Add(textBlockMessage);

                borderReply.Child = stackPanelFirst;

                replies.Children.Add(borderReply);
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProjectIssuesPage(Ticket.Project.Id));
        }

        private void buttonSubmit_Click(object sender, RoutedEventArgs e)
        {
            List<string> fields = new List<string> { "Ticket_Id", "Message", "Created", "Creator_Id" };
            List<string> values = new List<string> { Ticket.Id.ToString(), textBox.Text, DB.getTimestamp().ToString(), User.currentUser.Id.ToString()};

            DB.Insert(fields, values, "Ticket_comms");
            NavigationService.Navigate(new ProjectIssuesPage(Ticket.Project.Id));
        }
    }
}
