using ProfileBook.Resources.Themes;
using Xamarin.Essentials;

namespace ProfileBook.Servises.Settings
{
    public class SettingsManager : ISettingsManager
    { 
        public string SortingName
        {
            get => Preferences.Get(nameof(SortingName), string.Empty);
            set => Preferences.Set(nameof(SortingName), value);
        }

        public string ThemeName
        {
            get => Preferences.Get(nameof(ThemeName), nameof(LightTheme));
            set => Preferences.Set(nameof(ThemeName), value);
        }

        public string CultureName
        {
            get => Preferences.Get(nameof(CultureName), "en");
            set => Preferences.Set(nameof(CultureName), value);
        }
    }
}
