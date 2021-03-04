using ProfileBook.Resources.Themes;
using ProfileBook.Servises.Settings;
using Xamarin.Forms;

namespace ProfileBook.ResourceActivator
{
    public class ThemeActivator : IThemeActivator
    {
        private ISettingsManager _manager;

        public ThemeActivator(ISettingsManager manager)
        {
            _manager = manager;
        }
        public void AplyTheme(ResourceDictionary resourceDictionary)
        {
            resourceDictionary.Clear();

            switch (_manager.ThemeName)
            {
                case nameof(DarkTheme):
                    resourceDictionary.Add(new DarkTheme());
                    break;
                default:
                    resourceDictionary.Add(new LightTheme());
                    break;
            }
        }
    }
}
