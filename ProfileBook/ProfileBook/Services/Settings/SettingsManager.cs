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
        public bool IsDarkTheme { get; set; }
        public string Language { get; set; }

        public void ChangeTheme()
        {
            var r = App.Current.Resources;
            r.Clear();
            if (IsDarkTheme)
            {
                r.Add(new darkTheme());
            }
            else
            {
                r.Add(new lightTheme());
            }
        }

        public List<Models.Profile> SortByName(IRepository repository, int id)
        {
            string sql = $"SELECT * FROM Profiles  WHERE UserId='{id}' ORDER BY Name";
            var result = repository.GetListItems<Models.Profile>(sql);
            return result.Result;
        }

        public List<Models.Profile> SortByNickName(IRepository repository, int id)
        {
            string sql = $"SELECT * FROM Profiles  WHERE UserId='{id}' ORDER BY NickName";
            var result = repository.GetListItems<Models.Profile>(sql);
            return result.Result;
        }

        public List<Models.Profile> SortByDate(IRepository repository, int id)
        {
            string sql = $"SELECT * FROM Profiles  WHERE UserId='{id}' ORDER BY StartDate";
            var result = repository.GetListItems<Models.Profile>(sql);
            return result.Result;
        }

        public void ChengeCulture(string culture)
        {
            CultureInfo myCIintl = new CultureInfo(culture, false);
            Resource.Culture = myCIintl;
            CrossSettings.Current.AddOrUpdateValue("culture", culture);
        }
    }
}
