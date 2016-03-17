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
    /// Interaction logic for ClientsList.xaml
    /// </summary>
    public partial class ClientsList : Page
    {
        public ClientsList()
        {
            InitializeComponent();

            if (User.currentUser.Rank == Rank.Client)
                buttonNewClient.Visibility = Visibility.Collapsed;

            buttonNewClient.Style = Resources["NewProjectButton"] as Style;

            List<Dictionary<string, string>> clients = DB.Select("*", "Users");

            int numberProjets = 0;
            foreach (Dictionary<string, string> client in clients)
            {
                if (!User.currentUser.hasPermissionTo(Permission.userView, User.Get(int.Parse(client["Id"].ToString()))) && User.currentUser.Id != int.Parse(client["Id"]) )
                    continue;
                Button buttonProjet = new Button();
                buttonProjet.Content = client["Username"];
                buttonProjet.Style = Resources["ListProjectButtons"] as Style;
                buttonProjet.Click += buttonClient_Click;
                buttonProjet.Tag = client["Id"];

                Console.WriteLine("Nouveau client: " + client["Username"]);

                stackPanel_projects.Children.Add(buttonProjet);

                numberProjets++;
            }
        }

        private void buttonNewClient_Click(object sender, RoutedEventArgs e)
        {
            frame_client.Navigate(new CreateClientPage());
        }

        private void buttonClient_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(((Button)sender).Tag.ToString());
            frame_client.Navigate(new ClientProfilePage(id));
        }
    }
}
