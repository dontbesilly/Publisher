using LiteDB;
using Publisher.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Publisher.Services
{
    public class VariablesService
    {
        public int Id { get; set; }

        public string PathToProjects { get; set; }

        public string PathToPublish { get; set; }

        public List<PublishProject> ProjectsToPublish { get; set; }

        public VariablesService()
        {
            ProjectsToPublish = new List<PublishProject>();
        }

        public void LoadSettings()
        {
            using (var rep = new LiteRepository(@"settings.db"))
            {
                var collection = rep.Database.GetCollection<VariablesService>("variables");

                if (collection.Count() == 0)
                {
                    PathToPublish = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "Publish");
                    collection.Insert(this);
                }
                else
                {
                    var settings = collection.FindAll().First();
                    PathToProjects = settings.PathToProjects;
                    PathToPublish = settings.PathToPublish;
                }
            }
        }

        public void UpdateSettings()
        {
            using (var rep = new LiteRepository(@"settings.db"))
            {
                var collection = rep.Database.GetCollection<VariablesService>("variables");

                foreach (var variablesService in collection.FindAll())
                {
                    variablesService.PathToProjects = PathToProjects;
                    variablesService.PathToPublish = PathToPublish;
                    collection.Update(variablesService);
                }
            }
        }
    }
}
