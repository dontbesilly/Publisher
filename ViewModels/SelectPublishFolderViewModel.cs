using Publisher.Helpers;
using Publisher.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Input;

namespace Publisher.ViewModels
{
    public class SelectPublishFolderViewModel : INotifyPropertyChanged
    {
        private readonly VariablesService variablesService;

        public SelectPublishFolderViewModel(VariablesService variablesService)
        {
            this.variablesService = variablesService;
            publishDir = variablesService.PathToPublish;
        }

        private string publishDir;
        public string PublishDir
        {
            get => publishDir;
            set
            {
                publishDir = value;
                OnPropertyChanged(nameof(PublishDir));
            }
        }

        public ICommand ChangeFolderCommand => new RelayCommand(ChangeFolderCommandExecuted);

        private void ChangeFolderCommandExecuted(object obj)
        {
            var dialog = new FolderBrowserDialog { SelectedPath = publishDir };
            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                PublishDir = dialog.SelectedPath;
                variablesService.PathToPublish = PublishDir;
                variablesService.UpdateSettings();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
