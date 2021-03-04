using Prism.Navigation;
using ProfileBook.Models;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProfileBook.ViewModels
{
    public class ModalViewModel : BaseViewModel
    {
        public ModalViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        #region --- Public Properties ---

        public ICommand GoBackTapCommand => new Command(OnGoBackTap);

        private ImageSource _pictupeSource;
        public ImageSource PictureSource
        {
            get => _pictupeSource;
            set => SetProperty(ref _pictupeSource, value);
        }

        #endregion

        #region --- Private Helpers ---

        private async void OnGoBackTap()
        {
            await navigationService.GoBackAsync(useModalNavigation: true);
        }

        #endregion

        #region --- Overrides ---

        public override void Initialize(INavigationParameters parameters)
        {
            if (parameters.TryGetValue("profile", out ProfileModel value))
            {
                PictureSource = ImageSource.FromFile(value.ImagePath);
            }
        }

        #endregion
    }
}
