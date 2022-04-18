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
    /// Interaction logic for ProductIntakesPage.xaml
    /// </summary>
    public partial class ProductIntakesPage : Page
    {
        public ObservableCollection<ProductIntake> ProductIntakes { get; set; }
        public ProductIntakesPage()
        {
            InitializeComponent();
            ProductIntakes = DataAccess.GetProductIntakes();
            this.DataContext = this;
            CheckRole(App.User.RoleId);

            DataAccess.NewItemAddedEvent += DataAccess_NewItemAddedEvent;
        }

        private void DataAccess_NewItemAddedEvent()
        {
            ProductIntakes = DataAccess.GetProductIntakes();

            dgProductIntakes.ItemsSource = ProductIntakes;
            dgProductIntakes.Items.Refresh();
        }

        private void btnAddIntake_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProductIntakePage());
        }

        private void btnEditIntake_Click(object sender, RoutedEventArgs e)
        {
            var productIntake = dgProductIntakes.SelectedItem as ProductIntake;
            if (!IsProductIntakeSelect(productIntake))
                return;
            NavigationService.Navigate(new ProductIntakePage(productIntake));
        }

        private bool IsProductIntakeSelect(ProductIntake productIntake)
        {
            if (productIntake == null)
            {
                MessageBox.Show($"Поставка не выбрана", "Ошибка");
                return false;
            }

            return true;
        }

        private void CheckRole(int roleId)
        {
            if (DataAccess.GetRole(roleId).Name == "Кладовщик")
            {
                btnAddIntake.Visibility = Visibility.Visible;
                btnEditIntake.Visibility = Visibility.Hidden;
            }
            else
            {
                btnAddIntake.Visibility = Visibility.Hidden;
                btnEditIntake.Visibility = Visibility.Visible;
            }
        }

    }
}
