using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using Gateways;
using Gateways.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EfawateerTests
{
    [TestClass]
    public class process
    {
        private DataTable _paymentTable;
        private DataTable _operatorTable;

        [TestInitialize]
        public void Initialize()
        {
            _paymentTable = new DataTable();
            _paymentTable.Columns.Add(new DataColumn("InitialSessionNumber", typeof (string)));
            _paymentTable.Columns.Add(new DataColumn("SessionNumber", typeof(string)));
            _paymentTable.Columns.Add(new DataColumn("TerminalID", typeof(int)));
            _paymentTable.Columns.Add(new DataColumn("StatusID", typeof(int)));
            _paymentTable.Columns.Add(new DataColumn("ErrorCode", typeof(int)));
            _paymentTable.Columns.Add(new DataColumn("Params", typeof(string)));
            _paymentTable.Columns.Add(new DataColumn("Amount", typeof(double)));
            _paymentTable.Columns.Add(new DataColumn("AmountAll", typeof(double)));

            _operatorTable = new DataTable();
            _operatorTable.Columns.Add(new DataColumn("OsmpFormatString", typeof(string)));
        }

        [TestMethod, Description("Ручная проверка")]
        public void ProcessPayment()
        {
            var paymentTbl = _paymentTable.NewRow();
            paymentTbl["TerminalID"] = 10;
            paymentTbl["StatusID"] = 1;
            paymentTbl["ErrorCode"] = 0;
            paymentTbl["Params"] = "billingno=25\\nbillingcode=39";
            paymentTbl["Amount"] = 10;
            paymentTbl["AmountAll"] = 10;

            var operatorTbl = _operatorTable.NewRow();
            operatorTbl["OsmpFormatString"] = "PymentType=[#PAYMENTTYPE];BillingNo=[#NUMBER];ServiceType=[#SERVICETYPE];PrepaidCat=[#ACCOUNT];DueAmt=[#AMOUNT]";

            var gate = new Gateways.EfawateerGateway();
            gate.Initialize(File.ReadAllText("initialize.xml"));
            gate.ProcessPayment(paymentTbl, operatorTbl, null);
        }

        [TestMethod, Description("Ручная проверка")]
        public void ProcessOnlineCheck()
        {
            var data = new NewPaymentData
            {
                Params = "billingno=25\\nbillingcode=39"
            };

            var row = _operatorTable.NewRow();
            row["OsmpFormatString"] = "billingno=[#billingno];billingcode=[#billingcode];";

            var gate = new Gateways.EfawateerGateway();

            gate.Initialize(File.ReadAllText("initialize.xml"));

            var processOnlineCheck = gate.ProcessOnlineCheck(data, row);
        }

        [TestMethod, Description("Ручная проверка")]
        public void PostPayment()
        {
            var data = new NewPaymentData
            {
                Params = "billingno=25\\nbillingcode=39"
            };

            var row = _operatorTable.NewRow();
            row["OsmpFormatString"] = "billingno=[#billingno];billingcode=[#billingcode];";

            var gate = new Gateways.EfawateerGateway();

            gate.Initialize(File.ReadAllText("initialize.xml"));

            var processOnlineCheck = gate.ProcessOnlineCheck(data, row);

            var q = processOnlineCheck.Split(new[]
            {
                Environment.NewLine
            }, StringSplitOptions.RemoveEmptyEntries).First(s => s.StartsWith("DUE")).Replace("DUE=", string.Empty);

            var paymentTbl = _paymentTable.NewRow();
            paymentTbl["TerminalID"] = 10;
            paymentTbl["StatusID"] = 1;
            paymentTbl["ErrorCode"] = 0;
            paymentTbl["Params"] = "billingno=25\\nbillingcode=39";            
            paymentTbl["Amount"] = Convert.ToDouble(q, CultureInfo.InvariantCulture);
            paymentTbl["AmountAll"] = Convert.ToDouble(q, CultureInfo.InvariantCulture);

            var operatorTbl = _operatorTable.NewRow();
            operatorTbl["OsmpFormatString"] = "billingno=[#billingno];billingcode=[#billingcode];";
            
            gate.ProcessPayment(paymentTbl, operatorTbl, null);
        }

        [TestMethod]
        public void prepaid_validation_service_type_request()
        {
            var gate = new Gateways.EfawateerGateway();
            gate.Initialize(File.ReadAllText("initialize.xml"));

            var list = new StringList("ServiceType=Prepaid;BillingNo=9010020304;DueAmt=43.12", ";");

            var request = gate.PrepaidValidationRequest(700162, list);
        }

        [TestMethod]
        public void bill_inquiry_request()
        {
            var gate = new Gateways.EfawateerGateway();
            gate.Initialize(File.ReadAllText("initialize.xml"));

            var list = new StringList("BillingNo=9010020304;ServiceType=Electricity", ";");

            var request = gate.BillInquiryRequest(700039, list);
        }

        [TestMethod]
        public void bill_payment_request()
        {

            var session = Guid.NewGuid().ToString();

            var gate = new Gateways.EfawateerGateway();
            gate.Initialize(File.ReadAllText("initialize.xml"));

            var list = new StringList("BillingNo=9010020304;ServiceType=Electricity", ";");

            var request = gate.BillInquiryRequest(700039, list);
            var dueAmt = request["DueAmt"];

            list = new StringList(string.Format("BillingNo=9010020304;ServiceType=Electricity;DueAmt={0}", dueAmt), ";");

            var response = gate.BillPaymentRequest(700039, list, session);

            Assert.AreEqual(0, response.Error);
        }

        [TestMethod]
        public void prepaid_payment_request()
        {
            var session = Guid.NewGuid().ToString();

            var gate = new Gateways.EfawateerGateway();
            gate.Initialize(File.ReadAllText("initialize.xml"));

            var list = new StringList("BillingNo=9010020304;ServiceType=Prepaid;DueAmt=43.12;ValidationCode=1234567", ";");
            var response = gate.PrepaidPaymentRequest(700162, list, session);

            Assert.AreEqual(0, response.Error);
        }

        [TestMethod]
        public void payment_inquiry_request()
        {
            var session = Guid.NewGuid().ToString();

            var gate = new Gateways.EfawateerGateway();
            gate.Initialize(File.ReadAllText("initialize.xml"));

            var list = new StringList("BillingNo=9010020304;ServiceType=Electricity;DueAmt=43.12;ValidationCode=1234567;PaymentType=Prepaid", ";");
            var response = gate.PaymentInquiryRequest(700039, list, session);

            Assert.AreEqual(0, response.Error);
        }
    }
}
