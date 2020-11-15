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
        public List<Models.Profile> SortByName(IRepository repository, int id)
        {
            string sql = $"SELECT * FROM Profiles  WHERE UserId='{id}' ORDER BY Name";
            return repository.ChooseItems<Models.Profile>(sql).Result;
        }

        public List<Models.Profile> SortByNickName(IRepository repository, int id)
        {
            string sql = $"SELECT * FROM Profiles  WHERE UserId='{id}' ORDER BY NickName";
            return repository.ChooseItems<Models.Profile>(sql).Result;
        }

        public List<Models.Profile> SortByDate(IRepository repository, int id)
        {
            string sql = $"SELECT * FROM Profiles  WHERE UserId='{id}' ORDER BY StartDate";
            return repository.ChooseItems<Models.Profile>(sql).Result;
        }

        public void AplyTheme(string themeName)
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

        public void AplyCulture(IRepository repository)
        {
            var culture = GetCultureName(repository);

            if (string.IsNullOrEmpty(culture))
            {
                culture = "en";
            }

            CultureInfo myCIintl = new CultureInfo(culture, false);
            Resource.Culture = myCIintl;
        }

        public void AddOrUpdateSorting(IRepository repository, string sortingName)
        {
            var userSettings = FindUserSettings(repository);

            if (userSettings == null)
            {
                userSettings = CreateUserSettings();
                userSettings.Sorting = sortingName;
                repository.InsertItem<UserSettings>(userSettings);
            }
            else
            {
                userSettings.Sorting = sortingName;
                repository.UpdateItem<UserSettings>(userSettings);
            }        
        }

        public void AddOrUpdateTheme(IRepository repository, string themeName)
        {
            var userSettings = FindUserSettings(repository);

            if (userSettings == null)
            {
                userSettings = CreateUserSettings();
                userSettings.Theme = themeName;
                repository.InsertItem<UserSettings>(userSettings);
            }
            else
            {
                userSettings.Theme = themeName;
                repository.UpdateItem<UserSettings>(userSettings);
            }
        }

        public void AddOrUpdateCulture(IRepository repository, string culture)
        {
            var userSettings = FindUserSettings(repository);

            if (userSettings == null)
            {
                userSettings = CreateUserSettings();                
                userSettings.Culture = culture;
                repository.InsertItem<UserSettings>(userSettings);
            }
            else
            {
                userSettings.Culture = culture;
                repository.UpdateItem<UserSettings>(userSettings);
            }
        }

        public string GetSortingName(IRepository repository)
        {
            var userSettings = FindUserSettings(repository);

            if (userSettings == null)
            {
                return string.Empty;
            }

            return userSettings.Sorting;
        }

        public string GetThemeName(IRepository repository)
        {
            var userSettings = FindUserSettings(repository);

            if(userSettings == null)
            {
                return string.Empty;             
            }

            return userSettings.Theme;
        }

        public string GetCultureName(IRepository repository)
        {
            var userSettings = FindUserSettings(repository);

            if (userSettings == null)
            {
                return string.Empty;
            }

            return userSettings.Culture;
        }

        private UserSettings CreateUserSettings()
        {
            var userSettings = new UserSettings();
            userSettings.UserId = CrossSettings.Current.GetValueOrDefault("id", 0);

            return userSettings;
        }

        private UserSettings FindUserSettings(IRepository repository)
        {
            var userId = CrossSettings.Current.GetValueOrDefault("id", 0);

            if (userId == 0)
            {
                return null;
            }

            string sql = $"SELECT * FROM UserSettings WHERE UserId='{userId}'";
            return repository.FindItem<UserSettings>(sql).Result;            
        }
    }
}
