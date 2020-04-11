using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Publisher.Helpers;
using Publisher.Services;

namespace Publisher.ViewModels
{
    public class ZipViewModel : INotifyPropertyChanged
    {
        private readonly VariablesService variablesService;
        private readonly MainWindowViewModel mainWindowViewModel;

        public ZipViewModel(VariablesService variablesService, MainWindowViewModel mainWindowViewModel)
        {
            this.variablesService = variablesService;
            this.mainWindowViewModel = mainWindowViewModel;

            zipEachOne = variablesService.ZipEachOne;
            zipAll = variablesService.ZipAll;
        }

        private bool zipEachOne;
        public bool ZipEachOne
        {
            get => zipEachOne;
            set
            {
                zipEachOne = value;

                if (zipEachOne)
                    ZipAll = false;

                variablesService.ZipEachOne = zipEachOne;
                variablesService.UpdateSettings();
                OnPropertyChanged(nameof(ZipEachOne));
            }
        }

        private bool zipAll;
        public bool ZipAll
        {
            get => zipAll;
            set
            {
                zipAll = value;

                if (zipAll)
                    ZipEachOne = false;

                variablesService.ZipAll = zipAll;
                variablesService.UpdateSettings();
                OnPropertyChanged(nameof(ZipAll));
            }
        }

        public ICommand NextCommand => new RelayCommand(NextCommandExecuted);

        private void NextCommandExecuted(object obj)
        {
            if (ZipEachOne || ZipAll)
            {
                mainWindowViewModel.ZipFiles();
            }
            else
            {
                mainWindowViewModel.NavigateFinalView();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
