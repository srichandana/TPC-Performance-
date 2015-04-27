using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TPC.Core.Models
{
    public class AuthorViewModel
    {
        private string authorID;
        [Display(Name = "Author ID")]
        public string AuthorID
        {
            get { return authorID; }
            set { authorID = value; }
        }

        private string authorName;
        [Display(Name = "Author Name")]
        public string AuthorName
        {
            get { return authorName; }
            set { authorName = value; }
        }

        private int noofBooks;

        public int NoofBooks
        {
            get { return noofBooks; }
            set { noofBooks = value; }
        }
    }
}
