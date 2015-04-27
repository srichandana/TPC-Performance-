using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPC.Common.Constants
{
    public static class QuoteValidationConstants
    {
        public const string Hold_Representative = "HREP";
        public const string Hold_PO = "HPO";
        public const string Hold_Credit = "HCRDT";
        public const string Hold_2_previews = "H2PRV";
        public const string Hold_stick_rate = "HSTICK";
        public const string Hold_Audit = "HADT";
        public const string Hold_Profile_Incomplete = "HPROF";

        #region ReviewSource
        public const string SLJ = "Schl Lib Jrnl";
        public const string PW = "Pub Wkly";
        public const string HORN = "Horn Bk";
        public const string KIRKUS = "Kirkus";
        public const string LJ = "Lib Jrnl";
        #endregion
        
        #region InterestGrades
        public const string Preschooltograde1Text = "K";
        public const string Preschooltograde1 ="P-1";
        public const string Grade2to3 ="2-3";
        public const string Grade4Above ="4+";
        #endregion
    }
}
