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

            dgProducts.ItemsSource = new ObservableCollection<ProductIntakeProduct>();
            this.DataContext = this;
        }

        public ProductIntakePage(ProductIntake productIntake)
        {
            InitializeComponent();
            ProductIntake = productIntake;

            Products = DataAccess.GetProducts();
            cbProducts.ItemsSource = Products;
            
            Suppliers = DataAccess.GetSuppliers();
            cbSupplier.ItemsSource = Suppliers;
            cbSupplier.SelectedItem = ProductIntake.Supplier;

            StatusIntakes = DataAccess.GetStatusIntakes();
            cbStatusIntake.ItemsSource = StatusIntakes;
            cbStatusIntake.SelectedItem = ProductIntake.StatusIntake;

            this.DataContext = this;
        }
    }
}
