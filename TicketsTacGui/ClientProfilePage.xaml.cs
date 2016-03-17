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
        User profileOwner;

        public ClientProfilePage()
        {
            profileOwner = User.currentUser;
            InitializeComponent();

            //Le rang !!!!!
            textBoxEmail.Text = profileOwner.Email;
            textBoxPassword.Text = "*********";
        }

        public ClientProfilePage(int userId)
        {
            if ( !User.currentUser.hasPermissionTo(Permission.userUpdate, null) )
            {
                textBoxEmail.IsReadOnly = true;
            }

            profileOwner = User.Get(userId);
            InitializeComponent();

            textBoxEmail.Text = profileOwner.Email;
            textBoxPassword.Text = "*********";
        }

        private void buttonSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (profileOwner.Email != textBoxPassword.Text)
            {
                profileOwner.setEmail(textBoxEmail.Text);
            }
            string passwdText = textBoxPassword.Text;
            bool passwdChanged = false;

            for ( int i = 0; i < passwdText.Length; i++ )
            {
                if ( !passwdText.ElementAt(i).ToString().Equals("*") )
                {
                    passwdChanged = true;
                }
            }

            if (passwdChanged) profileOwner.setPassword(passwdText);
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}