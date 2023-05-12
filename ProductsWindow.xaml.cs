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
using System.Windows.Shapes;

namespace SportShop
{
    /// <summary>
    /// Логика взаимодействия для ProductsWindow.xaml
    /// </summary>
    public partial class ProductsWindow : Window
    {
        private SportShopEntities db;

        private User _user;

        private Order Order;

        private List<OrderProduct> OrderProducts;

        public ProductsWindow(User user)
        {
            InitializeComponent();
            db = new SportShopEntities();
            _user = user;

            ListProducts.ItemsSource = db.Product.ToList();

            ComboBoxFilterProductDiscountAmount.ItemsSource = new List<string>
            {
                "0-10%", "10-15%", "15-∞%", "All ranges"
            };

            OrderByFilter.ItemsSource = new List<string>
            {
                "Сброс", "По возрастанию", "По убыванию"
            };
            
            if (_user != null)
                UserFio.Content = _user.UserSurname + " " + _user.UserName + " " + _user.UserName;
            else
                UserFio.Content = "Неавторизированный пользователь";

            CounterList.Content = $"Показано записей { ListProducts.Items.Count } из { ListProducts.Items.Count }";

            CreateOrderButton.Visibility = Visibility.Hidden;
        }

        private void ButtonExit_OnClick(object sender, RoutedEventArgs e)
        {
            Hide();
            new Authorization().Show();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            Close();
        }

        private void ComboBoxFilterProductDiscountAmount_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ComboBoxFilterProductDiscountAmount.SelectedIndex)
            {
                case 0:
                    {
                        ListProducts.ItemsSource = db.Product.Where(p => p.ProductMaxDiscountAmount < 10).ToList();
                        CounterList.Content = $"Показано записей { ListProducts.Items.Count } из { db.Product.ToList().Count }";
                        break;
                    }
                case 1:
                    {
                        ListProducts.ItemsSource = db.Product.Where(p => p.ProductMaxDiscountAmount > 10 && p.ProductMaxDiscountAmount < 15).ToList();
                        CounterList.Content = $"Показано записей { ListProducts.Items.Count } из { db.Product.ToList().Count }";
                        break;
                    }
                case 2:
                    {
                        ListProducts.ItemsSource = db.Product.Where(p => p.ProductMaxDiscountAmount > 15).ToList();
                        CounterList.Content = $"Показано записей { ListProducts.Items.Count } из { db.Product.ToList().Count }";
                        break;
                    }
                case 3:
                    {
                        ListProducts.ItemsSource = db.Product.ToList();
                        CounterList.Content = $"Показано записей { ListProducts.Items.Count } из { db.Product.ToList().Count }";
                        break;
                    }
            }
        }

        private void OrderByFilter_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (OrderByFilter.SelectedIndex)
            {
                case 0:
                    {
                        ListProducts.ItemsSource = db.Product.ToList();
                        CounterList.Content = $"Показано записей { ListProducts.Items.Count } из { db.Product.ToList().Count }";
                        break;
                    }
                case 1:
                    {
                        ListProducts.ItemsSource = db.Product.OrderBy(x => x.ProductCost).ToList();
                        CounterList.Content = $"Показано записей { ListProducts.Items.Count } из { db.Product.ToList().Count }";
                        break;
                    }
                case 2:
                    {
                        ListProducts.ItemsSource = db.Product.OrderByDescending(x => x.ProductCost).ToList();
                        CounterList.Content = $"Показано записей { ListProducts.Items.Count } из { db.Product.ToList().Count }";
                        break;
                    }
            }
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            ListProducts.ItemsSource = db.Product.Where(x => x.ProductName.Contains(Search.Text)).ToList();
            CounterList.Content = $"Показано записей { ListProducts.Items.Count } из { db.Product.ToList().Count }";
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            var product = (sender as Button)?.DataContext as Product;
            new EditCreateWindow(product, _user, true).Show();
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new EditCreateWindow(new Product(), _user, false).Show();
        }

        private void Grid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (OrderProducts == null)
                OrderProducts = new List<OrderProduct>();

            var result = MessageBox.Show("Добавить товар в заказ?", "Добавление товара", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    var product = (((sender as Grid).DataContext as Product));
                    if (OrderProducts.Select(x => x.Product).ToList().Contains(product))
                        OrderProducts.Find(x => x.Product == product).Count = OrderProducts.Find(x => x.Product == product).Count += 1;
                    else
                        OrderProducts.Add(new OrderProduct{ Product = product, Count = 1});
                    break;
                case MessageBoxResult.Cancel:
                    break;
                case MessageBoxResult.None:
                case MessageBoxResult.OK:
                case MessageBoxResult.No:
                default:
                    break;
            }

            if(OrderProducts.Any())
                CreateOrderButton.Visibility = Visibility.Visible;
            else
                CreateOrderButton.Visibility = Visibility.Hidden; 
        }

        private void CreateOrderButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new OrderWindow(OrderProducts).Show();
        }
    }
}
