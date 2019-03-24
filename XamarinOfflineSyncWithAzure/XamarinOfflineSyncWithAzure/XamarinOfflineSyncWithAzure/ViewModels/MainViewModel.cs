using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using XamarinOfflineSyncWithAzure.Annotations;
using XamarinOfflineSyncWithAzure.Models;

namespace XamarinOfflineSyncWithAzure.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IMobileServiceTable<Project> _projectTable;
        private string _newProjectName;

        public MainViewModel(IMobileServiceTable<Project> projectTable)
        {
            _projectTable = projectTable;

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
        }

        public async void DeleteProject(Project project)
        {
            await _projectTable.DeleteAsync(project);
            Projects.Remove(project);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
