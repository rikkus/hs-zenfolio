using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using HS.Zf.Api.Common;
using HS.Zf.WSApi.com.zenfolio.www;

namespace HS.Zf.WSApi
{
    public class Client : ZfApi
    {
        private readonly Auth auth = Auth.Unauthorized;

        public Client(SecurityMode securityMode)
        {
            SecurityMode = securityMode;

            Url = Utils.UrlForSecurityMode(SecurityMode);
        }

        public Client(string userName, string password, SecurityMode securityMode)
        {
            UserName = userName;
            Password = password;
            SecurityMode = securityMode;

            Url = Utils.UrlForSecurityMode(SecurityMode);
        }

        public string UserName { get; private set; }
        public string Password { get; private set; }
        public SecurityMode SecurityMode { get; set; }

        private bool ShouldAutoAuthenticate
        {
            get { return !string.IsNullOrEmpty(UserName); }
        }

        private string CustomUserAgent
        {
            get { return typeof (Client).Namespace + " " + GetType().Assembly.GetName().Version; }
        }

        private static string Authenticate(string userName, string password, SecurityMode securityMode)
        {
            using (var client = new Client(securityMode))
            {
                var authChallenge = client.GetChallenge(userName);

                using (var hashAlgorithm = new SHA256Managed())
                {
                    return client.Authenticate(authChallenge.Challenge,
                                               HashPassword(password, hashAlgorithm, authChallenge));
                }
            }
        }

        private static byte[] HashPassword(string password, HashAlgorithm hashAlgorithm, AuthChallenge authChallenge)
        {
            return Encoding.UTF8.GetBytes(password)
                .Hash(authChallenge.PasswordSalt, hashAlgorithm)
                .Hash(authChallenge.Challenge, hashAlgorithm)
                ;
        }

        private bool Authenticate()
        {
            auth.Token = Authenticate(UserName, Password, SecurityMode);
            auth.ObtainedAt = DateTime.Now;

            return auth.IsValid;
        }

        protected override WebRequest GetWebRequest(Uri uri)
        {
            bool authorized = auth.IsValid;

            if (!authorized && ShouldAutoAuthenticate)
            {
                authorized = Authenticate();
            }

            var request = (HttpWebRequest) base.GetWebRequest(uri);

            SetUserAgentHeaders(CustomUserAgent, request);

            if (authorized)
            {
                SetTokenAndCookieHeaders(auth.Token, request);
            }

            return request;
        }

        private static void SetTokenAndCookieHeaders(string token, WebRequest request)
        {
            request.Headers[Constants.XZenFolioTokenHeaderKey] = token;
            request.Headers.Add(HttpRequestHeader.Cookie, token);
        }

        private static void SetUserAgentHeaders(string userAgent, HttpWebRequest request)
        {
            request.Headers[Constants.XZenFolioUserAgentHeaderKey] = userAgent;
            request.UserAgent = userAgent;
        }
    }
}