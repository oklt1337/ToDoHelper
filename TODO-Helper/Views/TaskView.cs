using System.Windows;
using System.Windows.Controls;
using TODO_Helper.DomainModels;

namespace TODO_Helper.Views
{
    public class TaskView
    {
        private readonly MainWindow _mainWindow;
        public Task CurrentTask;

        public TaskView(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public void OnClickOpenTask(object sender, RoutedEventArgs args)
        {
            if (sender is not Button button)
                return;
            
            var index = _mainWindow.ProjectView.CurrentProject.Tasks.FindIndex(x=> x.Title == (string)button.Tag);
            if (index != -1)
            {
                var task = _mainWindow.ProjectView.CurrentProject.Tasks[index];
                LoadTask(task);
            }
        }

        private void LoadTask(Task task)
        {
            CurrentTask = task;

            _mainWindow.CurrentTaskName.Content = task.Title;
            _mainWindow.CurrentTaskAssignee.Text = task.Assignee;
            _mainWindow.CurrentTaskDueDate.Text = task.DueDate.ToString("m");
            _mainWindow.CurrentTaskProject.Text = task.ParentFile;
            _mainWindow.CurrentTaskPriority.Text = task.Priority.ToString();
            _mainWindow.CurrentTaskDescription.Text = task.Description;

            _mainWindow.AppMenu.OpenCurrentTaskInProjectScreen();
        }
    }
}
