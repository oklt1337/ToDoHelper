using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TODO_Helper.DomainModels;
using TODO_Helper.Persistence;
using TODO_Helper.Views;

namespace TODO_Helper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public Dictionary<string, Project> Projects { get; }

        public readonly AppMenu AppMenu;
        public readonly Home Home;
        public readonly ProjectView ProjectView;
        public readonly TaskView TaskView;

        public MainWindow()
        {
            Projects = ProjectDeserializer.LoadProjects();
            InitializeComponent();
            AppMenu = new AppMenu(this);
            ProjectView = new ProjectView(this);
            TaskView = new TaskView(this);
            Home = new Home(this);
            Home.SetProjectIcons(Projects.Values.ToList());
            Home.LoadProjects(Projects.Values.ToList());
        }

        #region Application Functions

        private void OnWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
            ProjectSerializer.SaveAllProjects(Projects.Values.ToList());
        }

        private void OnClickMinimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        #endregion

        #region Menu Functions

        private void ListViewItem_MouseEnter(object sender, MouseEventArgs e)
        {
            AppMenu.SetTooltipVisibility();
        }

        private void OnClickHome(object sender, RoutedEventArgs e)
        {
            AppMenu.OpenHomeScreen();
        }

        private void OnClickMyTask(object sender, RoutedEventArgs e)
        {
            AppMenu.OpenTaskScreen();
        }

        private void OnClickSettings(object sender, RoutedEventArgs e)
        {
            AppMenu.OpenSettingsScreen();
        }

        private void OnClickInfo(object sender, RoutedEventArgs e)
        {
            AppMenu.OpenInfoScreen();
        }

        #endregion

        #region Home Functions

        private void OnClickAddProject(object sender, RoutedEventArgs e)
        {
            var project = Home.GetNewProject();
            if (project == null)
                return;

            Projects.Add(project.ProjectName, project);
        }

        private void OnClickRAP(object sender, RoutedEventArgs e)
        {
            Home.ChangeTgButtonImage(ref TgBtnRap);
        }

        private void OnClickClosestTask(object sender, RoutedEventArgs e)
        {
            Home.ChangeTgButtonImage(ref TgBtnClosestTask);
        }

        #endregion

        #region ProjectPanel

        private void OnDropLvPToDo(object sender, DragEventArgs e)
        {
            var formats = e.Data.GetFormats();
            if (formats.Length != 1 || formats[0] != "TODO_Helper.DomainModels.Task") 
                return;
            if (e.Data.GetData(formats[0]) is not Task task) 
                return;
            
            ProjectView.DeleteTaskItem(task);
            task.State = State.ToDo;
            ProjectView.CreateNewProjectTaskItem(task, LvPToDo);
        }

        private void OnDropLvPInProgress(object sender, DragEventArgs e)
        {
            var formats = e.Data.GetFormats();
            if (formats.Length != 1 || formats[0] != "TODO_Helper.DomainModels.Task") 
                return;
            if (e.Data.GetData(formats[0]) is not Task task) 
                return;
                
            ProjectView.DeleteTaskItem(task);
            task.State = State.InProgress;
            ProjectView.CreateNewProjectTaskItem(task, LvPInProgress);
        }

        private void OnDropLvPDone(object sender, DragEventArgs e)
        {
            var formats = e.Data.GetFormats();
            if (formats.Length != 1 || formats[0] != "TODO_Helper.DomainModels.Task") 
                return;
            if (e.Data.GetData(formats[0]) is not Task task) 
                return;
              
            ProjectView.DeleteTaskItem(task);
            task.State = State.Done;
            ProjectView.CreateNewProjectTaskItem(task, LvPDone);
        }
        
        private void OnDragOverLvPToDo(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            if (e.Data.GetFormats()[0] == "TODO_Helper.DomainModels.Task")
            {
                e.Effects = DragDropEffects.Move;
            }

            e.Handled = true;
        }

        private void OnDragOverLvPInProgress(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            if (e.Data.GetFormats()[0] == "TODO_Helper.DomainModels.Task")
            {
                e.Effects = DragDropEffects.Move;
            }

            e.Handled = true;
        }

        private void OnDragOverLvPLvPDone(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            if (e.Data.GetFormats()[0] == "TODO_Helper.DomainModels.Task")
            {
                e.Effects = DragDropEffects.Move;
            }

            e.Handled = true;
        }
        
        private void OnClickDelProject(object sender, RoutedEventArgs e)
        {
            if (ProjectView.CurrentProject == null)
                return;
            
            foreach (var rapItems in LvRap.Items)
            {
                if (rapItems is not ListViewItem item) 
                    continue;
                if (item.Name != ProjectView.CurrentProject.ProjectName) 
                    continue;
                
                LvRap.Items.Remove(item);
                break;
            }
            Projects.Remove(ProjectView.CurrentProject.ProjectName);
            ProjectSerializer.DeleteProjectSaveFile(ProjectView.CurrentProject);
            ProjectView.CurrentProject = null;
            Home.LoadProjects(Projects.Values.ToList());
            AppMenu.OpenHomeScreen();
        }
        
        private void OnClickCloseTaskDetails(object sender, RoutedEventArgs e)
        {
            CurrentTaskPbl.Visibility = Visibility.Collapsed;
            TaskView.CurrentTask = null;
        }

        #endregion
    }
}
