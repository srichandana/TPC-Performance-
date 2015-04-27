using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TPC.Core.Models.ViewModels
{
    public class ItemDetailedViewModel : KPLBasedCommonViewModel
    {



        private string binding;
        [Display(Name = "Binding")]
        public string Binding
        {
            get { return binding; }
            set { binding = value; }
        }
        private string readingPrograms;
        [Display(Name = "ReadingPrograms")]
        public string ReadingPrograms
        {
            get { return readingPrograms; }
            set { readingPrograms = value; }
        }
        private string fiction;
        [Display(Name = "Fiction/NonFiction")]
        public string Fiction
        {
            get { return fiction; }
            set { fiction = value; }

        }
        private string dewey;
        [Display(Name = "Dewey")]
        public string Dewey
        {
            get { return dewey; }
            set { dewey = value; }
        }



        private int quoteID;

        public int QuoteID
        {
            get { return quoteID; }
            set { quoteID = value; }
        }


        private int quoteTypeID;

        public int QuoteTypeID
        {
            get { return quoteTypeID; }
            set { quoteTypeID = value; }
        }



        private List<ItemParentViewModel> lstItemParentVM;

        public List<ItemParentViewModel> LstItemParentVM
        {
            get { return lstItemParentVM; }
            set { lstItemParentVM = value; }
        }

        private int setItemCount;
        public int SetItemCount
        {
            get
            {
                if (lstItemParentVM != null && lstItemParentVM.Count() > 0)
                {
                    return lstItemParentVM[0].ListItemVM.Count();
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                setItemCount = value;
            }
        }

        private double setItemsTotalPrice;

        public double SetItemsTotalPrice
        {
            get { return setItemsTotalPrice; }
            set { setItemsTotalPrice = value; }
        }

        private int seriesItemCount;

        public int SeriesItemCount
        {
            get
            {
                if (lstItemParentVM != null && lstItemParentVM.Count() > 1)
                {
                    return lstItemParentVM[1].ListItemVM.Count();
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                setItemCount = value;
            }
        }

        private double seriesItemsTotalPrice;

        public double SeriesItemsTotalPrice
        {
            get { return seriesItemsTotalPrice; }
            set { seriesItemsTotalPrice = value; }
        }
    }
}
