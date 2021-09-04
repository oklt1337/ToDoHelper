using System;
using System.Windows;
using System.Windows.Controls;
using TODO_Helper.DomainModels;
using TODO_Helper.Functions;
using Button = System.Windows.Controls.Button;
using ListViewItem = System.Windows.Controls.ListViewItem;

namespace TODO_Helper.Views
{
    public class ProjectView
    {
        private readonly MainWindow _mainWindow;
        public Project CurrentProject;

        public ProjectView(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public void OnClickOpenProject(object sender, RoutedEventArgs args) {
            if (sender is not Button button)
                return;

            var project = _mainWindow.Projects[(string)button.Tag];
            LoadProject(project);
        }

        public void LoadProject(Project project)
        {
            CurrentProject = project;

            _mainWindow.TblProjectInfo.Text = project.ProjectName;

            _mainWindow.LvPToDo.Items.Clear();
            _mainWindow.LvPInProgress.Items.Clear();
            _mainWindow.LvPDone.Items.Clear();
            foreach (var task in project.Tasks)
            {
                var listView = task.State switch
                {
                    State.Done => _mainWindow.LvPDone,
                    State.InProgress => _mainWindow.LvPInProgress,
                    _ => _mainWindow.LvPToDo
                };
                CreateNewProjectTaskItem(task, listView);
            }
            _mainWindow.ImgProject.Source = project.ProjectIcon.ImageSource;
            _mainWindow.AppMenu.OpenProjectScreen();
        }

        public void CreateNewProjectTaskItem(Task task, ItemsControl listView)
        {
            listView.Items.Add(CreateItems.CreateTaskItem(task, _mainWindow.TaskView.OnClickOpenTask));
        }

        public void DeleteTaskItem(Task task)
        {
            switch (task.State)
            {
                case State.ToDo:
                    foreach (var item in _mainWindow.LvPToDo.Items)
                    {
                        if (item is not ListViewItem lvItem)
                            continue;
                        if ((string)lvItem.Tag != task.Title)
                            continue;

                        _mainWindow.LvPToDo.Items.Remove(item);
                        _mainWindow.LvPToDo.UpdateLayout();
                        return;
                    }

                    break;
                case State.InProgress:
                    foreach (var item in _mainWindow.LvPInProgress.Items)
                    {
                        if (item is not ListViewItem lvItem)
                            continue;
                        if ((string)lvItem.Tag != task.Title)
                            continue;

                        _mainWindow.LvPInProgress.Items.Remove(item);
                        _mainWindow.LvPInProgress.UpdateLayout();
                        return;
                    }

                    break;
                case State.Done:
                    foreach (var item in _mainWindow.LvPDone.Items)
                    {
                        if (item is not ListViewItem lvItem)
                            continue;
                        if ((string)lvItem.Tag != task.Title)
                            continue;

                        _mainWindow.LvPDone.Items.Remove(item);
                        _mainWindow.LvPDone.UpdateLayout();
                        return;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
