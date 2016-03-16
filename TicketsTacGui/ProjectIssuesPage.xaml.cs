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
                        ticketButton.Style = Resources["ListIssueButtons"] as Style;
                        ticketButton.Tag = ticket.Id;
                        ticketButton.Click += TicketButton_Click;

                        StackPanel stackPanelTicketHorizontal = new StackPanel();
                        stackPanelTicketHorizontal.Orientation = Orientation.Horizontal;

                        StackPanel stackPanelTicketButton = new StackPanel();

                        StackPanel stackPanelTicketStatus = new StackPanel();

                        TextBlock textBlockTicketButtonDescription = new TextBlock(new Run(ticket.ProblemDescription));
                        textBlockTicketButtonDescription.Margin = new Thickness(5, 5, 5, 0);
                        textBlockTicketButtonDescription.FontWeight = FontWeights.DemiBold;
                        textBlockTicketButtonDescription.FontSize = 15;

                        //TextBlock textBlockTicketButtonAuthor = new TextBlock(new Run(ticket.UserAssign.ElementAt(0).Username));
                        TextBlock textBlockTicketButtonAuthor = new TextBlock(new Run("Assigné à User assigned"));
                        textBlockTicketButtonAuthor.Margin = new Thickness(5, 0, 5, 5);
                        textBlockTicketButtonAuthor.Foreground = Brushes.Gray;

                        TextBlock textBlockTicketButtonStatus = new TextBlock(new Run(ticket.State.ToString()));
                        textBlockTicketButtonStatus.Margin = new Thickness(5, 8, 0, 0);
                        textBlockTicketButtonStatus.Foreground = Brushes.White;
                        switch (ticket.State)
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
                        }
                        textBlockTicketButtonStatus.Padding = new Thickness(5, 1, 5, 2);

                        stackPanelTicketStatus.Children.Add(textBlockTicketButtonStatus);

                        stackPanelTicketButton.Children.Add(textBlockTicketButtonDescription);
                        stackPanelTicketButton.Children.Add(textBlockTicketButtonAuthor);

                        //Border ticketButtonBorder = new Border();
                        //ticketButton.bo

                        stackPanelTicketHorizontal.Children.Add(stackPanelTicketButton);
                        stackPanelTicketHorizontal.Children.Add(stackPanelTicketStatus);

                        ticketButton.Content = stackPanelTicketHorizontal;

                        stackPanel_issues.Children.Add(ticketButton);
                    });
                }
            });
        }

        private void TicketButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ViewTicketPage(int.Parse(((Button)sender).Tag.ToString())));
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NewTicketPage(Project.Id));
        }
    }
}
