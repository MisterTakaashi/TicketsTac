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
using TicketsTac;

namespace TicketsTacGui
{
    /// <summary>
    /// Interaction logic for DbConnectPage.xaml
    /// </summary>
    public partial class DbConnectPage : Page
    {
        public DbConnectPage()
        {
            InitializeComponent();
        }

        private void button_connect_Click(object sender, RoutedEventArgs e)
        {
            DBConfig conf = new DBConfig(textbox_host.Text, textBox_username.Text, textbox_password.Text);
        }
    }
}
