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
using Excel = Microsoft.Office.Interop.Excel;

namespace SportShop
{
    /// <summary>
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        Order _order;

        List<OrderProduct> _orderProduct;

        SportShopEntities db = new SportShopEntities();

        public OrderWindow(List<OrderProduct> orderProduct)
        {
            InitializeComponent();

            _orderProduct = orderProduct;
            ListProducts.ItemsSource = _orderProduct.ToList();
            PickupPointComboBox.ItemsSource = db.PickupPoint.Select(x => x.Address).ToList();
            PriceText.Content = "Итоговая цена: " + _orderProduct.Select(f => f.Product).Select(f => f.ProductCostWithAmount).Sum().ToString();
        }

        private void CompleteOrderButton_Click(object sender, RoutedEventArgs e)
        {
            int orderCode = db.Order.OrderBy(x => x.OrderGetCode).ToList().Last().OrderGetCode + 1;

            _order = new Order
            {
                OrderStatusID = 1,
                PickupPointID = db.PickupPoint.ToList().Find(x => x.Address == PickupPointComboBox.SelectedValue.ToString()).PickupPointID,
                OrderCreateDate = DateTime.Now.Date,
                OrderDeliveryDate = DeliveryDateDatePicker.DisplayDate,
                OrderGetCode = orderCode
            };

            db.Order.Add(_order);
            db.SaveChanges();

            foreach(var product in _orderProduct)
            {
                var orderProduct = new OrderProduct
                {
                    OrderID = _order.OrderID,
                    ProductID = product.Product.ProductID,
                    Count = 1
                };

                db.OrderProduct.Add(orderProduct);
            }
            db.SaveChanges();

            MessageBox.Show("Заказ успешно создан :)");
            CompleteOrderButton.Visibility = Visibility.Hidden;
            _order.OrderProduct = _orderProduct;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var product = (sender as Button).DataContext as OrderProduct;
            _orderProduct.Remove(product);
            PriceText.Content = "Итоговая цена: " + _orderProduct.Select(f => f.Product).Select(f => f.ProductCostWithAmount).Sum().ToString();
            ListProducts.ItemsSource = _orderProduct.ToList();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new ProductsWindow(null).Show();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            var orders = _order;
            var app = new Excel.Application
            {
                SheetsInNewWorkbook = 1
            };
            var workbook = app.Workbooks.Add(Type.Missing);

            Excel.Worksheet worksheet = app.Worksheets.Item[1];
            worksheet.Name = "Card";

            worksheet.Cells[1][1] = "Order number";
            worksheet.Cells[1][2] = "Product list";
            worksheet.Cells[1][3] = "Total cost";

            worksheet.Cells[2][1] = orders.OrderID;

            var fullProductList = string.Empty;
            fullProductList = orders.OrderProduct.Aggregate(fullProductList,
                (current, product) => current + $"{product.Product.ProductName}\n");
            worksheet.Cells[2][2] = fullProductList;
            worksheet.Cells[2][3] = orders.OrderProduct.Sum(p => p.Product.ProductCost);

            worksheet.Columns.AutoFit();

            app.Visible = true;

            app.Application.ActiveWorkbook.SaveAs(@"C:\Users\79393\source\repos\SportShop\test.xlsx");

            var excelDocument = app.Workbooks.Open(@"C:\Users\79393\source\repos\SportShop\test.xlsx");
            excelDocument.ExportAsFixedFormat(Excel.XlFixedFormatType.xlTypePDF, @"C:\Users\79393\source\repos\SportShop\test.pdf");
            excelDocument.Close(false, "", false);
            app.Quit();
        }
    }
}