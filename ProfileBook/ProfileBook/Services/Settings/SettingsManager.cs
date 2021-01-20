using Plugin.Settings;
using ProfileBook.Models;
using ProfileBook.Properties;
using ProfileBook.Resources.Themes;
using ProfileBook.Servises.Repository;
using System.Collections.Generic;
using System.Globalization;

namespace ProfileBook.Servises.Settings
{
    public class SettingsManager : ISettingsManager
    {
        public string CreateSortRequest(int id, string sortingTopic)
        {
            return $"SELECT * FROM Profiles  WHERE UserId='{id}' ORDER BY " + sortingTopic;
        }

        public List<Models.Profile> Sort(IRepository repository, string sql)
        {
            return repository.ChooseItems<Models.Profile>(sql).Result;
        }

        public void AplyTheme(string themeName)
        {
            var resource = App.Current.Resources;
            resource.Clear();

            switch (themeName)
            {
                case "dark":
                    resource.Add(new DarkTheme());
                    break;
                default:
                    resource.Add(new LightTheme());
                    break;
            }
        }

        public void AplyCulture(IRepository repository)
        {
            var cultureName = GetCultureName(repository);

            if (string.IsNullOrEmpty(cultureName))
            {
                cultureName = "en";
            }

            CultureInfo info = new CultureInfo(cultureName, false);
            Resource.Culture = info;
        }

        public void AddOrUpdateSorting(IRepository repository, string sortingName)
        {
            var userSettings = GetUserSettings(repository);
            userSettings.Sorting = sortingName;
            repository.UpdateItem<UserSettings>(userSettings);      
        }

        public void AddOrUpdateTheme(IRepository repository, string themeName)
        {
            var userSettings = GetUserSettings(repository);
            userSettings.Theme = themeName;
            repository.UpdateItem<UserSettings>(userSettings);       
        }

        public void AddOrUpdateCulture(IRepository repository, string culture)
        {
            var userSettings = GetUserSettings(repository);
            userSettings.Culture = culture;
            repository.UpdateItem<UserSettings>(userSettings);        
        }

        public string GetSortingName(IRepository repository)
        {
            var sortingName = GetUserSettings(repository).Sorting;
            return sortingName;
        }

        public string GetThemeName(IRepository repository)
        {
            var themeName = GetUserSettings(repository).Theme;
            return themeName;
        }

        public string GetCultureName(IRepository repository)
        {
            var cultureName = GetUserSettings(repository).Culture;
            return cultureName;
        }

        private UserSettings CreateUserSettings()
        {
            var userSettings = new UserSettings();
            userSettings.UserId = CrossSettings.Current.GetValueOrDefault("id", 0);

            return userSettings;
        }

        private UserSettings GetUserSettings(IRepository repository)
        {
            var userId = CrossSettings.Current.GetValueOrDefault("id", 0);

            string sql = $"SELECT * FROM UserSettings WHERE UserId='{userId}'";
            var userSettings = repository.FindItem<UserSettings>(sql).Result;

            if (userSettings == null)
            {
                userSettings = CreateUserSettings();
                repository.InsertItem<UserSettings>(userSettings);
            }

            return userSettings;
        }
    }
}
