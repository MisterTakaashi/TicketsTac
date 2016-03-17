using System.Windows;
using System.Windows.Controls;

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
            //DBConfig conf = new DBConfig(textbox_host.Text, textBox_username.Text, textbox_password.Text);
            //DB.testConnection(conf);
        }
    }
}
