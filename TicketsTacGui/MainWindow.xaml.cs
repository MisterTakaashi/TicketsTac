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

            frameContent.Navigate(new LoginPage());

            //frameContent.Source = new Uri("LoginPage.xaml", UriKind.Relative);
            

            //frameContent.Navigate(new DbConnectPage());
        }
    }
}
