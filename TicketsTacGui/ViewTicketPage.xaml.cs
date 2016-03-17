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
    /// Interaction logic for ViewTicketPage.xaml
    /// </summary>
    public partial class ViewTicketPage : Page
    {
        Ticket Ticket = null;

        public ViewTicketPage(int id)
        {
            InitializeComponent();

            Task.Run(() =>
            {
                Ticket = Ticket.GetFromDb(id);

                replies.Dispatcher.Invoke(() =>
                {
                    if (!User.currentUser.hasPermissionTo(Permission.ticketValidate, Ticket))
                        buttonValidateTicket.Visibility = Visibility.Hidden;

                    labelProjectTitle.Content = Ticket.Name;
                    //labelTicketText.Content = Ticket.ProblemDescription;

                    textBlockDecriptionAuthor.Text = Ticket.Auteur.Username.ToUpper() + "    ";
                    // TODO : Date création du ticket
                    textBlockDecriptionDate.Text = "    " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm");
                    textBlockDescriptionMessage.Text = Ticket.ProblemDescription;

                    foreach (Commentaire commentaire in Ticket.AdditionnalNotes)
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
                        textBlockDate.Text = "    " + dateCreate.ToString("yyyy-MM-dd HH:mm");

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
                });
            });
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


        /*switch (ticket.State)
                        {
                            case StateEnum.Open:
                                textBlockTicketButtonStatus.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF28b62c"));
                                break;
                            case StateEnum.Commented:
                                textBlockTicketButtonStatus.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF148cba"));
                                break;
                            case StateEnum.Resolve:
                                textBlockTicketButtonStatus.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFff851b"));
                                break;
                            case StateEnum.Validate:
                                textBlockTicketButtonStatus.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF75caeb"));
                                break;
                            case StateEnum.Closed:
                                textBlockTicketButtonStatus.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFff4136"));
                                break;
                            default:
                                textBlockTicketButtonStatus.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF28b62c"));
                                break;
                        }*/


        private void buttonStatusOpen_Click(object sender, RoutedEventArgs e)
        {
            this.Ticket.State = StateEnum.Open;
            this.changeBackgroundStatus();
            this.Ticket.ChangeState(StateEnum.Open);
        }

        private void buttonStatusResolve_Click(object sender, RoutedEventArgs e)
        {
            this.Ticket.State = StateEnum.Resolve;
            this.changeBackgroundStatus();
            this.Ticket.ChangeState(StateEnum.Open);
        }

        private void buttonStatusClose_Click(object sender, RoutedEventArgs e)
        {
            this.Ticket.State = StateEnum.Closed;
            this.changeBackgroundStatus();
            this.Ticket.ChangeState(StateEnum.Closed);
        }

        private void changeBackgroundStatus()
        {
            switch (Ticket.State)
            {
                case StateEnum.Open:
                    this.textBlockStatus.Text = "Open";
                    backgroundStatus.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF28b62c"));
                    break;
                case StateEnum.Commented:
                    this.textBlockStatus.Text = "Commented";
                    backgroundStatus.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF148cba"));
                    break;
                case StateEnum.Resolve:
                    this.textBlockStatus.Text = "Resolved";
                    backgroundStatus.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFff851b"));
                    break;
                case StateEnum.Validate:
                    this.textBlockStatus.Text = "Validated";
                    backgroundStatus.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF75caeb"));
                    break;
                case StateEnum.Closed:
                    this.textBlockStatus.Text = "Closed";
                    backgroundStatus.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFff4136"));
                    break;
                default:
                    this.textBlockStatus.Text = "Open";
                    backgroundStatus.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF28b62c"));
                    break;
            }
        }
    }
}
