using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPC.Core.Models
{
    public class RapidEntryModel : BaseViewModel
    {
        private int quoteId;

        public int QuoteID
        {
            get { return quoteId; }
            set { quoteId = value; }
        }

        private int quantityperitem;

        public int QuantityPerItem
        {
            get { return quantityperitem; }
            set { quantityperitem = value; }
        }

        private int NoOftitles;

        public int NoOfTitles
        {
            get { return NoOftitles; }
            set { NoOftitles = value; }
        }

        private int Noofbooks;

        public int NoOfBooks
        {
            get { return Noofbooks; }
            set { Noofbooks = value; }
        }


        private double quotetotalbeforecatlogandtax;

        public double QuoteTotalBefortaxandCatlog
        {
            get { return quotetotalbeforecatlogandtax; }
            set { quotetotalbeforecatlogandtax = value; }
        }


        private bool itemstatus;

        public bool ItemStatus
        {
            get { return itemstatus; }
            set { itemstatus = value; }
        }

        private bool itemExist;

        public bool ItemExist
        {
            get { return itemExist; }
            set { itemExist = value; }
        }

        private int quoteTypeID;

        public int QuoteTypeID
        {
            get { return quoteTypeID; }
            set { quoteTypeID = value; }
        }

    }
}