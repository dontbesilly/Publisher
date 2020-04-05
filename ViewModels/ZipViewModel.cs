using Publisher.Services;

namespace Publisher.ViewModels
{
    public class ZipViewModel
    {
        private readonly VariablesService variablesService;

        public ZipViewModel(VariablesService variablesService)
        {
            this.variablesService = variablesService;
        }
    }
}
