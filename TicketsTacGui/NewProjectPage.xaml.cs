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
    /// Interaction logic for NewProjectPage.xaml
    /// </summary>
    public partial class NewProjectPage : Page
    {
        public NewProjectPage()
        {
            InitializeComponent();
            List<User> users = User.GetAll();
            foreach (User user in users)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Tag = user.Id;
                item.Content = user.Username;

                comboBox_assignee.Items.Add(item);
            }
        }

        private void buttonCreateProject_Click(object sender, RoutedEventArgs e)
        {
            User u = new User(DB.Get(int.Parse(((ComboBoxItem)comboBox_assignee.SelectedItem).Tag.ToString()), "Users"));
            DB.Insert(new Projet(textBox_title.Text, u, User.currentUser), "Projets");
        }
    }
}
