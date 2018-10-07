using SmartShopping.Model;
using SmartShopping.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace SmartShopping.ViewModel.Page
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Page1 : ContentPage
    {
        string UserId = string.Empty;
		public Page1 (string userName, string userId)
		{
			InitializeComponent ();
            var vm = new Page1ViewModel();
            this.BindingContext = vm;
            UserNameLbl.Text = "Welcome " + userName;
            UserNameLbl.FontSize = 18;
            UserId = userId;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                var app = new Android.App.Application();
                MobileBarcodeScanner.Initialize(app);

                var scanner = new MobileBarcodeScanner();
                scanner.TopText = "Hold the camera up to the QR code\nAbout 6 inches away";
                scanner.BottomText = "Wait for the QR code to automatically scan!";

                //This will start scanning
                ZXing.Result result = await scanner.Scan();

                string[] lines = result.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                ProductsModel productsModel = new ProductsModel()
                {
                    ProductId = lines[0].Substring(lines[0].IndexOf('-') + 1),
                    SerialNo = "1",
                    ProductDescription = lines[1].Substring(lines[1].IndexOf('-') + 1),
                    UnitPrice = lines[2].Substring(lines[2].IndexOf('-') + 1),
                    SellPrice = lines[3].Substring(lines[3].IndexOf('-') + 1),
                    Quantity = lines[4].Substring(lines[4].IndexOf('-') + 1)
                };
               await Navigation.PushAsync(new ProductListPage(productsModel, UserId));
            }
            catch (Exception ex)
            {
               //await DisplayAlert("Error", ex.Message, "Ok");
            }
        }    
    }
}