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
        public List<Product> Products { get; set; }
        public ProductsPage()
        {
            InitializeComponent();
            Products = DataAccess.GetProducts().ToList();
            Units = DataAccess.GetUnits();
            Units.Add(new Unit { Name = "Все"});
            cbMonth.SelectedIndex = 0;
            cbUnits.SelectedIndex = Units.Count() - 1;
            this.DataContext = this;
        }
        private void Apply()
        {
            if (cbMonth.SelectedItem != null && cbUnits.SelectedItem != null)
            {
                var unit = cbUnits.SelectedItem as Unit;
                if (unit.Name == "Все")
                    Products = DataAccess.GetProducts().ToList();
                else
                    Products = DataAccess.GetProducts().Where(p => p.UnitId == unit.Id).ToList();

                var text = tbSearch.Text;
                Products = Products.Where(p => p.Name.ToLower().Contains(text.ToLower()) || p.Description.ToLower().Contains(text.ToLower())).ToList();

                if ((cbMonth.SelectedItem as ComboBoxItem).Content.ToString() != "Все")
                {
                    Products = Products.Where(p => p.AddDate.Month == DateTime.Now.Month).ToList();
                }

                dgProducts.ItemsSource = Products;
                IsSearchNotNull(Products);
            }
        }

        private void cbUnits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Apply();
            //var unit = cbUnits.SelectedItem as Unit;
            //if (unit.Name == "Все")
            //    ProductsForSearch = Products.ToList();
            //else
            //    ProductsForSearch = Products.Where(p => p.UnitId == unit.Id).ToList();

            //dgProducts.ItemsSource = ProductsForSearch;
            //IsSearchNotNull(ProductsForSearch);
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Apply();
            //var text = tbSearch.Text;
            //var search = ProductsForSearch.Where(p => p.Name.ToLower().Contains(text.ToLower()) || p.Description.ToLower().Contains(text.ToLower())).ToList();
            //ProductsForSearch = search;
            

            //dgProducts.ItemsSource = ProductsForSearch;
            //IsSearchNotNull(ProductsForSearch);
        }

        private void cbMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Apply();
            //if ((cbMonth.SelectedItem as ComboBoxItem).Content.ToString() == "Все")
            //{
            //    ProductsForSearch = Products.ToList();
            //}
            //else
            //{
            //    var search = ProductsForSearch.Where(p => p.AddDate.Month == DateTime.Now.Month).ToList();
            //    ProductsForSearch = search;
            //}
            
            //dgProducts.ItemsSource = ProductsForSearch;
            //IsSearchNotNull(ProductsForSearch);
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

            Apply();
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

        private bool IsProductSelect(Product product)
        {
            if (product == null)
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

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var product = dgProducts.SelectedItem as Product;
            if (!IsProductSelect(product))
                return;
            NavigationService.Navigate(new ProductPage(product));
        }
    }
}
