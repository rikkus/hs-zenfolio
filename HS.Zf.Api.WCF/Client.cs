using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using HS.Zf.Api.Common;
using HS.Zf.Api.HeaderManipulation;
using HS.Zf.Api.Zenfolio;
using SecurityMode=HS.Zf.Api.Common.SecurityMode;

namespace HS.Zf.Api
{
    internal delegate string Authenticate(string userName, string password);

    public class Client : ZfApiSoapClient
    {
        private readonly Auth auth = Auth.Unauthorized;
        private readonly SecurityMode securityMode;

        public Client(string userName, string password, SecurityMode securityMode)
            : base(CreateBinding(securityMode), CreateEndpointAddress(securityMode))
        {
            this.securityMode = securityMode;

            Endpoint.Behaviors.Add(new UserAgentHeaderAddingBehaviour());
            Endpoint.Behaviors.Add(new AuthTokenHeaderAddingBehaviour(userName, password, Authenticate, auth));
        }

        private static Binding CreateBinding(SecurityMode securityMode)
        {
            return new BasicHttpBinding
                (
                securityMode == SecurityMode.TransportEncrypted
                    ? BasicHttpSecurityMode.Transport
                    : BasicHttpSecurityMode.None
                );
        }

        private static EndpointAddress CreateEndpointAddress(SecurityMode securityMode)
        {
            return new EndpointAddress(Utils.UrlForSecurityMode(securityMode));
        }

        private ZfApiSoapClient CreateNonAutoAuthorizingClient()
        {
            var client = new ZfApiSoapClient(CreateBinding(securityMode), CreateEndpointAddress(securityMode));

            client.Endpoint.Behaviors.Add(new UserAgentHeaderAddingBehaviour());

            return client;
        }

        private string Authenticate(string userName, string password)
        {
            using (var client = CreateNonAutoAuthorizingClient())
            {
                var authChallenge = client.GetChallenge(userName);

                using (var hashAlgorithm = new SHA256Managed())
                {
                    return client.Authenticate
                        (
                        authChallenge.Challenge,
                        Encoding.UTF8.GetBytes(password)
                            .Hash(authChallenge.PasswordSalt, hashAlgorithm)
                            .Hash(authChallenge.Challenge, hashAlgorithm)
                        );
                }
            }
        }
    }
}