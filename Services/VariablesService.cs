using System;
using Publisher.Models;
using System.Collections.Generic;
using System.IO;

namespace Publisher.Services
{
    public class VariablesService
    {
        public string PathToProjects { get; set; } = @"E:\net-projects\job\evj\evj";

        public string PathToPublish { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "Publish");

        public List<PublishProject> ProjectsToPublish { get; set; }

        public VariablesService()
        {

        }
    }
}
