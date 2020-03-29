using Publisher.Views;
using System;
using System.Windows;
using Publisher.ViewModels;

namespace Publisher
{
    public class MainWindowViewModel
    {
        public MainWindow MainWindow { get; set; }

        public SelectProjectsView SelectProjectsView { get; set; }

        public MainWindowViewModel()
        {
            InitializeViews();
            InitializeNavigationCommands();
        }

        private void InitializeViews()
        {
            SelectProjectsView = new SelectProjectsView { DataContext = new SelectProjectViewModel(@"E:\net-projects\job\evj\back") };
        }

        private void InitializeNavigationCommands()
        {
            // TODO
        }

        public void InitMainWindow(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            MainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            MainWindow.Show();
            MainWindow.FrameBody.NavigationService.Navigate(SelectProjectsView);
        }
    }
}
