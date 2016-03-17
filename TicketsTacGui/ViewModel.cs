using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TicketsTacGui
{
    public class ViewModel : ViewModelBase
    {

        private Dictionary<string, object> _items;
        private Dictionary<string, object> _selectedItems;


        public Dictionary<string, object> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                NotifyPropertyChanged("Items");
            }
        }

        public Dictionary<string, object> SelectedItems
        {
            get
            {
                return _selectedItems;
            }
            set
            {
                _selectedItems = value;
                NotifyPropertyChanged("SelectedItems");
            }
        }



        public ViewModel()
        {
            Items = new Dictionary<string, object>();
            List<Dictionary<String, String>> result = DB.Select("*", "Users");
            foreach (Dictionary<String, String> user in result)
            {

                Items.Add(user["Username"], (Object)user["Id"]);
            }

            SelectedItems = new Dictionary<string, object>();
        }

        private void Submit()
        {
            foreach (KeyValuePair<string, object> s in SelectedItems)
                MessageBox.Show(s.Key);
        }


    }


}
