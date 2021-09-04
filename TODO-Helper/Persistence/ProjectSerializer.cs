using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Project = TODO_Helper.DomainModels.Project;

namespace TODO_Helper.Persistence
{
    public static class ProjectSerializer
    {
        public static void SaveProject(Project project)
        {
            var settingsFolderPath =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TodoHelper");
            CreateSettingsFolder(settingsFolderPath);
            File.WriteAllText(Path.Combine(settingsFolderPath, project.ProjectName + ".json"),
                JsonConvert.SerializeObject(project, Formatting.Indented));
        }

        public static void SaveAllProjects(List<Project> projects)
        {
            foreach (var project in projects)
            {
                SaveProject(project);
            }
        }

        public static void DeleteProjectSaveFile(Project project)
        {
            var settingsFolderPath =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TodoHelper");
            
            File.Delete(Path.Combine(settingsFolderPath, project.ProjectName + ".json"));
        }

        private static void CreateSettingsFolder(string path)
        {
            if (Directory.Exists(path))
                return;

            Directory.CreateDirectory(path);
        }
    }
}