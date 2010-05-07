namespace HS.Zf.Api.Common
{
    public static class Utils
    {
        public static string UrlForSecurityMode(SecurityMode securityMode)
        {
            return securityMode == SecurityMode.TransportEncrypted
                       ? Constants.SSLApiUri
                       : Constants.ApiUri
                ;
        }
    }
}