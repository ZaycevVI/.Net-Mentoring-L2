using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OnlineShop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ProductService Service { get; }

        public MainWindow()
        {
            InitializeComponent();
            Service = new ProductService();
            DataContext = Service;
            ProductsListBox.ItemsSource = Service.Products;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text) || !(((TextBox)e.OriginalSource).Text.Length < 8);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var product = new Product
            {
                Name = NameTxtBox.Text,
                Price = Convert.ToInt32(PriceTxtBox.Text)
            };

            Service.AddAsync(product);
        }

        private void RemoveBtn_OnClick(object sender, RoutedEventArgs e)
        {
            Service.RemoveAsync(ProductsListBox.SelectedItem as Product);
        }
    }
}
