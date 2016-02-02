﻿using System;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
using System.Xml.Linq;
using EfawateerTests.Proxy;
using Gateways;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EfawateerTests
{
    [TestClass]
    public class manual_test_all_service
    {
        [TestMethod]
        public void BillerList()
        {
            var binding = new WSHttpBinding(SecurityMode.None, true);

            var token = new TokenServiceClient(binding, new EndpointAddress(UriContext.Authenticate));
            var element = token.Authenticate(Guid.NewGuid().ToString(), 158, "Test@1234");
            var key = element.Element("MsgBody").Element("TokenConf").Element("TokenKey").Value;

            var client = new BillersListClient(binding, new EndpointAddress(UriContext.BillersList));
            var list = client.GetBillersList(Guid.NewGuid().ToString(), key);   

            Debug.WriteLine("BillerList " +list);
        }

        [TestMethod]
        public void AccountUpload()
        {
            var binding = new WSHttpBinding(SecurityMode.None, true);

            var token = new TokenServiceClient(binding, new EndpointAddress(UriContext.Authenticate));
            var element = token.Authenticate(Guid.NewGuid().ToString(), 158, "Test@1234");
            var key = element.Element("MsgBody").Element("TokenConf").Element("TokenKey").Value;

            var request = XElement.Parse(GetContentFromFile("AccountUpload.xml"));
            var sign = new EfawateerSigner("ARKAIM-TEST");

            request.Element("MsgHeader").Element("TmStp").Value = DateTime.Now.ToString("s");
            request.Element("MsgFooter").Element("Security").Element("Signature").Value = sign.SignData(request.Element("MsgBody").ToString());

            var client = new AccountUploadClient(binding, new EndpointAddress(UriContext.AccountUpload));
            var upload = client.AccountUpload(Guid.NewGuid().ToString(), key, request);

            Debug.WriteLine("AccountUpload" + upload);
        }

        [TestMethod]
        public void AccountInquiry()
        {
            var binding = new WSHttpBinding(SecurityMode.None, true);

            var token = new TokenServiceClient(binding, new EndpointAddress(UriContext.Authenticate));
            var element = token.Authenticate(Guid.NewGuid().ToString(), 158, "Test@1234");
            var key = element.Element("MsgBody").Element("TokenConf").Element("TokenKey").Value;

            var request = XElement.Parse(GetContentFromFile("AccountInquiry.xml"));
            var sign = new EfawateerSigner("ARKAIM-TEST");

            request.Element("MsgHeader").Element("TmStp").Value = DateTime.Now.ToString("s");
            request.Element("MsgFooter").Element("Security").Element("Signature").Value = sign.SignData(request.Element("MsgBody").ToString());

            var client = new AccountInquiryClient(binding, new EndpointAddress(UriContext.AccountInquiry));
            var upload = client.AccountInquiry(Guid.NewGuid().ToString(), key, request);

            Debug.WriteLine("AccountInquiry " + upload);
        }

        [TestMethod]
        public void AddCustomerBilling()
        {
            var binding = new WSHttpBinding(SecurityMode.None, true);

            var token = new TokenServiceClient(binding, new EndpointAddress(UriContext.Authenticate));
            var element = token.Authenticate(Guid.NewGuid().ToString(), 158, "Test@1234");
            var key = element.Element("MsgBody").Element("TokenConf").Element("TokenKey").Value;

            var request = XElement.Parse(GetContentFromFile("AddCustomerBilling.xml"));
            var sign = new EfawateerSigner("ARKAIM-TEST");

            request.Element("MsgHeader").Element("TmStp").Value = DateTime.Now.ToString("s");
            request.Element("MsgFooter").Element("Security").Element("Signature").Value = sign.SignData(request.Element("MsgBody").ToString());

            var client = new AddCustomerBillingClient(binding, new EndpointAddress(UriContext.AddCustomerBilling));
            var upload = client.AddCustomerBilling(Guid.NewGuid().ToString(), key, request);

            Debug.WriteLine("AddCustomerBilling " + upload);
        }

        [TestMethod]
        public void BillPayment()
        {
            var binding = new WSHttpBinding(SecurityMode.None, true);

            var token = new TokenServiceClient(binding, new EndpointAddress(UriContext.Authenticate));
            var element = token.Authenticate(Guid.NewGuid().ToString(), 158, "Test@1234");
            var key = element.Element("MsgBody").Element("TokenConf").Element("TokenKey").Value;

            var request = XElement.Parse(GetContentFromFile("BillPayment.xml"));
            var sign = new EfawateerSigner("ARKAIM-TEST");

            var time = DateTime.Now.ToString("s");
            request.Element("MsgHeader").Element("TmStp").Value = time;
            request.Element("MsgBody").Element("Transactions").Element("TrxInf").Element("ProcessDate").Value = time;
            request.Element("MsgBody").Element("Transactions").Element("TrxInf").Element("BankTrxID").Value = Guid.NewGuid().ToString();
            request.Element("MsgFooter").Element("Security").Element("Signature").Value = sign.SignData(request.Element("MsgBody").ToString());

            var client = new PaymentClient(binding, new EndpointAddress(UriContext.BillPayment));
            var upload = client.PayBill(Guid.NewGuid().ToString(), key, request);

            Debug.WriteLine("BillPayment " + upload);
        }

        [TestMethod]
        public void BillInquiry()
        {
            var binding = new WSHttpBinding(SecurityMode.None, true);

            var token = new TokenServiceClient(binding, new EndpointAddress(UriContext.Authenticate));
            var element = token.Authenticate(Guid.NewGuid().ToString(), 158, "Test@1234");
            var key = element.Element("MsgBody").Element("TokenConf").Element("TokenKey").Value;

            var request = XElement.Parse(GetContentFromFile("BillInquiry.xml"));
            var sign = new EfawateerSigner("ARKAIM-TEST");

            var time = DateTime.Now.ToString("s");
            request.Element("MsgHeader").Element("TmStp").Value = time;
            request.Element("MsgFooter").Element("Security").Element("Signature").Value = sign.SignData(request.Element("MsgBody").ToString());

            var client = new BillInquiryClient(binding, new EndpointAddress(UriContext.BillInquiry));
            var upload = client.Inquire(Guid.NewGuid().ToString(), key, request);

            Debug.WriteLine("BillInquiry " + upload);
        }

        [TestMethod]
        public void PaymentInquiry()
        {
            var binding = new WSHttpBinding(SecurityMode.None, true);

            var token = new TokenServiceClient(binding, new EndpointAddress(UriContext.Authenticate));
            var element = token.Authenticate(Guid.NewGuid().ToString(), 158, "Test@1234");
            var key = element.Element("MsgBody").Element("TokenConf").Element("TokenKey").Value;

            var request = XElement.Parse(GetContentFromFile("PaymentInquiry.xml"));
            var sign = new EfawateerSigner("ARKAIM-TEST");

            var time = DateTime.Now.ToString("s");
            request.Element("MsgHeader").Element("TmStp").Value = time;
            request.Element("MsgBody").Element("Transactions").Element("TrxInf").Element("ProcessDate").Value = time;
            request.Element("MsgBody").Element("Transactions").Element("TrxInf").Element("PmtGuid").Value = Guid.NewGuid().ToString();
            request.Element("MsgBody").Element("Transactions").Element("TrxInf").Element("ParTrxID").Value = Guid.NewGuid().ToString();
            request.Element("MsgFooter").Element("Security").Element("Signature").Value = sign.SignData(request.Element("MsgBody").ToString());

            var client = new PaymentInquiryClient(binding, new EndpointAddress(UriContext.PaymentInquiry));
            var upload = client.Inquire(Guid.NewGuid().ToString(), key, request);

            Debug.WriteLine("PaymentInquiry " + upload);
        }

        [TestMethod]
        public void PrepaidPayment()
        {
            var binding = new WSHttpBinding(SecurityMode.None, true);

            var token = new TokenServiceClient(binding, new EndpointAddress(UriContext.Authenticate));
            var element = token.Authenticate(Guid.NewGuid().ToString(), 158, "Test@1234");
            var key = element.Element("MsgBody").Element("TokenConf").Element("TokenKey").Value;

            var request = XElement.Parse(GetContentFromFile("PrepaidPayment.xml"));
            var sign = new EfawateerSigner("ARKAIM-TEST");

            var time = DateTime.Now.ToString("s");
            var guid = Guid.NewGuid().ToString();
            request.Element("MsgHeader").Element("TmStp").Value = time;
            request.Element("MsgHeader").Element("GUID").Value = guid;
            request.Element("MsgBody").Element("TrxInf").Element("ProcessDate").Value = time;
            request.Element("MsgBody").Element("TrxInf").Element("BankTrxID").Value = guid;
            request.Element("MsgFooter").Element("Security").Element("Signature").Value = sign.SignData(request.Element("MsgBody").ToString());

            var client = new PrepaidPaymentClient(binding, new EndpointAddress(UriContext.PrepaidPayment));
            var upload = client.Pay(guid, key, request);

            Debug.WriteLine("PrepaidPayment " + upload);
        }

        [TestMethod]
        public void PrepaidValidation()
        {
            var binding = new WSHttpBinding(SecurityMode.None, true);

            var token = new TokenServiceClient(binding, new EndpointAddress(UriContext.Authenticate));
            var element = token.Authenticate(Guid.NewGuid().ToString(), 158, "Test@1234");
            var key = element.Element("MsgBody").Element("TokenConf").Element("TokenKey").Value;

            var request = XElement.Parse(GetContentFromFile("PrepaidValidation.xml"));
            var sign = new EfawateerSigner("ARKAIM-TEST");

            var time = DateTime.Now.ToString("s");
            var guid = Guid.NewGuid().ToString();
            request.Element("MsgHeader").Element("TmStp").Value = time;
            request.Element("MsgHeader").Element("GUID").Value = guid;
            request.Element("MsgFooter").Element("Security").Element("Signature").Value = sign.SignData(request.Element("MsgBody").ToString());

            var client = new PrepaidValidationClient(binding, new EndpointAddress(UriContext.PrepaidValidation));
            var upload = client.Validate(guid, key, request);

            Debug.WriteLine("PrepaidValidation " + upload);
        }

        private string GetContentFromFile(string name)
        {
            return File.ReadAllText("requests\\"+name);
        }
    }
}