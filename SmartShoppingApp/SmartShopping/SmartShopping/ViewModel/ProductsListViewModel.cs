using Android.Runtime;
using SmartShopping.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartShopping.ViewModel
{
    public class ProductsListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<ProductsModel> productsList;
        public Func<string> IncrementFunction;
        public ICommand IncrementCommand { protected set; get; }

        private string productId;
        public string ProductId
        {
            get { return productId; }
            set
            {
                productId = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ProductId"));
            }
        }

        public ObservableCollection<ProductsModel> ProductsList
        {
            get { return productsList; }
            set { productsList = value; PropertyChanged(this, new PropertyChangedEventArgs("ProductsList")); }
        }
        public ProductsListViewModel()
        {
            IncrementCommand = new Command<string>(OnIncrement);
        }

        public void OnIncrement(string value)
        {

        }
    }
}
