﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EfawateerWcf.BillersList {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BillersList.IBillersList")]
    public interface IBillersList {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBillersList/GetBillersList", ReplyAction="http://tempuri.org/IBillersList/GetBillersListResponse")]
        System.Xml.Linq.XElement GetBillersList(string guid, string tokenKey);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IBillersListChannel : EfawateerWcf.BillersList.IBillersList, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class BillersListClient : System.ServiceModel.ClientBase<EfawateerWcf.BillersList.IBillersList>, EfawateerWcf.BillersList.IBillersList {
        
        public BillersListClient() {
        }
        
        public BillersListClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public BillersListClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BillersListClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BillersListClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Xml.Linq.XElement GetBillersList(string guid, string tokenKey) {
            return base.Channel.GetBillersList(guid, tokenKey);
        }
    }
}
