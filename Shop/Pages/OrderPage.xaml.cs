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

            Order = new Order
            {
                Date = DateTime.Now
            };
            ProductOrders = Order.ProductOrders.ToList();

            Products = DataAccess.GetProducts();
            StatusOrders = DataAccess.GetStatusOrders();

            this.DataContext = this;
            
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
            SetEnable();
        }

        private void dgProducts_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (this.dgProducts.SelectedItem != null)
            {
                (sender as DataGrid).RowEditEnding -= dgProducts_RowEditEnding;
                //(gridProducts.SelectedItem as IntakeProduct).ProductId = (gridProducts.SelectedItem as IntakeProduct).Product.Id;
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
                Order = Order
            });

            dgProducts.Items.Refresh();

            Products.Remove(product);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Order.ProductOrders = ProductOrders;

            DataAccess.SaveOrder(Order);

            NavigationService.GoBack();
        }

        private void SetEnable()
        {
            if (Order.StatusOrder.Name == "Новый")
                return;
            grid.IsEnabled = false;
        }
    }
}
