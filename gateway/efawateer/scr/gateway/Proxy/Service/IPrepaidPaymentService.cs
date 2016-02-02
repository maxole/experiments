﻿namespace EfawateerGateway.Proxy.Service
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName = "PrepaidPayment.IPrepaidPayment")]
    public interface IPrepaidPaymentService
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://tempuri.org/IPrepaidPayment/Pay", ReplyAction = "http://tempuri.org/IPrepaidPayment/PayResponse")]
        System.Xml.Linq.XElement Pay(string guid, string tokenKey, System.Xml.Linq.XElement paymnentValidationRequest);
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPrepaidPaymentChannel : IPrepaidPaymentService, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PrepaidPaymentClient : System.ServiceModel.ClientBase<IPrepaidPaymentService>, IPrepaidPaymentService
    {

        public PrepaidPaymentClient()
        {
        }

        public PrepaidPaymentClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public PrepaidPaymentClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public PrepaidPaymentClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public PrepaidPaymentClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        public System.Xml.Linq.XElement Pay(string guid, string tokenKey, System.Xml.Linq.XElement paymnentValidationRequest)
        {
            return base.Channel.Pay(guid, tokenKey, paymnentValidationRequest);
        }
    }
}
