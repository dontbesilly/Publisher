using System.Windows.Input;

namespace Publisher.Helpers
{
    public static class NavigationCommands
    {
        public static RoutedCommand OpenSelectProjectView = new RoutedCommand();
        public static RoutedCommand OpenSelectProjectFolderView = new RoutedCommand();
        public static RoutedCommand OpenSelectPublishFolderView = new RoutedCommand();
        public static RoutedCommand OpenProgressBarView = new RoutedCommand();
        public static RoutedCommand OpenProgressBarMigrationsView = new RoutedCommand();
    }
}
