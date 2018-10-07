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
    public partial class PaymentPage : ContentPage
    {
        ObservableCollection<ProductsModel> producList = new ObservableCollection<ProductsModel>();

        string OrderId = string.Empty;
        string TotalAmount = string.Empty;
        string UserID = String.Empty;
        public PaymentPage(ObservableCollection<ProductsModel> _producList, string orderId, string totlalAmount, string userID)
        {
            InitializeComponent();

            OrderId = orderId;
            producList = _producList;
            TotalAmount = totlalAmount;
            UserID = userID;
        }

        private void Cash_Pay_Button_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Cash Payment", "We have received your Order. Your Order Id is " + OrderId + ", Please Pay Amount at Cash Counter", "Ok");
            SaveData("Cash", "Pending");
            Navigation.PushAsync(new OrderNotificationPage(producList, TotalAmount, UserID, OrderId,false));
        }

        private void Online_Pay_Button_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Online Payment", "We have received your Order. Your Order Id is " + OrderId, "Ok");
            SaveData("Online", "Success");
            Navigation.PushAsync(new OrderNotificationPage(producList, TotalAmount, UserID, OrderId,false));
        }

        public async void SaveData(string paymentMode, string paymentStatus)
        {
            Random generator = new Random();
            String paymentId = generator.Next(0, 999999).ToString("D6");

            List<Product> productsList = new List<Product>();
            Product product = new Product();
            foreach (var item in producList)
            {
                product = new Product();
                product.Discount = 0;
                product.ModifiedDate = null;
                product.ProductDescription = item.ProductDescription;
                product.ProductId = Convert.ToInt32(item.ProductId);
                product.Quantity = Convert.ToDecimal(item.Quantity);
                product.SellPrice = Convert.ToDecimal(item.SellPrice);
                product.UnitPrice = Convert.ToDecimal(item.UnitPrice);

                productsList.Add(product);
                product = null;
            }

            OrderModel orderModel = new OrderModel()
            {
                OrderId = OrderId,
                orderStatus = 1,
                PaymentDetails = new PaymentDetails()
                {
                    PaymentId = paymentId,
                    PaymentMode = paymentMode,
                    PaymentStatus = paymentStatus,
                    TotalSellprice = Convert.ToDecimal(TotalAmount)
                },
                products = productsList,
                UserId = UserID
            };

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var uri = new Uri("http://virtualshoppingcart-env.kc3cmgmk6m.us-east-2.elasticbeanstalk.com/api/PlacedOrder");
                string serializedObject = JsonConvert.SerializeObject(orderModel);
                HttpContent contentPost = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(uri, contentPost);

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                }
            }
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
            });

            // Always return true because this method is not asynchronous.
            // We must handle the action ourselves: see above.
            return true;
        }
    }
}