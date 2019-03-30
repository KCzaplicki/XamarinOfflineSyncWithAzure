using Xamarin.Forms;
using XamarinOfflineSyncWithAzure.Models;
using XamarinOfflineSyncWithAzure.ViewModels;

namespace XamarinOfflineSyncWithAzure
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            var projectTable = App.MobileService.GetSyncTable<Project>();
            var vm = new MainViewModel(App.MobileService, projectTable);
            vm.Initialize();

            BindingContext = vm;
            InitializeComponent();
        }
    }
}
