using System;
using Publisher.Models;
using Publisher.Services;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace Publisher.ViewModels
{
    public class ProgressBarViewModel : INotifyPropertyChanged
    {
        private readonly VariablesService variablesService;
        private readonly MainWindowViewModel mainWindowViewModel;
        private readonly BackgroundWorker backgroundWorker;
        private readonly PublishSettings settings;

        public ProgressBarViewModel(VariablesService variablesService, MainWindowViewModel mainWindowViewModel)
        {
            this.variablesService = variablesService;
            this.mainWindowViewModel = mainWindowViewModel;

            var settingsJson = File.ReadAllText("appsettings.json", Encoding.UTF8);
            settings = JsonSerializer.Deserialize<PublishSettings>(settingsJson);
            backgroundWorker = new BackgroundWorker();
        }

        private int progress;
        public int Progress
        {
            get => progress;
            set
            {
                progress = value;
                OnPropertyChanged(nameof(Progress));
            }
        }

        private string progressText;
        public string ProgressText
        {
            get => progressText;
            set
            {
                progressText = value;
                OnPropertyChanged(nameof(ProgressText));
            }
        }

        public void PublishProjects()
        {
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.DoWork += BackgroundWorker_PublishProjects;
            backgroundWorker.RunWorkerAsync();
        }

        private void BackgroundWorker_PublishProjects(object sender, DoWorkEventArgs e)
        {
            var projects = variablesService.ProjectsToPublish.Where(p => p.IsSelected).ToList();
            int progressValue = 0;
            int projectsCount = projects.Count;
            if (projectsCount == 0)
            {
                MessageBox.Show("Не выбраны проекты.");
                return;
            }
            int diff = 100 / projectsCount;
            for (var i = 0; i < projectsCount; i++)
            {
                var project = projects[i];
                string outputDir = Path.Combine(variablesService.PathToPublish, project.Name);
                if (Directory.Exists(outputDir))
                {
                    var di = new DirectoryInfo(outputDir);
                    foreach (var file in di.GetFiles())
                        file.Delete();
                    foreach (var dir in di.GetDirectories())
                        dir.Delete(true);
                }
                else
                {
                    Directory.CreateDirectory(outputDir);
                }

                using var process = new Process
                {
                    StartInfo =
                    {
                        UseShellExecute = false,
                        FileName = "dotnet.exe",
                        Arguments = $"{settings.PublishCommand} {project.Path} --output {outputDir}",
                        CreateNoWindow = true
                    }
                };
                backgroundWorker.ReportProgress(0, project.Name);
                process.Start();
                process.WaitForExit();
                progressValue += diff;
                backgroundWorker.ReportProgress(progressValue);
            }
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (Progress < e.ProgressPercentage) Progress = e.ProgressPercentage;
            if (e.UserState != null)
            {
                ProgressText = (string)e.UserState;
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Progress = 100;
            mainWindowViewModel.NavigateFinalView();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
