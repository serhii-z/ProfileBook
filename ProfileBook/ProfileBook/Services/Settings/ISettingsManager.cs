using ProfileBook.Servises.Repository;
using System.Collections.Generic;

namespace ProfileBook.Servises.Settings
{
    public interface ISettingsManager
    {    
        List<Models.Profile> SortByName(IRepository repository, int id);
        List<Models.Profile> SortByNickName(IRepository repository, int id);
        List<Models.Profile> SortByDate(IRepository repository, int id);
        void AplyTheme(string themeName);
        void AplyCulture(IRepository repository);
        void AddOrUpdateSorting(IRepository repository, string sortingName);
        void AddOrUpdateTheme(IRepository repository, string themeName);
        void AddOrUpdateCulture(IRepository repository, string culture);     
        string GetSortingName(IRepository repository);
        string GetThemeName(IRepository repository);
        string GetCultureName(IRepository repository);
    }
}
