using SmartShopping.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartShopping.Page
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckOrderStatus : ContentPage
    {
        ObservableCollection<ProductsModel> productsCollection = new ObservableCollection<ProductsModel>();
        string TotalAmounts = string.Empty;
        string UserId = string.Empty;
        string OrderId = string.Empty;
        bool IsCartReady = false;

        public CheckOrderStatus(ObservableCollection<ProductsModel> _productsCollection, string totalAmount, string userId, string orderId, bool isCartReady)
        {
            InitializeComponent();
            productsCollection = _productsCollection;
            ProductsView.ItemsSource = _productsCollection.OrderBy(x=>x.SerialNo);
            UserId = userId;
            OrderId = orderId;
            TotalAmount.Text = totalAmount;
            TotalAmounts = totalAmount;
            IsCartReady = isCartReady;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new OrderNotificationPage(productsCollection, TotalAmounts, UserId, OrderId, IsCartReady));
        }

        private void Exit_Button_Clicked(object sender, EventArgs e)
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}