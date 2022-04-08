using System;
using System.Collections.Generic;
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

namespace Shop.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ShopWindow : Window
    {
        public ShopWindow()
        {
            InitializeComponent();
            frame.Navigated += Frame_Navigated;
            frame.NavigationService.Navigate(new Pages.AuthorizationPage());
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            if (frame.Content is Pages.AuthorizationPage)
                spButtons.Visibility = Visibility.Hidden;
            else if (frame.Content is Pages.RegistrationPage)
            {
                spButtons.Visibility = Visibility.Visible;
                btnGoForward.Visibility = Visibility.Hidden;
            }
            else
            {
                spButtons.Visibility = Visibility.Visible;
                btnGoForward.Visibility = Visibility.Visible;
            }
        }

        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            if (frame.NavigationService.CanGoBack)
                frame.NavigationService.GoBack();
        }

        private void btnGoForward_Click(object sender, RoutedEventArgs e)
        {
            if (frame.NavigationService.CanGoForward)
                frame.NavigationService.GoForward();
        }
    }
}
