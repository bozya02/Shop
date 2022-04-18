using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Core;

namespace Shop.Pages
{
    /// <summary>
    /// Interaction logic for OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        public List<Order> Orders { get; set; }
        public OrdersPage()
        {
            InitializeComponent();
            Orders = DataAccess.GetOrders(App.User);
            this.DataContext = this;

            DataAccess.NewItemAddedEvent += DataAccess_NewItemAddedEvent;
        }

        private void DataAccess_NewItemAddedEvent()
        {
            Orders = DataAccess.GetOrders(App.User);

            dgOrders.ItemsSource = Orders;
            dgOrders.Items.Refresh();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            var order = dgOrders.SelectedItem as Order;

            if (!IsOrderelect(order))
                return;

            NavigationService.Navigate(new OrderPage(order));
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrderPage());
        }

        private bool IsOrderelect(Order order)
        {
            if (order == null)
            {
                MessageBox.Show($"Заказ не выбран", "Ошибка");
                return false;
            }

            return true;
        }
    }
}
