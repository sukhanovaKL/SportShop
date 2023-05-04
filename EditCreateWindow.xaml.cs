using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для EditCreateWindow.xaml
    /// </summary>
    public partial class EditCreateWindow : Window
    {
        private SportShopEntities db = new SportShopEntities();

        private User _user;

        private Product _product;

        private bool _isEdit;

        private string _imagePath;

        public EditCreateWindow(Product product, User user, bool isEdit)
        {
            InitializeComponent();
            _user = user;
            ProductGrid.DataContext = product;
            _product = product;
            _isEdit = isEdit;
            ProductArticleNumberTextBox.IsEnabled = !_isEdit;
            CategoryComboBox.ItemsSource = db.ProductCategory.Select(x => x.ProductCategoryName).ToList();
            ManufacturerComboBox.ItemsSource = db.ProductManufacturer.Select(x => x.ProductManufacturerName).ToList();
            ProductSupplierComboBox.ItemsSource = db.ProductSupplier.Select(x => x.ProductSupplierName).ToList();
            UnitTypeCombobox.ItemsSource = db.UnitType.Select(u => u.UnitTypeName).ToList();
        }

        private void ExitButtom_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new ProductsWindow(_user).Show();
        }

        private void SaveButtom_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                if (_isEdit)
                {
                    var updatedProduct = db.Product.Find(_product.ProductID);
                    updatedProduct.ProductName = _product.ProductName;
                    updatedProduct.ProductCategoryID = db.ProductCategory.ToList().Find(c => c.ProductCategoryName == CategoryComboBox.SelectedValue.ToString()).ProductCategoryID;
                    updatedProduct.ProductManufacturerID = db.ProductManufacturer.ToList().Find(m => m.ProductManufacturerName == ManufacturerComboBox.SelectedValue.ToString()).ProductManufacturerID;
                    updatedProduct.ProductMaxDiscountAmount = _product.ProductMaxDiscountAmount;
                    updatedProduct.ProductDiscountAmount = _product.ProductDiscountAmount;
                    updatedProduct.ProductCost = _product.ProductCost;
                    updatedProduct.ProductDescription = _product.ProductDescription;
                }
                else
                {
                    _product.ProductManufacturerID = db.ProductManufacturer.ToList().Find(m => m.ProductManufacturerName == ManufacturerComboBox.SelectedValue.ToString()).ProductManufacturerID;
                    _product.ProductCategoryID = db.ProductCategory.ToList().Find(c => c.ProductCategoryName == CategoryComboBox.SelectedValue.ToString()).ProductCategoryID;
                    _product.ProductSupplierID = db.ProductSupplier.ToList().Find(s => s.ProductSupplierName == ProductSupplierComboBox.SelectedValue.ToString()).ProductSupplierID;
                    _product.UnitTypeID = db.UnitType.ToList().Find(u => u.UnitTypeName == UnitTypeCombobox.SelectedValue.ToString()).UnitTypeID;
                    _product.ProductPhoto = _imagePath;
                    db.Product.Add(_product);
                }

                db.SaveChanges();
                MessageBox.Show("Успешно сохранено");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void EditPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var Picture = _product.ProductPhoto;
                OpenFileDialog opFD = new OpenFileDialog();
                opFD.ShowDialog();
                var imag = opFD.FileName;
                string dest = "C:/Users/79393/source/repos/SportShop/Photos/" + System.IO.Path.GetFileName(imag);
                Image image = new Image();
                var bi = new BitmapImage(new Uri(dest));
                Photo.Source = bi;
                var pr = db.Product.ToList().Find(f => f.ProductID == _product.ProductID);
                if (pr == null)
                    _imagePath = opFD.SafeFileName;
                else
                {
                    pr.ProductPhoto = opFD.SafeFileName;
                    db.SaveChanges();
                }
           
                ProductGrid.DataContext = pr;
                File.Copy(imag, dest);
            }
            catch
            {
         
            }
        }

        private void DeleteButtom_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Product product = db.Product.ToList().Find(p => p.ProductID == ((sender as Button).DataContext as Product).ProductID);
                db.Product.Remove(product);
                db.SaveChanges();
                Hide();
                new ProductsWindow(_user).Show();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Данную запись нельзя удалить!" + ex.Message);
            }
        }
    }
}
