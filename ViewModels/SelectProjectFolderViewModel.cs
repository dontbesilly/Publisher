using Publisher.Helpers;
using Publisher.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Input;

namespace Publisher.ViewModels
{
    public class SelectProjectFolderViewModel : INotifyPropertyChanged
    {
        private readonly VariablesService variablesService;

        public SelectProjectFolderViewModel(VariablesService variablesService)
        {
            this.variablesService = variablesService;
            projectDir = variablesService.PathToProjects;
        }

        private string projectDir;
        public string ProjectDir
        {
            get => projectDir;
            set
            {
                projectDir = value;
                OnPropertyChanged(nameof(ProjectDir));
            }
        }

        public ICommand ChangeFolderCommand => new RelayCommand(ChangeFolderCommandExecuted);

        private void ChangeFolderCommandExecuted(object obj)
        {
            var dialog = new FolderBrowserDialog { SelectedPath = projectDir };
            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                ProjectDir = dialog.SelectedPath;
                variablesService.PathToProjects = ProjectDir;
                variablesService.UpdateSettings();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
