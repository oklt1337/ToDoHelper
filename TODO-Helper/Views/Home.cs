using System;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Microsoft.WindowsAPICodePack.Dialogs;
using TODO_Helper.DomainModels;
using TODO_Helper.Functions;
using TODO_Helper.Persistence;

namespace TODO_Helper.Views
{
    public class Home
    {
        private readonly MainWindow _mainWindow;
        
        public Home(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public Project GetNewProject()
        {
            var ofd = new CommonOpenFileDialog {IsFolderPicker = true};
            if (ofd.ShowDialog() != CommonFileDialogResult.Ok) 
                return default;

            var project = new Project
            {
                ProjectName = ofd.FileName.Substring(ofd.FileName.LastIndexOf("\\", StringComparison.Ordinal) + 1),
                Root = ofd.FileName,
                Tasks = new List<Task>(),
            };
            project.ProjectIcon = GetIcon(project.ProjectIconRecourse);

            if (_mainWindow.Projects.ContainsKey(project.ProjectName))
            {
                return null;
            }

            ProjectScanner.GetInstance().ScanProject(project);
            CreateNewProjectItem(project);
            ProjectSerializer.SaveProject(project);
            
            _mainWindow.ProjectView.LoadProject(project);
            
            return project;
        }

        public void ChangeTgButtonImage(ref ToggleButton tgBtn)
        {
            tgBtn.Background = tgBtn.IsChecked == true ? _mainWindow.Resources["tb_DropDown_Checked"] as ImageBrush : _mainWindow.Resources["tb_DropDown_UnChecked"]  as ImageBrush;
        }

        private void CreateNewProjectItem(Project pj)
        {
            _mainWindow.LvRap.Items.Add(CreateItems.CreateProjectViewItem(pj, _mainWindow.ProjectView.OnClickOpenProject));
        }

        public void LoadProjects(List<Project> projects)
        {
            projects.ForEach(p => _mainWindow.LvRap.Items.Add(CreateItems.CreateProjectViewItem(p, _mainWindow.ProjectView.OnClickOpenProject)));
        }
        
        public void SetProjectIcons(List<Project> projects)
        {
            foreach (var project in projects)
            {
                project.ProjectIcon = GetIcon(project.ProjectIconRecourse);
            }
        }
        
        private ImageBrush GetIcon(string resourceString)
        {
            if (!string.IsNullOrEmpty(resourceString)) 
                return _mainWindow.Resources[resourceString] as ImageBrush;
            
            var random = new Random();
            var index = random.Next(1, 5);
            return index switch
            {
                1 => _mainWindow.Resources["ProjectFireImg"] as ImageBrush,
                2 => _mainWindow.Resources["ProjectAirImg"] as ImageBrush,
                3 => _mainWindow.Resources["ProjectAirImg"] as ImageBrush,
                _ => _mainWindow.Resources["ProjectAirImg"] as ImageBrush
            };
        }
    }
}
