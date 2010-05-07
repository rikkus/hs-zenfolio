using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using HS.Zf.Api.Common;

namespace HS.Zf.Api.HeaderManipulation
{
    internal class UserAgentHeaderAddingInspector : IClientMessageInspector
    {
        private string UserAgent
        {
            get { return typeof (Client).Namespace + " " + GetType().Assembly.GetName().Version; }
        }

        #region IClientMessageInspector Members

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            HttpRequestMessageProperty httpRequestMessageProperty;

            object httpRequestMessageObject;

            if (request.Properties.TryGetValue(HttpRequestMessageProperty.Name, out httpRequestMessageObject))
            {
                httpRequestMessageProperty = (HttpRequestMessageProperty) httpRequestMessageObject;

                httpRequestMessageProperty.Headers.Set(HttpRequestHeader.UserAgent, UserAgent);
                httpRequestMessageProperty.Headers[Constants.XZenFolioUserAgentHeaderKey] = UserAgent;
            }

            return null; // No correlation state used.
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            // Nothing to do.
        }

        #endregion
    }
}