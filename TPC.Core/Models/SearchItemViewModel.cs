using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TPC.Core.Models
{
    public class SearchItemViewModel
    {
        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private int authorID;

        public int AuthorID
        {
            get { return authorID; }
            set { authorID = value; }
        }

        private string series;

        public string Series
        {
            get { return series; }
            set { series = value; }
        }

        private string primaryCharecter;

        public string PrimaryCharecter
        {
            get { return primaryCharecter; }
            set { primaryCharecter = value; }
        }

        private int noofBooksTitle;

        public int NoofBooksTitle
        {
            get { return noofBooksTitle; }
            set { noofBooksTitle = value; }
        }

        private int noofBooksSeries;

        public int NoofBooksSeries
        {
            get { return noofBooksSeries; }
            set { noofBooksSeries = value; }
        }

        private int noofBooksCharecter;

        public int NoofBooksCharecter
        {
            get { return noofBooksCharecter; }
            set { noofBooksCharecter = value; }
        }

        public List<SearchViewModel> Listtitle { get; set; }
    }
}
