using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Common;

namespace TPC.Common.Enumerations
{
    public enum ProductLineEnum
    {
        [Description("CAT")]
        Category = 1,
         [Description("PRO")]
        Promotional = 2,
         [Description("PPB")]
        PenworthyPreBound = 3,
         [Description("PUP")]
        Puppets = 4,
         [Description("BB ")]
        BoardBooks = 5,
         [Description("LBY")]
        LibraryBooks = 6
    }
}
