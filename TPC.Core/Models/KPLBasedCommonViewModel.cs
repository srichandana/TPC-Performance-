using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Infrastructure;
using System.Reflection.Emit;
using TPC.Core.Models.Models;

namespace TPC.Core.Models
{


    public class KPLBasedCommonViewModel : ItemViewModel
    {

        private String type = string.Empty;

        [Display(Name = "Type", Order = 1)]
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        private string title = string.Empty;
        [Display(Name = "Title", Order = 2)]

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string setTitle = string.Empty;
        [Display(Name = "Set Title", Order = -1)]

        public string SetTitle
        {
            get { return setTitle; }
            set { setTitle = value; }
        }


        private string setName = string.Empty;

        [Display(Name = "Set Name", Order = 27)]
        public string SetName
        {
            get { return setName; }
            set { setName = value; }
        }

        private string setProfile = string.Empty;
        [Display(Name = "Set Profile", Order = -1)]
        public string SetProfile
        {
            get { return setProfile; }
            set { setProfile = value; }
        }

        private int? copyRight;
        [Display(Name = "Copyrt", Order = 20)]
        public int? CopyRight
        {
            get { return copyRight; }
            set { copyRight = value; }
        }

        private string publisher = string.Empty;
        [Display(Name = "Publisher", Order = 26)]
        public string Publisher
        {
            get { return publisher; }
            set { publisher = value; }
        }

        private string author = string.Empty;
        [Display(Name = "Author", Order = 21)]
        public string Author
        {
            get { return author; }
            set { author = value; }
        }

        private string illustrator = string.Empty;
        [Display(Name = "Illustrator", Order = -1)]
        public string Illustrator
        {
            get { return illustrator; }
            set { illustrator = value; }
        }

        private string classification = string.Empty;
        [Display(Name = "F/NF", ShortName = "C", Order = 7)]
        public string Classification
        {
            get { return classification; }
            set { classification = value; }
        }

        private string format = string.Empty;
        [Display(Name = "Frmt", ShortName = "F", Order = 8)]
        public string Format
        {
            get { return format; }
            set { format = value; }
        }

        private string interestGrade = string.Empty;
        [Display(Name = "Grd", ShortName = "IG", Order = 9)]
        public string InterestGrade
        {
            get { return interestGrade; }
            set { interestGrade = value; }
        }

        private string subject = string.Empty;
        [Display(Name = "Subject", Order = 11)]
        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }



        private string reviewSource = string.Empty;
        [Display(Name = "Rvw", Order = 17)]
        public string ReviewSource
        {
            get { return reviewSource; }
            set { reviewSource = value; }
        }
        private decimal? price;
        [Display(Name = "Price", Order = 10)]
        public decimal? Price
        {
            get { return price; }
            set { price = value; }
        }

        private string onListDate = string.Empty;

        [Display(Name = "On Date", Order = 18)]
        public string OnListDate
        {
            get { return onListDate; }
            set { onListDate = value; }
        }

        private int pages;
        [Display(Name = "Pages", Order = 24)]
        public int Pages
        {
            get { return pages; }
            set { pages = value; }
        }
        private string aRLevel;//AR - Accelerated Reader
        [Display(Name = "AR", Order = 12)]
        public string ARLevel
        {
            get { return aRLevel; }
            set { aRLevel = value; }
        }

        private int aRQuiz;
        [Display(Name = "AR Quiz", Order = -1)]
        public int ARQuiz
        {
            get { return aRQuiz; }
            set { aRQuiz = value; }
        }
        private string rcLevel =string.Empty;
        [Display(Name = "RC", Order = 14)]
        public string RCLevel
        {
            get { return rcLevel; }
            set { rcLevel = value; }
        } //RC - ReadingCounts
        private int rcQuiz;
        [Display(Name = "RC Quiz", Order = -1)]
        public int RCQuiz
        {
            get { return rcQuiz; }
            set { rcQuiz = value; }
        }

        private string lexile = string.Empty;
        [Display(Name = "Lexile", Order = 13)]
        public string Lexile
        {
            get { return lexile; }
            set { lexile = value; }
        }

        private string description = string.Empty;
        [Display(Name = "Description", Order = 16)]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        private string series = string.Empty;
        [Display(Name = "Series", Order = 4)]
        public string Series
        {
            get { return series; }
            set { series = value; }
        }

        private string primarycharacter = string.Empty;
        [Display(Name = "Primary Character", Order = 5)]
        public string Primarycharacter
        {
            get { return primarycharacter; }
            set { primarycharacter = value; }
        }

        private string secondCharacter = string.Empty;
        [Display(Name = "Second Character", Order = 6)]
        public string SecondCharacter
        {
            get { return secondCharacter; }
            set { secondCharacter = value; }
        }

        private string size = string.Empty;
        [Display(Name = "Size", Order = 23)]
        public string Size
        {
            get { return size; }
            set { size = value; }
        }


        private string setDescription = string.Empty;
        [Display(Name = "Set Description", Order = -1)]
        public string SetDescription
        {
            get { return setDescription; }
            set { setDescription = value; }
        }

        private List<string> sellingPoint = new List<string>();
        [Display(Name = "Selling Point", Order = -1)]
        public List<string> SellingPoint
        {
            get { return sellingPoint; }
            set { sellingPoint = value; }
        }
        private List<string> package = new List<string>();
        [Display(Name = "Package", Order = -1)]
        public List<string> Package
        {
            get { return package; }
            set { package = value; }
        }

        private ItemGroupViewModel itemListGVM;

        [Display(Name = "IPVM", Order = -1)]
        public ItemGroupViewModel ItemListGVM
        {
            get { return itemListGVM; }
            set { itemListGVM = value; }
        }


        private bool isInQuoteType;
        [Display(Name = "IsInQuoteType", Order = 3)]
        public bool IsInQuoteType
        {
            get { return isInQuoteType; }
            set { isInQuoteType = value; }
        }

        private string itemCount = string.Empty;
        [Display(Name = "Item Count", Order = -1)]
        public string ItemCount
        {
            get { return itemCount; }
            set { itemCount = value; }
        }

        private string dewery = string.Empty;
        [Display(Name = "Dewey", Order = 28)]
        public string Dewery
        {
            get { return dewery; }
            set { dewery = value; }
        }

        [Display(Name = "lstFilterModel", Order = -1)]
        public List<FilterModel> lstFilterModel { get; set; }

        private int onhandQty;
        [Display(Name = "OnHandQty", Order = 30)]
        public int OnhandQty
        {
            get { return onhandQty; }
            set { onhandQty = value; }
        }
        
    }
}
