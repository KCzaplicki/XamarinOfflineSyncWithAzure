using System;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XamarinOfflineSyncWithAzure
{
    public partial class App : Application
    {
        private const string MobileServiceClientUrl = "https://xamarinofflinesyncwithazure.azurewebsites.net";
        public static MobileServiceClient MobileService = new MobileServiceClient(MobileServiceClientUrl);

        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
