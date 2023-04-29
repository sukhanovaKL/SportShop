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
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        private readonly SportShopEntities db; 

        private List<User> _users;

        public Authorization()
        {
            InitializeComponent();
            db = new SportShopEntities();
            _users = new List<User>(db.User.ToList());
        }

        private void EntryButton_Click(object sender, RoutedEventArgs e)
        {
            if (_users.Any(u => u.UserLogin == UserLogin.Text && u.UserPassword == UserPassword.Text && u.RoleID == 1))
            {
                Hide();
                new ClientWindow(_users.Find(u => u.UserLogin == UserLogin.Text && u.UserPassword == UserPassword.Text && u.RoleID == 1)).ShowDialog();
            }
            else if (_users.Any(u => u.UserLogin == UserLogin.Text && u.UserPassword == UserPassword.Text && u.RoleID == 2))
            {
                Hide();
                new AdminWindow(_users.Find(u => u.UserLogin == UserLogin.Text && u.UserPassword == UserPassword.Text && u.RoleID == 2)).ShowDialog();
            }
            else if (_users.Any(u => u.UserLogin == UserLogin.Text && u.UserPassword == UserPassword.Text && u.RoleID == 3))
            {
                Hide();
                new ManagerWindow(_users.Find(u => u.UserLogin == UserLogin.Text && u.UserPassword == UserPassword.Text && u.RoleID == 3)).ShowDialog();
            }
            else
            {
                Hide();
                MessageBox.Show("Введен неверный логин или пароль!");
                new Captcha().ShowDialog();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new ProductsWindow(null).Show(); 
        }
    }
}