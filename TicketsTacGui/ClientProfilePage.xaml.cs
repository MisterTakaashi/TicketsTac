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
    /// Interaction logic for ClientProfilePage.xaml
    /// </summary>
    public partial class ClientProfilePage : Page
    {
        User profileOwner = User.currentUser;

        public ClientProfilePage()
        {
            InitializeComponent();


            textBoxEmail.Text = profileOwner.Email;
            textBoxPassword.Text = "*********";
        }

        private void buttonSubmit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
