using System;
using System.Diagnostics;
using System.IO;
using Publisher.Models;
using Publisher.Services;
using System.Text;
using System.Text.Json;

namespace Publisher.ViewModels
{
    public class ProgressBarViewModel
    {
        private readonly VariablesService variablesService;

        public ProgressBarViewModel(VariablesService variablesService)
        {
            this.variablesService = variablesService;
        }

        public void PublishProjects()
        {
            var settingsJson = File.ReadAllText("appsettings.json", Encoding.UTF8);
            var settings = JsonSerializer.Deserialize<PublishSettings>(settingsJson);

            foreach (var project in variablesService.ProjectsToPublish)
            {
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
                process.Start();
            }
        }
    }
}
