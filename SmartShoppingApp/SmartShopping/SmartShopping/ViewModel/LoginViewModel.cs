using Android.Content.Res;
using Newtonsoft.Json;
using SmartShopping.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;

namespace SmartShopping.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public Action DisplayInvalidUserLoginPrompt;
        public Action DisplayInvalidMobileLoginPrompt;
        public Action NavigateToOtherPage;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Email"));
            }
        }

        private string username;
        public string UserName
        {
            get { return username; }
            set
            {
                username = value;
                PropertyChanged(this, new PropertyChangedEventArgs("UserName"));
            }
        }

        private string mobileNo;
        public string MobileNo
        {
            get { return mobileNo; }
            set
            {
                mobileNo = value;
                PropertyChanged(this, new PropertyChangedEventArgs("MobileNo"));
            }
        }


        public ICommand SubmitCommand { protected set; get; }
        public LoginViewModel()
        {
            SubmitCommand = new Command(OnSubmit);
        }
        public void OnSubmit()
        {
            if (string.IsNullOrEmpty(username) || username.Length < 3)
            {
                DisplayInvalidUserLoginPrompt();
            }
            if (string.IsNullOrEmpty(mobileNo) || mobileNo.Length < 10)
            {
                DisplayInvalidMobileLoginPrompt();
            }
            else
            {
                UserModel userModel = new UserModel()
                {
                    Email = Email,
                    MobileNumber = MobileNo,
                    UserId = MobileNo,
                    UserName = UserName
                };
                UserName = userModel.UserName;
                PostUserDataToServer(userModel); //Api to call to save the User Details to Database
                NavigateToOtherPage();
            }
        }

        private static async void PostUserDataToServer(UserModel userModel)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //var uri = new Uri("http://192.168.43.3:8085/api/Registration");
                    var uri = new Uri("http://virtualshoppingcart-env.kc3cmgmk6m.us-east-2.elasticbeanstalk.com/api/Registration");
                    string serializedObject = JsonConvert.SerializeObject(userModel);
                    HttpContent contentPost = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(uri, contentPost);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
