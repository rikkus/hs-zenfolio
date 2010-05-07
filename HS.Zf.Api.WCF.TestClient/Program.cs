using System;
using HS.Zf.Api;
using HS.Zf.Api.Common;
using HS.Zf.TestClient.Properties;

namespace HS.Zf.TestClient
{
    internal class Program
    {
        private static void Main()
        {
            using (
                var client = new Client
                    (
                    Settings.Default.UserName,
                    Settings.Default.Password,
                    SecurityMode.CredentialsEncryptedOnly
                    )
                )
            {
                var profile = client.LoadPrivateProfile();

                var rootGroup = client.LoadGroupHierarchy(profile.LoginName);

                Console.WriteLine(rootGroup.Title);
            }

            Console.ReadLine();
        }
    }
}