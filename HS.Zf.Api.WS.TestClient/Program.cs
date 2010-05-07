using System;
using HS.Zf.Api.Common;
using HS.Zf.Api.WS.TestClient.Properties;
using HS.Zf.WSApi;

namespace HS.Zf.Api.WS.TestClient
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