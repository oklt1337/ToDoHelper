using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TODO_Helper.DomainModels;

namespace TODO_Helper.Persistence
{
    public static class ProjectDeserializer
    {
        public static Dictionary<string, Project> LoadProjects()
        {
            var settingsFolderPath =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TodoHelper");

            if (!Directory.Exists(settingsFolderPath) || Directory.GetFiles(settingsFolderPath).Length == 0)
                return new Dictionary<string, Project>();

            return Directory.GetFiles(settingsFolderPath)
                .Select(LoadProject)
                .ToDictionary(
                    project => project.ProjectName, 
                    project => project
                );
        }

        private static Project LoadProject(string path)
        {
            return JsonConvert.DeserializeObject<Project>(File.ReadAllText(path));
        }
    }
}
