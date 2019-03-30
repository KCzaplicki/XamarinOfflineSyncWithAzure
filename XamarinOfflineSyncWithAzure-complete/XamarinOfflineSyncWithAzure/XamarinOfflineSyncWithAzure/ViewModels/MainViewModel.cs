using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Plugin.Connectivity;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using XamarinOfflineSyncWithAzure.Annotations;
using XamarinOfflineSyncWithAzure.Models;

namespace XamarinOfflineSyncWithAzure.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IMobileServiceSyncTable<Project> _projectTable;
        private readonly MobileServiceClient _mobileService;
        private string _newProjectName;

        public MainViewModel(MobileServiceClient mobileService, IMobileServiceSyncTable<Project> projectTable)
        {
            _projectTable = projectTable;
            _mobileService = mobileService;
            AddProjectCommand = new Command(AddProject);
            DeleteProjectCommand = new Command<Project>(DeleteProject);
        }

        public ObservableCollection<Project> Projects { get; } = new ObservableCollection<Project>();
        public Command AddProjectCommand { get; }
        public Command DeleteProjectCommand { get; }
        public string NewProjectName
        {
            get => _newProjectName;
            set
            {
                if (_newProjectName == value)
                {
                    return;
                }

                _newProjectName = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async void Initialize()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                await _projectTable.PullAsync(null, _projectTable.CreateQuery());
            }

            var projectItems = await _projectTable.ReadAsync();
            projectItems.ForEach(item => Projects.Add(item));
        }

        public async void AddProject()
        {
            var project = new Project
            {
                Name = NewProjectName
            };
            NewProjectName = "";
            await _projectTable.InsertAsync(project);
            Projects.Add(project);

            if (CrossConnectivity.Current.IsConnected)
            {
                await SyncWithAzureAsync();
            }
        }

        public async void DeleteProject(Project project)
        {
            await _projectTable.DeleteAsync(project);
            Projects.Remove(project);

            if (CrossConnectivity.Current.IsConnected)
            {
                await SyncWithAzureAsync();
            }
        }

        private async Task SyncWithAzureAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await _mobileService.SyncContext.PushAsync();
                await _projectTable.PullAsync("projectTable", _projectTable.CreateQuery());
            } 
            catch(MobileServicePushFailedException ex)
            {
                if(ex.PushResult != null)
                {
                    syncErrors = ex.PushResult.Errors;
                }
            }

            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.Result != null)
                    {
                        await error.CancelAndUpdateItemAsync(error.Result);
                        var localItem = (Project)error.Item.ToObject(typeof(Project));
                        var index = Projects.IndexOf(x => x.Id == localItem.Id);
                        var remoteItem = (Project)error.Result.ToObject(typeof(Project));
                        Projects[index] = remoteItem;
                    }
                    else
                    {
                        await error.CancelAndDiscardItemAsync();
                    }
                }
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
