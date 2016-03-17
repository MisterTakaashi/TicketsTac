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

            foreach ( Rank value in Enum.GetValues(typeof(Rank)) )
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Tag = (int)value;
                item.Content = value.ToString();

                comboBoxRank.Items.Add(item);
            }

            textBlockClientName.Content = profileOwner.Username;

            textBoxEmail.Text = profileOwner.Email;
            textBoxPassword.Text = "*********";
        }

        public ClientProfilePage(int userId)
        {
            InitializeComponent();
            if ( !User.currentUser.hasPermissionTo(Permission.userUpdate, null) )
            {
                textBoxEmail.IsReadOnly = true;
                comboBoxRank.IsReadOnly = true;
                textBoxPassword.IsReadOnly = true;
            }

            profileOwner = User.Get(userId);

            foreach (Rank value in Enum.GetValues(typeof(Rank)))
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Tag = (int)value;
                item.Content = value.ToString();
                comboBoxRank.Items.Add(item);
                if ( value == profileOwner.Rank ) comboBoxRank.SelectedItem = item;
            }



            textBlockClientName.Content = profileOwner.Username;
            
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
            profileOwner.setRank((Rank)int.Parse(((ComboBoxItem)comboBoxRank.SelectedItem).Tag.ToString()));
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}