using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TPC.Common.Enumerations
{
    public enum CatalogSubjectOptionEnums
    {
        [Display(ShortName = "3105")]
        CatalogCardKit = 52,

        [Display(ShortName = "3110")]
        ElectronicRecord = 44,

        [Display(ShortName = "3120")]
        BarcodeLabel = 48,

        [Display(ShortName = "3122")]
        secondBarcodeLabel = 49,

        [Display(ShortName = "3130")]
        ShelfListCard = 50,

        [Display(ShortName = "3140")]
        SpineLabel = 51,

        [Display(ShortName = "3144")]
        SpinePocketLabelSet = 54,

        [Display(ShortName = "3150")]
        ARSpineLabel = 57,

        [Display(ShortName = "3154")]
        ARLabelSet = 58,

        [Display(ShortName = "3160")]
        RCSpineLabel = 59,

        [Display(ShortName = "3164")]
        RCLabelSet = 60,

        [Display(ShortName = "3166")]
        LexileSpineLabel = 61,

        [Display(ShortName = "3170")]
        PeelStickPocket = 53,

        [Display(ShortName = "3172")]
        CheckoutCard = 55,

        [Display(ShortName = "3180", Name = "Cataloging Items")]
        Protector = 62,

        [Display(ShortName = "3185", Name = "Shelf Ready Processing")]
        ShelfReadyUnit = 63,

        [Display(ShortName = "3181", Name = "Additional Protectors")]
        ExtraProtectors = 64,

        SpecialInstructions = 65,

        [Display(ShortName = "/CATPERBOOK", Name = "Special Per Book")]
        SpecialPerBookCharge = 66,

        [Display(ShortName = "/CATBULK", Name = "Special Bulk Charge")]
        SpecialBulkCharge = 68
    }
}
