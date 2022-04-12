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
        public List<Product> ProductsForSearch { get; set; }

        private Dictionary<string, Func<Product, object>> Sortings;

        private int startIndex;

        public ProductsPage(int roleId)
        {
            InitializeComponent();
            Products = DataAccess.GetProducts().ToList();
            Units = DataAccess.GetUnits();
            Units.Insert(0, new Unit { Name = "Все" });
            
            cbCountPerPage.SelectedIndex = 0;
            cbMonth.SelectedIndex = 0;
            cbSort.SelectedIndex = 0;
            cbUnits.SelectedIndex = 0;

            startIndex = 0;

            Sortings = new Dictionary<string, Func<Product, object>>
            {
                { "А-Я", x => x.Name},
                { "Я-А", x => x.Name},
                { "Сначала старые", x => x.AddDate},
                { "Сначала новые", x => x.AddDate},
                { "По умолчанию", x => x.Id}
            };

            this.DataContext = this;
            CheckRole(roleId);
        }
        private void Apply()
        {
            ProductsForSearch = Products.ToList();
            if (cbMonth.SelectedItem != null && cbUnits.SelectedItem != null)
            {
                var unit = cbUnits.SelectedItem as Unit;
                if (unit.Name != "Все")
                    ProductsForSearch = Products.Where(p => p.UnitId == unit.Id).ToList();

                var text = tbSearch.Text;
                ProductsForSearch = ProductsForSearch.Where(p => p.Name.ToLower().Contains(text.ToLower()) || p.Description.ToLower().Contains(text.ToLower())).ToList();

                if ((cbMonth.SelectedItem as ComboBoxItem).Content.ToString() != "Все")
                    ProductsForSearch = ProductsForSearch.Where(p => p.AddDate.Month == DateTime.Now.Month).ToList();
                if (cbSort.SelectedItem != null)
                {
                    var sort = (cbSort.SelectedItem as ComboBoxItem).Content.ToString();

                    ProductsForSearch = ProductsForSearch.OrderBy(Sortings[sort]).ToList();
                    if (sort == "Я-А" || sort == "Сначала новые")
                        ProductsForSearch.Reverse();
                }

                dgProducts.ItemsSource = ProductsForSearch;
            }
            Pagination();
        }

        private void cbUnits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Apply();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Apply();
        }

        private void cbMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Apply();
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
            Products = DataAccess.GetProducts().ToList();
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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductPage());
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Apply();
        }

        private void cbCountPerPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            startIndex = 0;

            Apply();
            Pagination();
        }

        private void Pagination()
        {
            var count = (cbCountPerPage.SelectedItem as ComboBoxItem).Content.ToString() == "Все" ? ProductsForSearch.Count : int.Parse((cbCountPerPage.SelectedItem as ComboBoxItem).Content.ToString());
            dgProducts.ItemsSource = ProductsForSearch.Skip(count * startIndex).Take(count);
            tbCount.Text = $"{dgProducts.ItemsSource.Cast<Product>().Count()} из {ProductsForSearch.Count()}";
            IsSearchNotNull(ProductsForSearch);
        }

        private void btnPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            if (startIndex != 0)
                startIndex--;
            Pagination();
        }

        private void btnNextPage_Click(object sender, RoutedEventArgs e)
        {
            if ((startIndex + 1) * Convert.ToInt32((cbCountPerPage.SelectedItem as ComboBoxItem).Content.ToString()) < ProductsForSearch.Count)
                startIndex++;
            Pagination();
        }

        private void CheckRole(int roleId)
        {
            spButtons.Visibility = DataAccess.GetRole(roleId).Name == "Клиент" ? Visibility.Hidden : Visibility.Visible;
        }

        private void btnIntakes_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductIntakesPage());
        }
    }
}
