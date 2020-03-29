using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;

namespace Publisher.ViewModels
{
    public class SelectProjectViewModel
    {
        private readonly string pathToSln;

        public List<PublishProject> PublishProjects { get; set; }

        public SelectProjectViewModel(string pathToSln)
        {
            this.pathToSln = pathToSln;
            PublishProjects = new List<PublishProject>();

            GetAllProjects();
        }

        public ICommand NextCommand => new RelayCommand(NextCommandExecuted);

        private void NextCommandExecuted(object obj)
        {
            Console.WriteLine();
        }

        private void GetAllProjects()
        {
            foreach (var directory in Directory.GetDirectories(pathToSln))
            {
                var directoryInfo = new DirectoryInfo(Path.Combine(pathToSln, directory));
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
    }

    public class PublishProject
    {
        public bool IsSelected { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

    }
}
