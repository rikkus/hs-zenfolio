using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using HS.Zf.Api.Common;

namespace HS.Zf.Api.HeaderManipulation
{
    internal class AuthTokenHeaderAddingBehaviour : IEndpointBehavior
    {
        private readonly Auth auth;
        private readonly Authenticate authenticate;
        private readonly string password;
        private readonly string userName;

        public AuthTokenHeaderAddingBehaviour(string userName, string password, Authenticate authenticate, Auth auth)
        {
            this.userName = userName;
            this.password = password;
            this.authenticate = authenticate;
            this.auth = auth;
        }

        #region IEndpointBehavior Members

        public void Validate(ServiceEndpoint endpoint)
        {
            // Nothing to do.
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            // Nothing to do.
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            // Nothing to do.
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add
                (new AuthTokenHeaderAddingInspector(userName, password, authenticate, auth));
        }

        #endregion
    }
}