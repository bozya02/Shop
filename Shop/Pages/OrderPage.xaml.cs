using Core;
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

namespace Shop.Pages
{
    /// <summary>
    /// Interaction logic for OrderPage.xaml
    /// </summary>
    public partial class OrderPage : Page
    {
        public ObservableCollection<Product> Products { get; set; }
        public Order Order { get; set; }
        public ObservableCollection<StatusOrder> StatusOrders { get; set; }
        public List<ProductOrder> ProductOrders { get; set; }
        public OrderPage()
        {
            InitializeComponent();

            StatusOrders = DataAccess.GetStatusOrders();
            
            Order = new Order
            {
                Date = DateTime.Now,
                StatusOrderId = 1,
                StatusOrder = DataAccess.GetStatusOrder(1)
            };

            ProductOrders = Order.ProductOrders.ToList();
            cbStatusOrder.SelectedItem = Order.StatusOrder;

            Products = DataAccess.GetProducts();

            this.DataContext = this;
            SetEnable();
            btnPay.Visibility = Visibility.Hidden;
            
        }

        public OrderPage(Order order)
        {
            InitializeComponent();

            Order = order;
            ProductOrders = Order.ProductOrders.ToList();

            Products = DataAccess.GetProducts();
            StatusOrders = DataAccess.GetStatusOrders();
            cbStatusOrder.SelectedItem = Order.StatusOrder;

            this.DataContext = this;
            tbSum.Text = ProductOrders.Sum(o => o.Sum).ToString();
            btnPay.Visibility = Visibility.Hidden;
            SetEnable();
        }

        private void dgProducts_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (this.dgProducts.SelectedItem != null)
            {
                (sender as DataGrid).RowEditEnding -= dgProducts_RowEditEnding;
                (sender as DataGrid).CommitEdit();
                (sender as DataGrid).Items.Refresh();

                decimal sum = 0;
                foreach (ProductOrder product in dgProducts.ItemsSource)
                {
                    sum += product.Sum;
                }

                tbSum.Text = sum.ToString();
                (sender as DataGrid).RowEditEnding += dgProducts_RowEditEnding;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var product = cbProducts.SelectedItem as Product;

            ProductOrders.Add(new ProductOrder
            {
                Product = product,
                ProductId = product.Id
            });

            dgProducts.Items.Refresh();

            Products.Remove(product);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Order.ProductOrders = ProductOrders;

            if (App.User.RoleId == 3)
            {
                Order.Client = DataAccess.GetClient(App.User);
            }
            else
            {
                Order.Worker = DataAccess.GetWorker(App.User);
            }
            Order.StatusOrder = cbStatusOrder.SelectedItem as StatusOrder;

            DataAccess.SaveOrder(Order);

            NavigationService.GoBack();
        }

        private void SetEnable()
        {
            if (Order.StatusOrder.Name == "Отклонен" || Order.StatusOrder.Name == "Готов")
            {
                grid.IsEnabled = false;
                return;
            }
            else if (Order.StatusOrder.Name != "Новый")
            {
                tbDate.IsEnabled = false;
                cbProducts.IsEnabled = false;
                dgProducts.IsEnabled = false;
                btnAdd.IsEnabled = false;
            }

            if (App.User.RoleId == 3)
            {
                cbStatusOrder.IsEnabled = false;
                if (Order.StatusOrder == DataAccess.GetStatusOrders().FirstOrDefault(s => s.Name == "К оплате"))
                {
                    btnPay.Visibility = Visibility.Visible;
                }
            }
        }

        private void btnPay_Click(object sender, RoutedEventArgs e)
        {
            Order.StatusOrder = DataAccess.GetStatusOrders().FirstOrDefault(s => s.Name == "Оплачен");
            btnPay.Visibility = Visibility.Hidden;
        }
    }
}
