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
    /// Interaction logic for ProductIntakePage.xaml
    /// </summary>
    public partial class ProductIntakePage : Page
    {
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Supplier> Suppliers { get; set; }
        public ObservableCollection<StatusIntake> StatusIntakes { get; set; }
        public ProductIntake ProductIntake { get; set; }
        public ProductIntakePage()
        {
            InitializeComponent();

            Products = DataAccess.GetProducts();
            Suppliers = DataAccess.GetSuppliers();
            StatusIntakes = DataAccess.GetStatusIntakes();
            ProductIntake = new ProductIntake
            {
                Data = DateTime.Now
            };
            cbStatusIntake.IsEnabled = false;
            this.DataContext = this;
        }

        public ProductIntakePage(ProductIntake productIntake)
        {
            InitializeComponent();
            ProductIntake = productIntake;

            Products = DataAccess.GetProducts();            
            Suppliers = DataAccess.GetSuppliers();

            cbSupplier.SelectedItem = ProductIntake.Supplier;

            StatusIntakes = DataAccess.GetStatusIntakes();

            cbStatusIntake.SelectedItem = ProductIntake.StatusIntake;
            SetEnable();
            this.DataContext = this;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var product = cbProducts.SelectedItem as Product;

            ProductIntake.ProductIntakeProducts.Add(new ProductIntakeProduct {
                Product = product
            });
            dgProducts.Items.Refresh();

            Products.Remove(product);
        }

        private void SetEnable()
        {
            if (ProductIntake.StatusIntake.Name == "Принят")
            {
                grid.IsEnabled = false;
                return;
            }
            cbSupplier.IsEnabled = false;
            cbProducts.IsEnabled = false;
            btnAdd.IsEnabled = false;
            dgProducts.IsEnabled = false;
        }
    }
}
