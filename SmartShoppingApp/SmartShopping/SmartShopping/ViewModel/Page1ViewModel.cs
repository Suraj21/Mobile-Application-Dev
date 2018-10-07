
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartShopping.ViewModel
{
    public class Page1ViewModel: INotifyPropertyChanged
    {
        public Page1ViewModel()
        {
           // OnScanCommand =  new Command(OnScan);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string resultText;
        public string ResultText
        {
            get { return resultText; }
            set
            {
                resultText = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ResultText"));
            }
        }

        //public ICommand OnScanCommand { protected set; get; }

        //public async void OnScan()
        //{
        //    try
        //    {
        //        //var app = new Android.App.Application();
        //        //MobileBarcodeScanner.Initialize(app);
        //        ////MobileBarcodeScanner.Initialize(Application.Current);

        //        //var scanner = new MobileBarcodeScanner();
        //        //scanner.TopText = "Hold the camera up to the barcode\nAbout 6 inches away";
        //        //scanner.BottomText = "Wait for the barcode to automatically scan!";

        //        ////This will start scanning
        //        //ZXing.Result result = await scanner.Scan();

        //        ////Show the result returned.
        //        //HandleResult(result);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
        //void HandleResult(ZXing.Result result)
        //{
        //    var msg = "No Barcode!";
        //    if (result != null)
        //    {
        //        msg = "Barcode: " + result.Text + " (" + result.BarcodeFormat + ")";
        //        ProductsModel productsModel = new ProductsModel()
        //        {
        //            ProductId = re
        //        }
        //    }

        //    DisplayAlert("", msg, "Ok");
        //}
    }
}
