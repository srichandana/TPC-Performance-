using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPC.Core.Models.Models
{
   public class InvoiceModel
    {
        private DateTime invoiceDate;

        public DateTime InvoiceDate
        {
            get { return invoiceDate; }
            set { invoiceDate = value; }
        }
        private string terms;

        public string Terms
        {
            get { return terms; }
            set { terms = value; }
        }
        private string shipVia;

        public string ShipVIA
        {
            get { return shipVia; }
            set { shipVia = value; }
        }
        private string orderNumber;

        public string OrderNumber
        {
            get { return orderNumber; }
            set { orderNumber = value; }
        }

        private string federalID;

        public string FederalID
        {
            get { return federalID; }
            set { federalID = value; }
        }


        private string questions;

        public string Questions
        {
            get { return questions; }
            set { questions = value; }
        }

        private string customerpo;

        public string CustomerPO
        {
            get { return customerpo; }
            set { customerpo = value; }
        }

        private string invoiceType;

        public string InvoiceType
        {
            get { return invoiceType; }
            set { invoiceType = value; }
        }
        
    }
}
