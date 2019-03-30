using System;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinOfflineSyncWithAzure.Models;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XamarinOfflineSyncWithAzure
{
    public partial class App : Application
    {
        private const string MobileServiceClientUrl = "https://xamarinofflinesyncwithazure.azurewebsites.net";
        private const string LocalStoreFilename = "localstore.db";
        public static MobileServiceClient MobileService = new MobileServiceClient(MobileServiceClientUrl);

        public App()
        {
            InitializeStore();
            InitializeComponent();

            MainPage = new MainPage();
        }

        private void InitializeStore()
        {
            var store = new MobileServiceSQLiteStore(LocalStoreFilename);
            store.DefineTable<Project>();
            MobileService.SyncContext.InitializeAsync(store);
        }
    }
}
