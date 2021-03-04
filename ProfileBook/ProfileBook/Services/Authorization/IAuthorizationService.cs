namespace ProfileBook.Servises.Authorization
{
    public interface IAuthorizationService
    {
        void AddOrUpdateAuthorization(int id);
        int GetAuthorizedUserId();
    }
}
