using ActivelyServer.Models;
using System.Collections.Generic;
using ActivelyServer;

namespace ActivelyServer.Services
{
    public class CredentialService : ICredentialService
    {
        private BazaDanych bazaDanych = new BazaDanych();

        public List<Credentials> GetAllCredentials()
        {
            Dictionary<string, string> users = bazaDanych.GetUsers();
            List<Credentials> credentials = new List<Credentials>();

            foreach (var user in users)
            {
                credentials.Add(new Credentials { Login = user.Key, Password = user.Value });
            }

            return credentials;
        }

        public void AddCredential(Credentials credential)
        {
            bazaDanych.AddUser(credential.Login, credential.Password);
        }
    }
}