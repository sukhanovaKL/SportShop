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
using System.Windows.Threading;

namespace SportShop
{
    /// <summary>
    /// Логика взаимодействия для Captcha.xaml
    /// </summary>
    public partial class Captcha : Window
    {
        private SportShopEntities db;

        private List<User> _users;

        public Captcha()
        {
            InitializeComponent();
            db = new SportShopEntities();
            _users = new List<User>(db.User.ToList());

            GetCaptcha();
        }

        public void GetCaptcha()
        {
            String allowchar = "";

            allowchar = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            allowchar += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,y,z";
            allowchar += "1,2,3,4,5,6,7,8,9,0";

            char[] a = { ',' };
            String[] ar = allowchar.Split(a);
            String pwd = "";
            string temp = "";
            Random r = new Random();

            for (int i = 0; i < 6; i++)
            {
                temp = ar[r.Next(0, ar.Length)];
                pwd += temp;
            }

            CaptchaText.Text = pwd;
        }

        private void EntryButton_Click(object sender, RoutedEventArgs e)
        {
            if (_users.Any(u => u.UserLogin == UserLogin.Text && u.UserPassword == UserPassword.Text && u.RoleID == 1) && CaptchaUser.Text.ToString() == CaptchaText.Text.ToString())
            {
                Hide();
                new ClientWindow(_users.Find(u => u.UserLogin == UserLogin.Text && u.UserPassword == UserPassword.Text && u.RoleID == 1)).ShowDialog();
            }
            else if (_users.Any(u => u.UserLogin == UserLogin.Text && u.UserPassword == UserPassword.Text && u.RoleID == 2) && CaptchaUser.Text.ToString() == CaptchaText.Text.ToString())
            {
                Hide();
                new AdminWindow(_users.Find(u => u.UserLogin == UserLogin.Text && u.UserPassword == UserPassword.Text && u.RoleID == 2)).ShowDialog();
            }
            else if (_users.Any(u => u.UserLogin == UserLogin.Text && u.UserPassword == UserPassword.Text && u.RoleID == 3) && CaptchaUser.Text.ToString() == CaptchaText.Text.ToString())
            {
                Hide();
                new ManagerWindow(_users.Find(u => u.UserLogin == UserLogin.Text && u.UserPassword == UserPassword.Text && u.RoleID == 3)).ShowDialog();
            }
            else
            {
                GetCaptcha();
                UserPassword.Text = "";
                MessageBox.Show("Вы заблокированы на 10 секунд!");

                BlockUser();
                DispatcherTimer timer = new DispatcherTimer();
                timer.Tick += new EventHandler(UnblockUser);
                timer.Interval = new TimeSpan(0, 0, 10);
                timer.Start();
            }
        }

        private void BlockUser()
        {
            UserLogin.IsEnabled = false;
            UserPassword.IsEnabled = false;
            CaptchaUser.IsEnabled = false;
        }

        private void UnblockUser(object sender, EventArgs e)
        {
            UserLogin.IsEnabled = true;
            UserPassword.IsEnabled = true;
            CaptchaUser.IsEnabled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new ProductsWindow(null).Show(); ;
        }
    }
}
