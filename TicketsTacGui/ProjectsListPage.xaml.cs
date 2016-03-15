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

            List<Projet> projets = Projet.GetAllProjetsFromBDD();

            int numberProjets = 0;
            foreach (Projet projet in projets)
            {
                Button buttonProjet = new Button();
                buttonProjet.Height = 50;
                buttonProjet.Margin = new Thickness(0, 50 * numberProjets, 0, 0);
                buttonProjet.Content = projet.Nom;

                Console.WriteLine("Nouveau projet: " + projet.Nom);

                stackPanel_projects.Children.Add(buttonProjet);

                numberProjets++;
            }

            Console.WriteLine(projets);
        }
    }
}
