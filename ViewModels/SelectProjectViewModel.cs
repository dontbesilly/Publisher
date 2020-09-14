using Publisher.Models;
using Publisher.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Data;

namespace Publisher.ViewModels
{
    public class SelectProjectViewModel : INotifyPropertyChanged
    {
        private readonly VariablesService variablesService;
        private readonly MainWindowViewModel mainWindowViewModel;

        public ObservableCollection<PublishProject> PublishProjects { get; set; }

        public SelectProjectViewModel(VariablesService variablesService, MainWindowViewModel mainWindowViewModel)
        {
            this.variablesService = variablesService;
            this.mainWindowViewModel = mainWindowViewModel;
            PublishProjects = new ObservableCollection<PublishProject>();
        }

        public void GetAllProjects()
        {
            PublishProjects.Clear();
            foreach (string directory in Directory.GetDirectories(variablesService.PathToProjects))
            {
                if (directory.ToLower().Contains("test"))
                    continue;

                var directoryInfo = new DirectoryInfo(Path.Combine(variablesService.PathToProjects, directory));
                var files = directoryInfo.GetFiles("*.csproj");

                if (files.Length <= 0) continue;

                var fileName = files[0].Name;
                var projectName = fileName.Remove(fileName.Length - 7);
                PublishProjects.Add(new PublishProject
                {
                    IsSelected = variablesService.ProjectsToPublish?.FirstOrDefault(x => x.Name == projectName)?.IsSelected ?? false,
                    Name = projectName,
                    Path = Path.Combine(directory, fileName)
                });
            }
        }
        
        private string searchField;

        public string SearchField
        {
            get => searchField;
            set
            {
                searchField = value;

                var itemsViewOriginal =
                    (CollectionView) CollectionViewSource.GetDefaultView(mainWindowViewModel.SelectProjectsView.Projects
                        .ItemsSource);
                itemsViewOriginal.Filter = o => string.IsNullOrEmpty(searchField) ||
                                                ((PublishProject) o).Name != null && ((PublishProject) o).Name
                                                .ToLower()
                                                .Contains(searchField.ToLower());
                itemsViewOriginal.Refresh();

                OnPropertyChanged(nameof(SearchField));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
