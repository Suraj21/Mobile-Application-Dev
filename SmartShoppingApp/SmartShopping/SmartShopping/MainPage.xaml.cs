using Android.Content.Res;
using SmartShopping.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SmartShopping.ViewModel.Page;

namespace SmartShopping
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
            var vm = new LoginViewModel();
            this.BindingContext = vm;
            vm.DisplayInvalidUserLoginPrompt += () => DisplayAlert("Error", "Invalid User Name (User Name Should be greater than 3 and should contains only characters)", "OK");
            vm.DisplayInvalidMobileLoginPrompt += () => DisplayAlert("Error", "Invalid Mobile No (Mobile No Should be of 10 digits)", "OK");
            vm.NavigateToOtherPage += () => Navigation.PushAsync(new Page1(vm.UserName,vm.MobileNo));

            InitializeComponent();

            UserName.Completed += (object sender, EventArgs e) =>
            {
                Email.Focus();
            };

            Email.Completed += (object sender, EventArgs e) =>
            {
                MobNo.Focus();
            };

            MobNo.Completed += (object sender, EventArgs e) =>
            {
                vm.SubmitCommand.Execute(null);
            };
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
                //Navigation.PushAsync(new Page1()); 
        }
    } 
}
