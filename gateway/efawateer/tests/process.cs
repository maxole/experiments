using System;
using System.Data;
using System.IO;
using System.Linq;
using Gateways;
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
            paymentTbl["Params"] = "billingno=25;";
            paymentTbl["Amount"] = 10;
            paymentTbl["AmountAll"] = 10;

            var operatorTbl = _operatorTable.NewRow();
            operatorTbl["OsmpFormatString"] = "billingno=[#billingno]";

            var gate = new Gateways.EfawateerGateway();
            gate.Initialize(File.ReadAllText("initialize.xml"));
            gate.ProcessPayment(paymentTbl, operatorTbl, null);
        }

        [TestMethod, Description("Ручная проверка")]
        public void ProcessOnlineCheck()
        {
            var data = new NewPaymentData
            {
                Params = "billingno=25;"
            };

            var row = _operatorTable.NewRow();
            row["OsmpFormatString"] = "billingno=[#billingno]";

            var gate = new Gateways.EfawateerGateway();

            gate.Initialize(File.ReadAllText("initialize.xml"));

            var processOnlineCheck = gate.ProcessOnlineCheck(data, row);
        }

        [TestMethod, Description("Ручная проверка")]
        public void PostPayment()
        {
            var data = new NewPaymentData
            {
                Params = "billingno=25;"
            };

            var row = _operatorTable.NewRow();
            row["OsmpFormatString"] = "billingno=[#billingno]";

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
            paymentTbl["Params"] = "billingno=25;";
            paymentTbl["Amount"] = Convert.ToDouble(q);
            paymentTbl["AmountAll"] = Convert.ToDouble(q);

            var operatorTbl = _operatorTable.NewRow();
            operatorTbl["OsmpFormatString"] = "billingno=[#billingno]";
            
            gate.ProcessPayment(paymentTbl, operatorTbl, null);
        }
    }
}
