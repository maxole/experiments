using System;
using System.Data;
using System.IO;
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

            _operatorTable = new DataTable();
            _operatorTable.Columns.Add(new DataColumn("OsmpFormatString", typeof(string)));
        }

        [TestMethod]
        public void ProcessPayment()
        {
            var row = _paymentTable.NewRow();
            row["TerminalID"] = 10;
            row["StatusID"] = 1;
            row["ErrorCode"] = 0;
            row["Params"] = string.Empty;

            var gate = new Gateways.EfawateerGateway();
            gate.ProcessPayment(row, null, null);
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

    }
}
