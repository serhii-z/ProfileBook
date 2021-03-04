namespace ProfileBook.Servises.Settings
{
    public interface ISettingsManager
    {
        string SortingName { get; set; }
        string ThemeName { get; set; }
        string CultureName { get; set; }
    }
}
