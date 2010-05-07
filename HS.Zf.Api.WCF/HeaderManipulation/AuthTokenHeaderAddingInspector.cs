using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using HS.Zf.Api.Common;

namespace HS.Zf.Api.HeaderManipulation
{
    internal class AuthTokenHeaderAddingInspector : IClientMessageInspector
    {
        private readonly Auth auth;
        private readonly Authenticate authenticate;
        private readonly string password;
        private readonly string userName;

        public AuthTokenHeaderAddingInspector(string userName, string password, Authenticate authenticate, Auth auth)
        {
            this.userName = userName;
            this.password = password;
            this.authenticate = authenticate;
            this.auth = auth;
        }

        private Auth Auth
        {
            get
            {
                if (!auth.IsValid)
                {
                    auth.Token = authenticate.Invoke(userName, password);
                    auth.ObtainedAt = DateTime.Now;
                }

                return auth;
            }
        }

        #region IClientMessageInspector Members

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            AddAuthToHttpHeader(request, Auth);

            return null; // No correlation state used.
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            // Nothing to do.
        }

        #endregion

        private static void AddAuthToHttpHeader(Message request, Auth auth)
        {
            object httpRequestMessageObject;

            if (!request.Properties.TryGetValue(HttpRequestMessageProperty.Name, out httpRequestMessageObject))
            {
                return;
            }

            var httpRequestMessageProperty = ((HttpRequestMessageProperty) httpRequestMessageObject);

            httpRequestMessageProperty.Headers[Constants.XZenFolioTokenHeaderKey] = auth.Token;
            httpRequestMessageProperty.Headers.Set(HttpRequestHeader.Cookie, auth.Token);
        }
    }
}