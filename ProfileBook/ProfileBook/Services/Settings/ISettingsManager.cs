using ProfileBook.Servises.Repository;
using System.Collections.Generic;

namespace ProfileBook.Servises.Settings
{
    public interface ISettingsManager
    {    
        List<Models.Profile> SortByName(IRepository repository, int id);
        List<Models.Profile> SortByNickName(IRepository repository, int id);
        List<Models.Profile> SortByDate(IRepository repository, int id);
        void ApplyTheme(string themeName);
        void ApplyCulture();
        void AddOrUpdateSorting(string sortingName);
        void AddOrUpdateTheme(string themeName);
        void AddOrUpdateCulture(string culture);     
        string GetSortingName();
        string GetThemeName();
        string GetCultureName();
    }
}
