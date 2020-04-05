using Publisher.Services;
using Publisher.ViewModels;
using Publisher.Views;
using System;
using System.Linq;
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

        public SelectPublishFolderViewModel SelectPublishFolderViewModel { get; set; }
        public SelectPublishFolderView SelectPublishFolderView { get; set; }

        public ProgressBarViewModel ProgressBarViewModel { get; set; }
        public ProgressBarView ProgressBarView { get; set; }

        public ZipViewModel ZipViewModel { get; set; }
        public ZipView ZipView { get; set; }

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

            SelectPublishFolderViewModel = new SelectPublishFolderViewModel(VariablesService);
            SelectPublishFolderView = new SelectPublishFolderView { DataContext = SelectPublishFolderViewModel };

            ProgressBarViewModel = new ProgressBarViewModel(VariablesService);
            ProgressBarView = new ProgressBarView { DataContext = ProgressBarViewModel };
        }

        private void InitializeNavigationCommands()
        {
            CommandManager.RegisterClassCommandBinding(typeof(MainWindow),
                new CommandBinding(NavigationCommands.OpenSelectProjectView, OpenSelectProjectViewCommandExecuted));
            CommandManager.RegisterClassCommandBinding(typeof(MainWindow),
                new CommandBinding(NavigationCommands.OpenSelectProjectFolderView, OpenSelectProjectFolderViewCommandExecuted));
            CommandManager.RegisterClassCommandBinding(typeof(MainWindow),
                new CommandBinding(NavigationCommands.OpenSelectPublishFolderView, OpenSelectPublishFolderViewCommandExecuted));
            CommandManager.RegisterClassCommandBinding(typeof(MainWindow),
                new CommandBinding(NavigationCommands.OpenProgressBarView, OpenProgressBarViewCommandExecuted));
        }

        private void OpenProgressBarViewCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MainWindow.FrameBody.NavigationService.Navigate(ProgressBarView);
            ProgressBarViewModel.PublishProjects();
        }

        private void OpenSelectPublishFolderViewCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            VariablesService.ProjectsToPublish = SelectProjectViewModel.PublishProjects.Where(p => p.IsSelected).ToList();
            MainWindow.FrameBody.NavigationService.Navigate(SelectPublishFolderView);
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
