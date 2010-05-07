using System;

namespace HS.Zf.Api.Common
{
    public class Auth
    {
        private const int MaxHoursToKeepToken = 12;

        public Auth(string token)
        {
            Token = token;
            ObtainedAt = DateTime.Now;
        }

        public string Token { get; set; }
        public DateTime ObtainedAt { get; set; }

        public bool IsValid
        {
            get
            {
                return !(string.IsNullOrEmpty(Token))
                       && DateTime.Now.Subtract(ObtainedAt).Hours < MaxHoursToKeepToken;
            }
        }

        public static Auth Unauthorized
        {
            get { return new Auth(string.Empty); }
        }
    }
}