using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Logique d'interaction pour LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();

            buttonConnexion.Click += buttonConnexion_Click;
        }

        private void buttonConnexion_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Connexion de " + textBoxUsername.Text + " Pass: " + passwordBoxPassword.Password);

            User user = User.Connect(textBoxUsername.Text, passwordBoxPassword.Password.ToSHA1());

            if (user == null)
            {
                MessageBox.Show("Email or Password is wrong");
            }
            else
            {
                User.currentUser = user;

                MainWindow main = (MainWindow)Application.Current.MainWindow;
                //main.frameContent.Navigate(new ProjectsListPage());
                main.frameContent.Navigate(new NewProjectPage());

            }
        }
    }
}
