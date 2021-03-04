using ProfileBook.Properties;
using ProfileBook.Servises.Settings;
using System.Globalization;
using Xamarin.Essentials;

namespace ProfileBook.ResourceActivator
{
    public class CultureActivator : ICultureActivator
    {
        private ISettingsManager _manager;

        public CultureActivator(ISettingsManager manager)
        {
            _manager = manager;
        }
        public void AplyCulture()
        {
            var cultureName = Preferences.Get(nameof(_manager.CultureName), "en");

            if (string.IsNullOrEmpty(cultureName))
            {
                cultureName = "en";
            }

            CultureInfo info = new CultureInfo(cultureName, false);
            Resource.Culture = info;
        }
    }
}
