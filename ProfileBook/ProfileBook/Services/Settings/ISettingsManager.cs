using ProfileBook.Servises.Repository;
using System.Collections.Generic;

namespace ProfileBook.Servises.Settings
{
    public interface ISettingsManager
    {
        bool IsDarkTheme { get; set; }
        void ChangeTheme();
        List<Models.Profile> SortByName(IRepository repository, int id);
        List<Models.Profile> SortByNickName(IRepository repository, int id);
        List<Models.Profile> SortByDate(IRepository repository, int id);
        void ChengeCulture(string culture);
    }
}
