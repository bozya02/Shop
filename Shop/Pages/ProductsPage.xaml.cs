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
    /// Interaction logic for ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        public ObservableCollection<Unit> Units { get; set; }
        public ObservableCollection<Product> Products { get; set; }
        public List<Product> ProductsForSearch { get; set; }
        public ProductsPage()
        {
            InitializeComponent();
            Products = DataAccess.GetProducts();
            ProductsForSearch = Products.ToList();
            Units = DataAccess.GetUnits();
            Units.Add(new Unit { Name = "Все"});
            this.DataContext = this;
        }

        private void cbUnits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var unit = cbUnits.SelectedItem as Unit;
            if (unit.Name == "Все")
                ProductsForSearch = Products.ToList();
            else
                ProductsForSearch = Products.Where(p => p.UnitId == unit.Id).ToList();

            dgProducts.ItemsSource = ProductsForSearch;
            IsSearchNotNull(ProductsForSearch);
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = tbSearch.Text;
            var search = ProductsForSearch.Where(p => p.Name.ToLower().Contains(text.ToLower()) || p.Description.ToLower().Contains(text.ToLower())).ToList();
            
            dgProducts.ItemsSource = search;
            IsSearchNotNull(search);
        }

        private void cbMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cbMonth.SelectedItem as ComboBoxItem).Content.ToString() == "Все")
            {
                ProductsForSearch = Products.ToList();
            }
            else
            {
                ProductsForSearch = Products.Where(p => p.AddDate.Month == DateTime.Now.Month).ToList();
            }
            
            
            dgProducts.ItemsSource = ProductsForSearch;
            IsSearchNotNull(ProductsForSearch);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var products = dgProducts.SelectedItems.Cast<Product>().ToList();
            if (!IsProductsSelect(products))
                return;

            foreach (var product in products)
            {
                var result = MessageBox.Show($"Вы точно хотите удалить {product.Name}?", "Предупреждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                    DataAccess.DeleteProduct(product);
            }
        }

        private bool IsProductsSelect(List<Product> products)
        {
            if (products.Count() == 0)
            {
                MessageBox.Show($"Продукт не выбран", "Ошибка");
                return false;
            }

            return true;
        }

        private bool IsSearchNotNull(List<Product> products)
        {
            if (products.Count() == 0)
            {
                MessageBox.Show("Ничего не найдено", "Предупреждение");
                return false;
            }

            return true;
        }
    }
}
