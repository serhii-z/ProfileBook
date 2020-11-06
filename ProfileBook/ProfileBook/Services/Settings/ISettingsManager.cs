using ProfileBook.Servises.Repository;
using System.Collections.Generic;

namespace ProfileBook.Servises.Settings
{
    public interface ISettingsManager
    {
        void ApplyTheme(bool isDarkTheme);
        List<Models.Profile> SortByName(IRepository repository, int id);
        List<Models.Profile> SortByNickName(IRepository repository, int id);
        List<Models.Profile> SortByDate(IRepository repository, int id);
        void ApplyCulture();
        void AddOrUpdateCulture(string culture);
        void AddOrUpdateSorting(string sortingName);
        void AddOrUpdateTheme(bool isTheme);
        string GetSortingName();
        string GetCultureName();
        bool GetThemeActive();
    }
}
