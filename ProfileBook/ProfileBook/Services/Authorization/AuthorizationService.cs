using Plugin.Settings.Abstractions;

namespace ProfileBook.Servises.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private ISettings _crossSettings;

        public AuthorizationService(ISettings crossSettings)
        {
            _crossSettings = crossSettings;
        }

        public void AddOrUpdateAuthorization(int id)
        {
             _crossSettings.AddOrUpdateValue("id", id);
        }

        public int GetAuthorizedUserId()
        {
            return _crossSettings.GetValueOrDefault("id", 0);
        }
    }
}
