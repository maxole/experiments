﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EfawateerWcf.BillInquiry {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="BillInquiry.IBillInquiry")]
    public interface IBillInquiry {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBillInquiry/Inquire", ReplyAction="http://tempuri.org/IBillInquiry/InquireResponse")]
        System.Xml.Linq.XElement Inquire(string guid, string tokenKey, System.Xml.Linq.XElement billInquiryRequest);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IBillInquiryChannel : EfawateerWcf.BillInquiry.IBillInquiry, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class BillInquiryClient : System.ServiceModel.ClientBase<EfawateerWcf.BillInquiry.IBillInquiry>, EfawateerWcf.BillInquiry.IBillInquiry {
        
        public BillInquiryClient() {
        }
        
        public BillInquiryClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public BillInquiryClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BillInquiryClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BillInquiryClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Xml.Linq.XElement Inquire(string guid, string tokenKey, System.Xml.Linq.XElement billInquiryRequest) {
            return base.Channel.Inquire(guid, tokenKey, billInquiryRequest);
        }
    }
}
