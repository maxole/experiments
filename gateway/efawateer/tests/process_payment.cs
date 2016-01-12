using System;
using System.Data;
using Gateways;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EfawateerTests
{
    [TestClass]
    public class process_payment
    {
        private DataTable paymentTable;
        private DataTable operatorTable;

        [TestInitialize]
        public void Initialize()
        {
            paymentTable = new DataTable();
            paymentTable.Columns.Add(new DataColumn("InitialSessionNumber", typeof (string)));
            paymentTable.Columns.Add(new DataColumn("SessionNumber", typeof(string)));
            paymentTable.Columns.Add(new DataColumn("TerminalID", typeof(int)));
            paymentTable.Columns.Add(new DataColumn("StatusID", typeof(int)));
            paymentTable.Columns.Add(new DataColumn("ErrorCode", typeof(int)));
            paymentTable.Columns.Add(new DataColumn("Params", typeof(string)));

            operatorTable = new DataTable();
            operatorTable.Columns.Add(new DataColumn("OsmpFormatString", typeof(string)));
        }

        [TestMethod]
        public void ProcessPayment()
        {
            var row = paymentTable.NewRow();
            row["TerminalID"] = 10;
            row["StatusID"] = 1;
            row["ErrorCode"] = 0;
            row["Params"] = string.Empty;

            var gate = new EfawateerGateway();
            gate.ProcessPayment(row, null, null);
        }

        [TestMethod]
        public void ProcessOnlineCheck()
        {
            var data = new NewPaymentData();

            var row = operatorTable.NewRow();

            var gate = new EfawateerGateway();
            gate.ProcessOnlineCheck(data, row);
        }

    }
}
