using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPC.Core.Models.ViewModels
{
    public class CartWhizardInfoModel
    {

        private int quoteID;
        [Display(Name = "QuoteID")]
        public int QuoteID
        {
            get { return quoteID; }
            set { quoteID = value; }
        }

        private DateTime penworthyUpdatedDate;

        [Display(Name = "Updated")]
        public DateTime PenworthyUpdatedDate
        {
            get { return penworthyUpdatedDate; }
            set { penworthyUpdatedDate = value; }
        }

        private DateTime customerUpdatedDate;

        [Display(Name = "Customer Updated")]
        public DateTime CustomerUpdatedDate
        {
            get { return customerUpdatedDate; }
            set { customerUpdatedDate = value; }
        }

        private int numberOfBooks;

        [Display(Name = "Items")]
        public int NumberOfBooks
        {
            get { return numberOfBooks; }
            set { numberOfBooks = value; }
        }

        private double totalPrice;

        [Display(Name = "Total")]
        public double TotalPrice
        {
            get { return totalPrice; }
            set { totalPrice = value; }
        }

       

        private int noofYesCount;

        [Display(Name = "Yes")]
        public int NoofYesCount
        {
            get { return noofYesCount; }
            set { noofYesCount = value; }
        }

        private int noofNoCount;
        [Display(Name = "No")]
        public int NoofNoCount
        {
            get { return noofNoCount; }
            set { noofNoCount = value; }
        }

        private int nooyMaybeCount;
        [Display(Name = "Maybe")]
        public int NoofMaybCount
        {
            get { return nooyMaybeCount; }
            set { nooyMaybeCount = value; }
        }


        [Display(Name = "New")]
        public int NoofNewCount
        {
            get { return numberOfBooks - (noofNoCount + noofYesCount + nooyMaybeCount); }
        }

        private string dwName;
          [Display(Name = "Name")]
        public string DWName
        {
            get { return dwName; }
            set { dwName = value; }
        }

        
    }
}
