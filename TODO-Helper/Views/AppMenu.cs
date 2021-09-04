using System.Windows;

namespace TODO_Helper.Views
{
    public class AppMenu
    {
        private readonly MainWindow _mainWindow;

        public AppMenu(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        /// <summary>
        /// Sets the visibility of Tooltips for the menu.
        /// </summary>
        public void SetTooltipVisibility()
        {
            // Set tooltip visibility

            if (_mainWindow.TgBtnMenu.IsChecked == true)
            {
                _mainWindow.TtHome.Visibility = Visibility.Collapsed;
                _mainWindow.TtMyTasks.Visibility = Visibility.Collapsed;
                _mainWindow.TtSettings.Visibility = Visibility.Collapsed;
                _mainWindow.TtInfo.Visibility = Visibility.Collapsed;
            }
            else
            {
                _mainWindow.TtHome.Visibility = Visibility.Visible;
                _mainWindow.TtMyTasks.Visibility = Visibility.Visible;
                _mainWindow.TtSettings.Visibility = Visibility.Visible;
                _mainWindow.TtInfo.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Opens the Homescreen.
        /// </summary>
        public void OpenHomeScreen()
        {
            _mainWindow.HomePnl.Visibility = Visibility.Visible;
            _mainWindow.TasksPnl.Visibility = Visibility.Collapsed;
            _mainWindow.SettingsPnl.Visibility = Visibility.Collapsed;
            _mainWindow.InfoPnl.Visibility = Visibility.Collapsed;
            _mainWindow.ProjectPnl.Visibility = Visibility.Collapsed;
            _mainWindow.CurrentTaskPbl.Visibility = Visibility.Collapsed;
            _mainWindow.ProjectView.CurrentProject = null;
        }

        /// <summary>
        /// Opens the Taskscreen.
        /// </summary>
        public void OpenTaskScreen()
        {
            _mainWindow.HomePnl.Visibility = Visibility.Collapsed;
            _mainWindow.TasksPnl.Visibility = Visibility.Visible;
            _mainWindow.SettingsPnl.Visibility = Visibility.Collapsed;
            _mainWindow.InfoPnl.Visibility = Visibility.Collapsed;
            _mainWindow.ProjectPnl.Visibility = Visibility.Collapsed;
            _mainWindow.CurrentTaskPbl.Visibility = Visibility.Collapsed;
            _mainWindow.ProjectView.CurrentProject = null;
        }

        /// <summary>
        /// Opens the Settingsscreen.
        /// </summary>
        public void OpenSettingsScreen()
        {
            _mainWindow.HomePnl.Visibility = Visibility.Collapsed;
            _mainWindow.TasksPnl.Visibility = Visibility.Collapsed;
            _mainWindow.SettingsPnl.Visibility = Visibility.Visible;
            _mainWindow.InfoPnl.Visibility = Visibility.Collapsed;
            _mainWindow.ProjectPnl.Visibility = Visibility.Collapsed;
            _mainWindow.CurrentTaskPbl.Visibility = Visibility.Collapsed;
            _mainWindow.ProjectView.CurrentProject = null;
        }

        /// <summary>
        /// Opens the Infoscreen.
        /// </summary>
        public void OpenInfoScreen()
        {
            _mainWindow.HomePnl.Visibility = Visibility.Collapsed;
            _mainWindow.TasksPnl.Visibility = Visibility.Collapsed;
            _mainWindow.SettingsPnl.Visibility = Visibility.Collapsed;
            _mainWindow.InfoPnl.Visibility = Visibility.Visible;
            _mainWindow.ProjectPnl.Visibility = Visibility.Collapsed;
            _mainWindow.CurrentTaskPbl.Visibility = Visibility.Collapsed;
            
            _mainWindow.ProjectView.CurrentProject = null;
        }

        /// <summary>
        /// Open the Projectscreen.
        /// </summary>
        public void OpenProjectScreen()
        {
            _mainWindow.HomePnl.Visibility = Visibility.Collapsed;
            _mainWindow.TasksPnl.Visibility = Visibility.Collapsed;
            _mainWindow.SettingsPnl.Visibility = Visibility.Collapsed;
            _mainWindow.InfoPnl.Visibility = Visibility.Collapsed;
            _mainWindow.ProjectPnl.Visibility = Visibility.Visible;
            _mainWindow.CurrentTaskPbl.Visibility = Visibility.Collapsed;
        }

        public void OpenCurrentTaskInProjectScreen()
        {
            if (_mainWindow.ProjectPnl.Visibility != Visibility.Visible)
                return;
            
            _mainWindow.CurrentTaskPbl.Visibility = Visibility.Visible;
        }
    }
}
