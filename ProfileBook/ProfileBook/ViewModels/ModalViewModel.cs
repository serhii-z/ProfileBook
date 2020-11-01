using Prism.Mvvm;
using Prism.Navigation;
using ProfileBook.Models;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProfileBook.ViewModels
{
    public class ModalViewModel : BindableBase, IInitialize
    {
        private INavigationService _navigationService;
        public ModalViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private ImageSource _pictupeSource;
        public ImageSource PictureSource
        {
            get => _pictupeSource;
            set => SetProperty(ref _pictupeSource, value);
        }

        public ICommand GoBackCommand => new Command(GoToBack);

        private async void GoToBack()
        {
            await _navigationService.GoBackAsync(useModalNavigation: true);
        }

        public void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("profile", out Profile value))
            {
                PictureSource = ImageSource.FromFile(value.ImagePath);
            }
        }
    }
}
