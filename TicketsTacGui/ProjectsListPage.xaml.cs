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
    /// Logique d'interaction pour ProjectsListPage.xaml
    /// </summary>
    public partial class ProjectsListPage : Page
    {
        public ProjectsListPage()
        {
            InitializeComponent();

            if (!User.currentUser.hasPermissionTo(Permission.projectCreate, null))
                buttonNewProject.Visibility = Visibility.Collapsed;

            buttonNewProject.Style = Resources["NewProjectButton"] as Style;

            List<Projet> projets = Projet.GetAllProjetsFromBDD();

            int numberProjets = 0;
            foreach (Projet projet in projets)
            {
                Button buttonProjet = new Button();
                buttonProjet.Content = projet.Nom;
                buttonProjet.Style = Resources["ListProjectButtons"] as Style;
                buttonProjet.Click += buttonProjet_Click;
                buttonProjet.Tag = projet.Id;

                Console.WriteLine("Nouveau projet: " + projet.Nom);

                stackPanel_projects.Children.Add(buttonProjet);

                numberProjets++;
            }

            Console.WriteLine(projets);
        }

        private void buttonProjet_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = sender as Button;
            int id = (int)senderButton.Tag;

            Console.WriteLine("Demande Projet N°" + id);

            frame_project.Navigate(new ProjectIssuesPage(id));
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NewProjectPage());
        }

        private void buttonProfile_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ClientsList());
        }
    }
}
