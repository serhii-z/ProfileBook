using Plugin.Settings;
using ProfileBook.Properties;
using ProfileBook.Resources.Themes;
using ProfileBook.Servises.Repository;
using System.Collections.Generic;
using System.Globalization;

namespace ProfileBook.Servises.Settings
{
    public class SettingsManager : ISettingsManager
    {
        public List<Models.Profile> SortByName(IRepository repository, int id)
        {
            string sql = $"SELECT * FROM Profiles  WHERE UserId='{id}' ORDER BY Name";
            return repository.GetListItems<Models.Profile>(sql).Result;
        }

        public List<Models.Profile> SortByNickName(IRepository repository, int id)
        {
            string sql = $"SELECT * FROM Profiles  WHERE UserId='{id}' ORDER BY NickName";
            return repository.GetListItems<Models.Profile>(sql).Result;
        }

        public List<Models.Profile> SortByDate(IRepository repository, int id)
        {
            string sql = $"SELECT * FROM Profiles  WHERE UserId='{id}' ORDER BY StartDate";
            return repository.GetListItems<Models.Profile>(sql).Result;
        }

        public void ApplyTheme(string themeName)
        {
            var resource = App.Current.Resources;
            resource.Clear();

            switch (themeName)
            {
                case "dark":
                    resource.Add(new darkTheme());
                    break;
                default:
                    resource.Add(new lightTheme());
                    break;
            }
        }

        public void ApplyCulture()
        {
            string culture = CrossSettings.Current.GetValueOrDefault("culture", string.Empty);

            if (culture.Equals(string.Empty))
            {
                culture = "en";
            }

            CultureInfo myCIintl = new CultureInfo(culture, false);
            Resource.Culture = myCIintl;
        }

        public void AddOrUpdateSorting(string sortingName)
        {
            CrossSettings.Current.AddOrUpdateValue("sort", sortingName);
        }

        public void AddOrUpdateTheme(string themeName)
        {
            CrossSettings.Current.AddOrUpdateValue("theme", themeName);
        }

        public void AddOrUpdateCulture(string culture)
        {
            CrossSettings.Current.AddOrUpdateValue("culture", culture);
        }

        public string GetSortingName()
        {
            return CrossSettings.Current.GetValueOrDefault("sort", string.Empty);
        }

        public string GetThemeName()
        {
            return CrossSettings.Current.GetValueOrDefault("theme", "light");
        }

        public string GetCultureName()
        {
            return CrossSettings.Current.GetValueOrDefault("culture", "en");
        }
    }
}
