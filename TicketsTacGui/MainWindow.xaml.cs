using System.Windows;
using System.Windows.Input;

namespace TicketsTacGui
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            frameContent.Navigate(new ProjectsListPage());
            //frameContent.Navigate(new DbConnectPage()); //Fenêtre sur laquelle Pierre rentre les identifiants de la bdd pour tester la connexion

            //frameContent.Navigate(new NewTicketPage(1));
        }
    }
}
