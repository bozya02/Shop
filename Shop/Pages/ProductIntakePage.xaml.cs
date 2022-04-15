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
        public List<ProductIntakeProduct> ProductIntakeProducts { get; set; }
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

            ProductIntakeProducts = ProductIntake.ProductIntakeProducts.ToList();

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

            ProductIntakeProducts = ProductIntake.ProductIntakeProducts.ToList();

            SetEnable();
            tbSum.Text = ProductIntake.TotalAmount.ToString();
            this.DataContext = this;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var product = cbProducts.SelectedItem as Product;

            ProductIntakeProducts.Add(new ProductIntakeProduct {
                Product = product,
                PriceUnit = product.Price
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

        private void dgProducts_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (this.dgProducts.SelectedItem != null)
            {
                (sender as DataGrid).RowEditEnding -= dgProducts_RowEditEnding;
                //(gridProducts.SelectedItem as IntakeProduct).ProductId = (gridProducts.SelectedItem as IntakeProduct).Product.Id;
                (sender as DataGrid).CommitEdit();
                (sender as DataGrid).Items.Refresh();

                decimal sum = 0;
                foreach (ProductIntakeProduct product in dgProducts.ItemsSource)
                {
                    sum += product.Sum;
                }

                tbSum.Text = sum.ToString();
                (sender as DataGrid).RowEditEnding += dgProducts_RowEditEnding;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ProductIntake.ProductIntakeProducts = ProductIntakeProducts;

            DataAccess.SaveProductIntake(ProductIntake);

            NavigationService.GoBack();
        }
    }
}
