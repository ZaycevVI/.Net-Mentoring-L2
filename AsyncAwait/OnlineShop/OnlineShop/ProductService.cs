using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Data;
using OnlineShop.Annotations;

namespace OnlineShop
{
    public class ProductService : INotifyPropertyChanged
    {
        private readonly object _lock = new object();
        public ObservableCollection<Product> Products { get; }

        private int _totalPrice;

        public int TotalPrice
        {
            get => _totalPrice;
            private set
            {
                _totalPrice = value;
                OnPropertyChanged();
            }
        }

        public ProductService()
        {
            Products = new ObservableCollection<Product>();
            BindingOperations.EnableCollectionSynchronization(Products, _lock);
        }

        public Task AddAsync(Product product)
        {
            return Task.Run(() =>
            {
                Task.Delay(1000);
                Products.Add(product);
                product.PropertyChanged += UpdatePriceAsync;
                UpdatePriceAsync();
            });
        }

        public Task RemoveAsync(Product product)
        {
            return Task.Run(() =>
            {
                Task.Delay(1000);
                Products.Remove(product);
                UpdatePriceAsync();
            });
        }

        private void UpdatePriceAsync(object sender = null, PropertyChangedEventArgs arg = null)
        {
            Task.Delay(1500)
            .ContinueWith(task => TotalPrice = Products.Sum(p => p.Price));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
