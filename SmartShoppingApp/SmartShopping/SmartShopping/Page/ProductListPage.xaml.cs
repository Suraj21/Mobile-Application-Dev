using Newtonsoft.Json;
using SmartShopping.Model;
using SmartShopping.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;

namespace SmartShopping.Page
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProductListPage : ContentPage
	{
        public ObservableCollection<ProductsModel> ProductsList { get; set; }
        decimal prodAddedPrice = 0;
        string UserID = string.Empty;
        public ProductListPage (ProductsModel productsModel,string userId)
		{
			InitializeComponent ();
            var vm = new ProductsListViewModel();
            this.BindingContext = vm;
            ProductsList = new ObservableCollection<ProductsModel>();
            ProductsList.Add(productsModel);
            ProductsView.ItemsSource = ProductsList;
            ProductsView.SelectedItem = productsModel;
            ProductsView.ItemSelected += ProductsView_ItemSelected;
            TotalAmount.Text = productsModel.SellPrice;
            UserID = userId;
        }

        private void ProductsView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ProductsView.Focus();
        }

        private async void Continue_Shopping_Button_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                var app = new Android.App.Application();
                MobileBarcodeScanner.Initialize(app);

                var scanner = new MobileBarcodeScanner();
                scanner.TopText = "Hold the camera up to the QR code\nAbout 6 inches away";
                scanner.BottomText = "Wait for the QRcode to automatically scan!";

                //This will start scanning
                ZXing.Result result = await scanner.Scan();

                string[] lines = result.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                string nxtSerialNo = Convert.ToString(ProductsList.Count + 1);

                ProductsModel productsModel = new ProductsModel()
                {
                    ProductId = lines[0].Substring(lines[0].IndexOf('-') + 1),
                    SerialNo = nxtSerialNo,
                    ProductDescription = lines[1].Substring(lines[1].IndexOf('-') + 1),
                    UnitPrice = lines[2].Substring(lines[2].IndexOf('-') + 1),
                    SellPrice = lines[3].Substring(lines[3].IndexOf('-') + 1),
                    Quantity = lines[4].Substring(lines[4].IndexOf('-') + 1)
                };

                ProductsModel productsModelData = (from row in ProductsList where row.ProductId == productsModel.ProductId select row).FirstOrDefault();
                if (productsModelData != null)
                {
                    ProductsList.Remove(productsModelData);
                    productsModelData.Quantity = Convert.ToString(Convert.ToInt32(productsModelData.Quantity) + 1);
                    ProductsList.Add(productsModelData);
                    prodAddedPrice =  Convert.ToDecimal(productsModelData.SellPrice);
                }
                else
                {
                    productsModelData = productsModel;
                    ProductsList.Add(productsModel);
                    prodAddedPrice = Convert.ToDecimal(productsModel.SellPrice);
                }

                TotalAmount.Text = Convert.ToString(Convert.ToDecimal(TotalAmount.Text) + prodAddedPrice);

                ProductsView.ItemsSource = null;
                ProductsView.ItemsSource = ProductsList;
                ProductsView.SelectedItem = null;
               ProductsView.SelectedItem = productsModelData;
            }
            catch (Exception ex)
            {
               await  DisplayAlert("Exception", ex.Message, "Ok");
            }
        }

        private void Increment_Button_Clicked(object sender, System.EventArgs e)
        {
            decimal basePrice = 0;
            ProductsModel selectedItem = ProductsView.SelectedItem as ProductsModel;
            ProductsModel productsModelData = (from row in ProductsList where row.ProductId == selectedItem.ProductId select row).FirstOrDefault();
            if (productsModelData != null)
            {
                ProductsList.Remove(productsModelData);
                //basePrice = Convert.ToInt32(productsModelData.Price) / Convert.ToInt32(productsModelData.Quantity);
                basePrice = Convert.ToInt32(productsModelData.SellPrice);
                productsModelData.Quantity = Convert.ToString(Convert.ToInt32(productsModelData.Quantity) + 1);

                //productsModelData.Price = Convert.ToString(Convert.ToInt32(productsModelData.Price) + basePrice);
                ProductsList.Add(productsModelData);
                prodAddedPrice = Convert.ToInt32(productsModelData.SellPrice);
            }
            TotalAmount.Text = Convert.ToString(Convert.ToDecimal(TotalAmount.Text) + basePrice);

            ProductsView.ItemsSource = null;
            ProductsView.ItemsSource = ProductsList.OrderBy(x => x.SerialNo);
            ProductsView.SelectedItem = null;
            ProductsView.SelectedItem = productsModelData;
        }

        private void Decrement_Button_Clicked(object sender, System.EventArgs e)
        {
            decimal basePrice = 0;
           ProductsModel selectedItem = ProductsView.SelectedItem as ProductsModel;
            ProductsModel productsModelData = (from row in ProductsList where row.ProductId == selectedItem.ProductId select row).FirstOrDefault();
            if (productsModelData != null && Convert.ToInt32(productsModelData.Quantity) > 0)
            {
                ProductsList.Remove(productsModelData);
                basePrice = Convert.ToInt32(productsModelData.SellPrice);
                productsModelData.Quantity = Convert.ToString(Convert.ToInt32(productsModelData.Quantity) - 1);

                //productsModelData.Price = Convert.ToString(Convert.ToInt32(productsModelData.Price) - basePrice);
                ProductsList.Add(productsModelData);
                prodAddedPrice = Convert.ToInt32(productsModelData.SellPrice);
            
            //else if(Convert.ToInt32(productsModelData.Quantity) == 1)
            //{
            //    Delete_Button_Clicked(sender, e);
            //}
            TotalAmount.Text = Convert.ToString(Convert.ToDecimal(TotalAmount.Text) - basePrice);
            ProductsView.ItemsSource = null;
            ProductsView.ItemsSource = ProductsList.OrderBy(x => x.SerialNo);
            ProductsView.SelectedItem = null;
            ProductsView.SelectedItem = productsModelData;
            }
        }

        private void Delete_Button_Clicked(object sender, System.EventArgs e)
        {
            decimal productPrice = 0;
           ProductsModel selectedItem = ProductsView.SelectedItem as ProductsModel;
            ProductsModel productsModelData = (from row in ProductsList where row.ProductId == selectedItem.ProductId select row).FirstOrDefault();
            if (productsModelData != null)
            {
                ProductsList.Remove(productsModelData);
                ProductsView.ItemsSource = null;
                ProductsView.ItemsSource = ProductsList;
                productPrice = Convert.ToInt32(productsModelData.SellPrice);
            }
            productsModelData = (from row in ProductsList select row).FirstOrDefault();
            if(productsModelData != null)
            {
                ProductsView.SelectedItem = null;
                ProductsView.SelectedItem = productsModelData;
            }
            
            TotalAmount.Text = Convert.ToString(Convert.ToDecimal(TotalAmount.Text) - productPrice);

        }

        private void Shopping_Done_Button_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                Random generator = new Random();
                String orderId = generator.Next(0, 999999).ToString("D6");
                Navigation.PushAsync(new PaymentPage(ProductsList, orderId, TotalAmount.Text, UserID));

            }
            catch (Exception ex)
            {

               // throw;
            }
        }
    }
}