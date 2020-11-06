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
        public void ApplyTheme(bool isDarkTheme)
        {
            var resource = App.Current.Resources;
            resource.Clear();

            if (isDarkTheme)
            {
                resource.Add(new darkTheme());
            }
            else
            {
                resource.Add(new lightTheme());
            }
        }

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

        public void ApplyCulture()
        {
            string culture = CrossSettings.Current.GetValueOrDefault("culture", "en");
            CultureInfo myCIintl = new CultureInfo(culture, false);
            Resource.Culture = myCIintl;
        }

        public void AddOrUpdateCulture(string culture)
        {
            CrossSettings.Current.AddOrUpdateValue("culture", culture);
        }

        public void AddOrUpdateSorting(string sortingName)
        {
            CrossSettings.Current.AddOrUpdateValue("sort", sortingName);
        }

        public void AddOrUpdateTheme(bool isTheme)
        {
            CrossSettings.Current.AddOrUpdateValue("theme", isTheme);
        }

        public string GetSortingName()
        {
            return CrossSettings.Current.GetValueOrDefault("sort", string.Empty);
        }

        public string GetCultureName()
        {
            return CrossSettings.Current.GetValueOrDefault("culture", "en");
        }

        public bool GetThemeActive()
        {
            return CrossSettings.Current.GetValueOrDefault("theme", false);
        }
    }
}
