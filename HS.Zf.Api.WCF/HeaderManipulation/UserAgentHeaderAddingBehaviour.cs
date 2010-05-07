using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace HS.Zf.Api.HeaderManipulation
{
    internal class UserAgentHeaderAddingBehaviour : IEndpointBehavior
    {
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
            clientRuntime.MessageInspectors.Add(new UserAgentHeaderAddingInspector());
        }

        #endregion
    }
}