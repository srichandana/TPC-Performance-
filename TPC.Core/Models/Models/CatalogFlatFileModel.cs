using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPC.Core.Models.Models
{
    public class CatalogFlatFileModel
    {
        private string labelidentification;
        [Display(Name = "UDF_Label_Identification", ShortName = "1")]
        public string LabelIdentification
        {
            get { return labelidentification; }
            set { labelidentification = value; }
        }

        private string style;
        [Display(Name = "UDF_Style", ShortName = "2")]
        public string Style
        {
            get { return style; }
            set { style = value; }
        }

        private string mainentrynumber;
        [Display(Name = "UDF_Main_Entry_Number", ShortName = "3")]
        public string MainEntryNumber
        {
            get { return mainentrynumber; }
            set { mainentrynumber = value; }
        }

        private string biography;
        [Display(Name = "UDF_Biography", ShortName = "4")]
        public string Biography
        {
            get { return biography; }
            set { biography = value; }
        }


        private string easybooks;
        [Display(Name = "UDF_Easy_Books", ShortName = "5")]
        public string EasyBooks
        {
            get { return easybooks; }
            set { easybooks = value; }
        }

        private string fictiongrades;
        [Display(Name = "UDF_Fiction", ShortName = "6")]
        public string FictionGrades
        {
            get { return fictiongrades; }
            set { fictiongrades = value; }
        }

        private string nonfictiondewey;
        [Display(Name = "UDF_Nonfiction_Dewey", ShortName = "7")]
        public string NoNFictionDewey
        {
            get { return nonfictiondewey; }
            set { nonfictiondewey = value; }
        }

        private string placesrightofdecimal;
        [Display(Name = "UDF_Decimal_Places", ShortName = "76")]
        public string PlacesRightofDecimal
        {
            get { return placesrightofdecimal; }
            set { placesrightofdecimal = value; }
        }

        private string classifictionprefix;
        [Display(Name = "UDF_Classification_Prefix", ShortName = "8")]
        public string ClassificationPrefix
        {
            get { return classifictionprefix; }
            set { classifictionprefix = value; }
        }


        private string subjectheading;
        [Display(Name = "UDF_Subject_Headings", ShortName = "9")]
        public string SubjectHeading
        {
            get { return subjectheading; }
            set { subjectheading = value; }
        }

        private string classifictioncomments;
        [Display(Name = "UDF_Classification_Comments", ShortName = "32")]
        public string ClassifictionComments
        {
            get { return classifictioncomments; }
            set { classifictioncomments = value; }
        }



        private string computertype;
        [Display(Name = "UDF_Computer_Type", ShortName = "12")]
        public string ComputerType
        {
            get { return computertype; }
            set { computertype = value; }
        }



        private string mediatype;
        [Display(Name = "UDF_Media_Type", ShortName = "13")]
        public string MediaType
        {
            get { return mediatype; }
            set { mediatype = value; }
        }

        private string dataforamt;
        [Display(Name = "UDF_Data_Format", ShortName = "14")]
        public string DataFormat
        {
            get { return dataforamt; }
            set { dataforamt = value; }
        }


        private string softwaresystem;
        [Display(Name = "UDF_Software_System", ShortName = "15")]
        public string SoftwareSystem
        {
            get { return softwaresystem; }
            set { softwaresystem = value; }
        }

        private string version;
        [Display(Name = "UDF_Version", ShortName = "33")]
        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        private string emailaddress;
        [Display(Name = "UDF_Email_Address", ShortName = "69")]
        public string EmailAddress
        {
            get { return emailaddress; }
            set { emailaddress = value; }
        }

        private string electronicrecordcomments;
        [Display(Name = "UDF_Electronic_Record_Comments", ShortName = "34")]
        public string ElectronicRecordComments
        {
            get { return electronicrecordcomments; }
            set { electronicrecordcomments = value; }
        }


        private string symbology;
        [Display(Name = "UDF_Symbology", ShortName = "19")]
        public string Symbology
        {
            get { return symbology; }
            set { symbology = value; }
        }


        private string startingbarcodenumber;
        [Display(Name = "UDF_Starting_Barcode_Number", ShortName = "35")]
        public string StartingBarcodeNumber
        {
            get { return startingbarcodenumber; }
            set { startingbarcodenumber = value; }
        }

        private string barcodecomments;
        [Display(Name = "UDF_BarCode_Comments", ShortName = "37")]
        public string BarCodeComments
        {
            get { return barcodecomments; }
            set { barcodecomments = value; }
        }

        private string shelfreadycomments;
        [Display(Name = "UDF_Shelf_Ready_Comments", ShortName = "97")]
        public string ShelfReadyComments
        {
            get { return shelfreadycomments; }
            set { shelfreadycomments = value; }
        }


        private string barcodeposition;
        [Display(Name = "UDF_BarCode_Position", ShortName = "24")]
        public string BarCodePosition
        {
            get { return barcodeposition; }
            set { barcodeposition = value; }
        }


        private string barcodealignment;
        [Display(Name = "UDF_Barcode_Alignmet", ShortName = "25")]
        public string BarcodeAlignmet
        {
            get { return barcodealignment; }
            set { barcodealignment = value; }
        }

        private string duplicatebarcodeposition;
        [Display(Name = "UDF_Duplicate_Barcode_Position", ShortName = "78")]
        public string DuplicateBarcodePosition
        {
            get { return duplicatebarcodeposition; }
            set { duplicatebarcodeposition = value; }
        }


        private string duplicatebarcodealignment;
        [Display(Name = "UDF_Duplicate_Barcode_Alignment", ShortName = "79")]
        public string DuplicateBarcodeAlignment
        {
            get { return duplicatebarcodealignment; }
            set { duplicatebarcodealignment = value; }
        }


        private string arinfolabelposition;
        [Display(Name = "UDF_AR_Info_Label_Position", ShortName = "80")]
        public string ARInfoLabelPosition
        {
            get { return arinfolabelposition; }
            set { arinfolabelposition = value; }
        }


        private string arinfolabelalignment;
        [Display(Name = "UDF_AR_Info_Label_Alignment", ShortName = "81")]
        public string ARInfoLabelALignment
        {
            get { return arinfolabelalignment; }
            set { arinfolabelalignment = value; }
        }

        private string rcinfolabelposition;
        [Display(Name = "UDF_RC_Info_Label_Position", ShortName = "82")]
        public string RCInfoLabelPosition
        {
            get { return rcinfolabelposition; }
            set { rcinfolabelposition = value; }
        }


        private string rcinfolabelalignment;
        [Display(Name = "UDF_RC_Info_Label_Alignment", ShortName = "83")]
        public string RCInfoLabelAlignment
        {
            get { return rcinfolabelalignment; }
            set { rcinfolabelalignment = value; }
        }


        private string covertitleorauthor;
        [Display(Name = "UDF_Cover_Title_Or_Author", ShortName = "98")]
        public string CoverTitleOrAuthor
        {
            get { return covertitleorauthor; }
            set { covertitleorauthor = value; }
        }



        private string Nonspinelabelcomment;
        [Display(Name = "UDF_Nonspine_Label_Comments", ShortName = "99")]
        public string NonSpineLabelComment
        {
            get { return Nonspinelabelcomment; }
            set { Nonspinelabelcomment = value; }
        }

        private string distancefrombottom;
        [Display(Name = "UDF_Distance_From_Bottom", ShortName = "73")]
        public string DistanceFromBottom
        {
            get { return distancefrombottom; }
            set { distancefrombottom = value; }
        }

        private string spinelabelcomments;
        [Display(Name = "UDF_Spine_Label_Comments", ShortName = "100")]
        public string SpineLabelComments
        {
            get { return spinelabelcomments; }
            set { spinelabelcomments = value; }
        }

        private string cardkit;
        [Display(Name = "UDF_Card_Kit", ShortName = "23")]
        public string CardKit
        {
            get { return cardkit; }
            set { cardkit = value; }
        }

        private string pocket;
        [Display(Name = "UDF_Pocket", ShortName = "94")]
        public string Pocket
        {
            get { return pocket; }
            set { pocket = value; }
        }

    }
}
