using Publisher.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace Publisher.ViewModels
{
    public class SelectProjectViewModel : INotifyPropertyChanged
    {
        private readonly VariablesService variablesService;

        public ObservableCollection<PublishProject> PublishProjects { get; set; }

        public SelectProjectViewModel(VariablesService variablesService)
        {
            this.variablesService = variablesService;
            PublishProjects = new ObservableCollection<PublishProject>();
        }

        public void GetAllProjects()
        {
            PublishProjects = new ObservableCollection<PublishProject>();
            foreach (string directory in Directory.GetDirectories(variablesService.PathToProjects))
            {
                if (directory.ToLower().Contains("test"))
                    continue;

                var directoryInfo = new DirectoryInfo(Path.Combine(variablesService.PathToProjects, directory));
                var files = directoryInfo.GetFiles("*.csproj");

                if (files.Length <= 0) continue;

                var fileName = files[0].Name;
                PublishProjects.Add(new PublishProject
                {
                    IsSelected = false,
                    Name = fileName.Remove(fileName.Length - 7),
                    Path = directory
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class PublishProject
    {
        public bool IsSelected { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

    }
}
