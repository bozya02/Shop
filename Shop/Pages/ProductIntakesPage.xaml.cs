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
        }
    }
}
