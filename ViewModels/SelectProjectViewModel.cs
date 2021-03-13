using System;
using System.Collections.Generic;
using Publisher.Models;
using Publisher.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Linq;

namespace Publisher.ViewModels
{
    public class SelectProjectViewModel : INotifyPropertyChanged
    {
        private readonly VariablesService variablesService;
        private readonly MainWindowViewModel mainWindowViewModel;

        public ObservableCollection<PublishProject> PublishProjects { get; set; }
        public ListCollectionView CollectionView { get; set; }

        public SelectProjectViewModel(VariablesService variablesService, MainWindowViewModel mainWindowViewModel)
        {
            this.variablesService = variablesService;
            this.mainWindowViewModel = mainWindowViewModel;
            PublishProjects = new ObservableCollection<PublishProject>();
        }

        public void GetAllProjects()
        {
            PublishProjects.Clear();
            foreach (var directory in Directory.GetDirectories(variablesService.PathToProjects))
            {
                if (directory.ToLower().Contains("test"))
                    continue;

                var directoryInfo = new DirectoryInfo(Path.Combine(variablesService.PathToProjects, directory));
                var files = directoryInfo.GetFiles("*.csproj");

                if (files.Length <= 0) continue;

                var fileName = files[0].Name;
                var projectName = fileName.Remove(fileName.Length - 7);
                var fullPathName = Path.Combine(directory, fileName);
                
                PublishProjects.Add(new PublishProject
                {
                    IsSelected = variablesService.ProjectsToPublish?.FirstOrDefault(x => x.Name == projectName)?.IsSelected ?? false,
                    Name = projectName,
                    Path = fullPathName,
                    ProjectType = GetProjectType(fullPathName)
                });
            }
        }

        private PublishProjectType GetProjectType(string fullPathName)
        {
            var projectDefinition = XDocument.Load(fullPathName);
            var sdk = projectDefinition.Elements().First().FirstAttribute.Value;

            var projectType = sdk switch
            {
                "Microsoft.NET.Sdk.Web" => PublishProjectType.Web,
                "Microsoft.NET.Sdk" => PublishProjectType.Lib,
                "Microsoft.NET.Sdk.Worker" => PublishProjectType.Worker,
                _ => PublishProjectType.Other
            };

            return projectType;
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
