using ActivelyServer.Models;

namespace ActivelyServer.Services
{
    public interface ICredentialService
    {
        List<Credentials> GetAllCredentials();
        void AddCredential(Credentials credentials);
    }
}
