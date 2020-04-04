using Publisher.Services;
using Publisher.ViewModels;
using Publisher.Views;
using System;
using System.Windows;
using System.Windows.Input;
using NavigationCommands = Publisher.Helpers.NavigationCommands;

namespace Publisher
{
    public class MainWindowViewModel
    {
        public MainWindow MainWindow { get; set; }

        public VariablesService VariablesService { get; set; }

        public SelectProjectFolderView SelectProjectFolderView { get; set; }
        public SelectProjectsView SelectProjectsView { get; set; }

        public SelectProjectFolderViewModel SelectProjectFolderViewModel { get; set; }
        public SelectProjectViewModel SelectProjectViewModel { get; set; }

        public MainWindowViewModel()
        {
            InitializeServices();
            InitializeViews();
            InitializeNavigationCommands();
        }

        private void InitializeServices()
        {
            VariablesService = new VariablesService();
        }

        private void InitializeViews()
        {
            SelectProjectFolderViewModel = new SelectProjectFolderViewModel(VariablesService);
            SelectProjectFolderView = new SelectProjectFolderView { DataContext = SelectProjectFolderViewModel };

            SelectProjectViewModel = new SelectProjectViewModel(VariablesService);
            SelectProjectsView = new SelectProjectsView { DataContext = SelectProjectViewModel };
        }

        private void InitializeNavigationCommands()
        {
            CommandManager.RegisterClassCommandBinding(typeof(MainWindow),
                new CommandBinding(NavigationCommands.OpenSelectProjectView, OpenSelectProjectViewCommandExecuted));
            CommandManager.RegisterClassCommandBinding(typeof(MainWindow),
                new CommandBinding(NavigationCommands.OpenSelectProjectFolderView, OpenSelectProjectFolderViewCommandExecuted));
        }

        private void OpenSelectProjectFolderViewCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindow.FrameBody.NavigationService.Navigate(SelectProjectFolderView);
        }

        private void OpenSelectProjectViewCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SelectProjectViewModel.GetAllProjects();
            MainWindow.FrameBody.NavigationService.Navigate(SelectProjectsView);
        }

        public void InitMainWindow(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            MainWindow.Show();
            MainWindow.FrameBody.NavigationService.Navigate(SelectProjectFolderView);
        }
    }
}
