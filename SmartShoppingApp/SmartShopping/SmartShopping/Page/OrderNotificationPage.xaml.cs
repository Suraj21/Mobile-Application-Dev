using Android.Graphics.Pdf;
using Newtonsoft.Json;
using SmartShopping.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartShopping.Page
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OrderNotificationPage : ContentPage
	{
        ObservableCollection<ProductsModel> productsCollection = new ObservableCollection<ProductsModel>();
        string TotalAmount = string.Empty;
        string UserId = string.Empty;
        string OrderId = string.Empty;
        bool isCartReady = false;

        public OrderNotificationPage (ObservableCollection<ProductsModel> _productsCollection, string totalAmount, string userId, string orderId, bool _isCartReady)
		{
			InitializeComponent ();
            productsCollection = _productsCollection;
            TotalAmount = totalAmount;
            OrderNotificationLbl.Text = "Your Cart with Order No " + orderId + " is prepared, we will inform you once your cart is ready";
            OrderStatusLbl.Text = "We are preparing your cart....";
            OrderStatusLbl.TextColor = Color.OrangeRed;
            OrderStatusLbl.FontSize = 22;
            UserId = userId;
            OrderId = orderId;
            Device.StartTimer(new TimeSpan(0, 0, 0, 2), () =>
              {
                  GetOrderStatus();
                  return true;
              });
            isCartReady = _isCartReady;
        }

        public async void GetOrderStatus()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //var uri = new Uri("http://virtualshoppingcart-env.kc3cmgmk6m.us-east-2.elasticbeanstalk.com/api/PlacedOrder");
                    var uri = new Uri("http://virtualshoppingcart-env.kc3cmgmk6m.us-east-2.elasticbeanstalk.com/api/ReadyOrder?userId=" + UserId + "&OrderId=" + OrderId);
                    client.BaseAddress = uri;
                    //HTTP GET
                    var responseTask = client.GetStringAsync(uri);
                    responseTask.Wait();

                    string json = responseTask.Result;

                    if(Convert.ToInt32(json) == 3 || isCartReady)
                    {
                        OrderNotificationLbl.Text = "Your Cart with Order No " + OrderId + " is ready, please take your order from the delivery counter..";
                        OrderStatusLbl.Text = "Your Cart is Ready";
                        OrderStatusLbl.TextColor = Color.LightSeaGreen;
                        OrderStatusLbl.FontSize = 26;
                        isCartReady = true;
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            // Create a new PDF document
          
        }

        private void Checked_Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CheckOrderStatus(productsCollection, TotalAmount,UserId,OrderId, isCartReady));

        }

        protected override bool OnBackButtonPressed()
        {
            // Begin an asyncronous task on the UI thread because we intend to ask the users permission.
            Device.BeginInvokeOnMainThread(async () =>
            {
                var answer = (await DisplayAlert("Exit page?", "Are you sure you want to exit this page? You will not be able to continue it.", "Yes", "No"));
                if (answer.Equals("Yes"))
                {
                    App.Current.MainPage = new NavigationPage(new MainPage());
                    await Navigation.PopAsync(true);
                }
                else
                {
                    Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
                }
            });

            // Always return true because this method is not asynchronous.
            // We must handle the action ourselves: see above.
            return true;
        }
    }
}